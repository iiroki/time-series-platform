using Iiroki.TimeSeriesPlatform.Core.Models.Dto;

namespace Iiroki.TimeSeriesPlatform.Core.Services;

public interface IMeasurementService
{
    public Task SaveMeasurementsAsync(
        IList<MeasurementBatch> measurements,
        string integrationSlug,
        CancellationToken ct = default
    );
}
