using System.Diagnostics;
using Iiroki.TimeSeriesPlatform.Application.Extensions;
using Iiroki.TimeSeriesPlatform.Domain.Models;
using Iiroki.TimeSeriesPlatform.Domain.Services;
using Iiroki.TimeSeriesPlatform.Domain.Services.Exceptions;
using Iiroki.TimeSeriesPlatform.Infrastructure.Database.Queries;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Services;

internal class MeasurementService : IMeasurementService
{
    private readonly NpgsqlDataSource _dbSource;
    private readonly ILogger<MeasurementService> _logger;

    public MeasurementService(NpgsqlDataSource dbSource, ILogger<MeasurementService> logger)
    {
        _dbSource = dbSource;
        _logger = logger;
    }

    public async Task SaveMeasurementsAsync(
        IList<MeasurementBatch> measurements,
        string integrationSlug,
        CancellationToken ct
    )
    {
        var measurementCount = measurements.Sum(m => m.Data.Count);
        _logger.LogDebug(
            "{I} - Saving {M} measurements for {T} tag(s)...",
            integrationSlug,
            measurementCount,
            measurements.Count
        );

        var timer = Stopwatch.StartNew();
        await using var conn = await _dbSource.OpenConnectionAsync(ct);
        await using var trans = await conn.BeginTransactionAsync(ct);

        foreach (var m in measurements)
        {
            await using var cmd = new NpgsqlCommand(MeasurementQueries.Upsert(m.Data.Count), conn, trans);
            var @params = m.Data
                .SelectMany(
                    d =>
                        new List<NpgsqlParameter>
                        {
                            new() { Value = d.Timestamp.EnsureUtc() },
                            new() { Value = d.Value }
                        }
                )
                .ToArray();

            cmd.Parameters.Add(new() { Value = integrationSlug });
            cmd.Parameters.Add(new() { Value = m.Tag });
            cmd.Parameters.Add(new() { Value = !string.IsNullOrWhiteSpace(m.Location) ? m.Location : DBNull.Value });
            cmd.Parameters.Add(new() { Value = m.VersionTimestamp ?? DateTime.UtcNow });
            cmd.Parameters.AddRange(@params);
            try
            {
                await cmd.ExecuteNonQueryAsync(ct);
            }
            catch (Exception ex)
            {
                throw new MeasurementServiceException("Could not save measurements", ex);
            }
        }

        await trans.CommitAsync(ct);
        _logger.LogInformation(
            "{I} - Saved {M} measurements for {T} tag(s) in {Ms} ms",
            integrationSlug,
            measurementCount,
            measurements.Count,
            timer.Elapsed.Milliseconds
        );
    }
}
