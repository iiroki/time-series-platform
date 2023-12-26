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
    private static readonly string Id = Guid.NewGuid().ToString().Replace('-', '_');
    private readonly string PubName = $"_pub_measurement_listener";
    private readonly string SlotName = $"_slot_measurement_listener";

    private readonly IConfiguration _config;
    private readonly NpgsqlDataSource _dbSource;
    private readonly ILogger<MeasurementListenerService> _logger;

    private readonly Dictionary<Guid, Action<MeasurementChange>> _listeners = [];

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

    public async Task StartAsync(CancellationToken ct)
    {
        // Reference: https://www.npgsql.org/doc/replication.html#logical-replication

        await using var pubRegistration = await CreatePubAsync(ct);
        await using var slotRegistration = await CreateSlotAsync(ct);

        var connection = _config.GetRequired(Config.DatabaseUrl);
        await using var dbConnection = new LogicalReplicationConnection(connection);
        var slot = new PgOutputReplicationSlot(SlotName);

        await dbConnection.Open(ct);

        _logger.LogInformation("Starting Postgres replication with 'pgoutput'...");
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

            dbConnection.SetReplicationStatus(msg.WalEnd);
            if (change != null)
            {
                foreach (var l in _listeners)
                {
                    l.Value(change);
                }
            }
        }

        _logger.LogDebug("Postgres replication finished");
    }

    IDisposable IMeasurementListenerService.RegisterListener(Action<MeasurementChange> listener)
    {
        var listenerId = Guid.NewGuid();
        _listeners.Add(listenerId, listener);
        return new Registration(() => _listeners.Remove(listenerId));
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
            await using var cmd = _dbSource.CreateCommand($"SELECT pg_drop_replication_slot('{SlotName}')");
            await cmd.ExecuteNonQueryAsync(ct);
            _logger.LogDebug("Deleted Postgres replication slot: {P}", SlotName);
        });
    }
}
