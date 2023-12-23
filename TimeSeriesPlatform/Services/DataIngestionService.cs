using Iiroki.TimeSeriesPlatform.Dto;

namespace Iiroki.TimeSeriesPlatform.Services;

public class DataIngestionService : IMeasurementService
{
    public async Task SaveMeasurementsAsync(IList<MeasurementDto> measurements, CancellationToken ct)
    {
        // TODO
    }
}
