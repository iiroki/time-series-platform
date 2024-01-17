using Iiroki.TimeSeriesPlatform.Dto;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IMeasurementService
{
    public Task SaveMeasurementsAsync(
        IList<MeasurementBatchDto> measurements,
        string integrationSlug,
        CancellationToken ct
    );
}
