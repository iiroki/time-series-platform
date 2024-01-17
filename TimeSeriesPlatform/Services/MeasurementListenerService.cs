using Iiroki.TimeSeriesPlatform.Database;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Models;
using Iiroki.TimeSeriesPlatform.Util;
using Npgsql;
using Npgsql.Replication;
using Npgsql.Replication.PgOutput;
using Npgsql.Replication.PgOutput.Messages;

namespace Iiroki.TimeSeriesPlatform.Services;

public class MeasurementListenerService : IMeasurementListenerService
{
    private readonly string PubName = $"_pub_measurement_listener";
    private readonly string SlotName = $"_slot_measurement_listener";

    private readonly IConfiguration _config;
    private readonly NpgsqlDataSource _dbSource;
    private readonly ILogger<MeasurementListenerService> _logger;

    private Action<MeasurementChange>? _listener;
    private bool _isRunning;

    public MeasurementListenerService(
        IConfiguration config,
        NpgsqlDataSource dbSource,
        ILogger<MeasurementListenerService> logger
    )
    {
        _config = config;
        _dbSource = dbSource;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        if (_listener == null)
        {
            throw new InvalidOperationException("Listener is not registered");
        }

        if (_isRunning)
        {
            throw new InvalidOperationException("Service is already running");
        }

        _isRunning = true;

        // Reference: https://www.npgsql.org/doc/replication.html#logical-replication

        await using var pubRegistration = await CreatePubAsync(ct);
        await using var slotRegistration = await CreateSlotAsync(ct);

        var connection = _config.GetRequired(Config.DatabaseUrl);
        await using var dbConnection = new LogicalReplicationConnection(connection);
        var slot = new PgOutputReplicationSlot(SlotName);

        _logger.LogInformation("Starting Postgres replication with 'pgoutput'");
        await dbConnection.Open(ct);
        await foreach (
            var msg in dbConnection.StartReplication(
                slot,
                new PgOutputReplicationOptions(PubName, 1, messages: true),
                ct
            )
        )
        {
            MeasurementChange? change = null;
            if (msg is InsertMessage insert)
            {
                change = await insert.ToMeasurementChange();
            }
            else if (msg is UpdateMessage update)
            {
                change = await update.ToMeasurementChange();
            }

            if (change != null)
            {
                _listener(change); // Should the changes be batches with "BeginMessage" and "CommitMessage"?
            }

            dbConnection.SetReplicationStatus(msg.WalEnd);
        }

        _logger.LogDebug("Postgres replication finished");
    }

    public void RegisterListener(Action<MeasurementChange> listener)
    {
        _listener = listener;
    }

    private async Task<IAsyncDisposable> CreatePubAsync(CancellationToken ct)
    {
        await using var dropCmd = _dbSource.CreateCommand($"DROP PUBLICATION IF EXISTS {PubName}");
        await dropCmd.ExecuteNonQueryAsync(ct);

        await using var createCmd = _dbSource.CreateCommand(
            $"""
            CREATE PUBLICATION {PubName}
            FOR TABLE {TspDbContext.ToTableName(nameof(TspDbContext.Measurement))};
            """
        );

        await createCmd.ExecuteNonQueryAsync(ct);
        _logger.LogDebug("Created Postgres publication: {P}", PubName);
        return new RegistrationAsync(async () =>
        {
            await using var dropCmd = _dbSource.CreateCommand($"DROP PUBLICATION IF EXISTS {PubName}");
            await dropCmd.ExecuteNonQueryAsync(ct);
            _logger.LogDebug("Deleted Postgres publication: {P}", PubName);
        });
    }

    private async Task<IAsyncDisposable> CreateSlotAsync(CancellationToken ct)
    {
        await using var dropCmd = _dbSource.CreateCommand($"SELECT pg_drop_replication_slot('{SlotName}')");
        try
        {
            await dropCmd.ExecuteNonQueryAsync(ct);
        }
        catch (PostgresException ex)
        {
            // 42704 = The replication slot did not exist
            if (ex.SqlState != "42704")
            {
                throw;
            }
        }

        await using var createCmd = _dbSource.CreateCommand(
            $"""
            SELECT pg_create_logical_replication_slot('{SlotName}', 'pgoutput');
            """
        );

        await createCmd.ExecuteNonQueryAsync(ct);
        _logger.LogDebug("Created Postgres replication slot: {P}", SlotName);
        return new RegistrationAsync(async () =>
        {
            await using var dropCmd = _dbSource.CreateCommand($"SELECT pg_drop_replication_slot('{SlotName}')");
            await dropCmd.ExecuteNonQueryAsync(ct);
            _logger.LogDebug("Deleted Postgres replication slot: {P}", SlotName);
        });
    }
}
