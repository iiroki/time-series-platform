using Iiroki.TimeSeriesPlatform.Domain.Models;

namespace Iiroki.TimeSeriesPlatform.Domain.Services;

public interface IMeasurementService
{
    public Task SaveMeasurementsAsync(
        IList<MeasurementBatch> measurements,
        string integrationSlug,
        CancellationToken ct = default
    );
}
