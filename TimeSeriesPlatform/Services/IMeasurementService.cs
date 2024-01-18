using Iiroki.TimeSeriesPlatform.Models.Dto;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IMeasurementService
{
    public Task SaveMeasurementsAsync(
        IList<MeasurementBatch> measurements,
        string integrationSlug,
        CancellationToken ct = default
    );
}
