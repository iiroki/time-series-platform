using Iiroki.TimeSeriesPlatform.Dto;
using Npgsql;

namespace Iiroki.TimeSeriesPlatform.Services;

public class MeasurementService : IMeasurementService
{
    private readonly NpgsqlDataSource _dbSource;
    private readonly ILogger<MeasurementService> _logger;

    public MeasurementService(NpgsqlDataSource dbSource, ILogger<MeasurementService> logger)
    {
        _dbSource = dbSource;
        _logger = logger;
    }

    public async Task SaveMeasurementsAsync(
        IList<MeasurementDto> measurements,
        string integrationSlug,
        CancellationToken ct
    )
    {
        _logger.LogDebug(
            "{I} - Saving {M} measurements for {T} tag(s)...",
            integrationSlug,
            measurements.Sum(m => m.Data.Count),
            measurements.Count
        );

        await using var conn = await _dbSource.OpenConnectionAsync(ct);
        await using var trans = await conn.BeginTransactionAsync(ct);

        // TODO: Save the measurements

        await trans.CommitAsync(ct);
    }
}
