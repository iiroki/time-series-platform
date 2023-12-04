using Iiroki.TimeSeriesPlatform.Dto;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IDataIngestionService
{
    public Task SaveMeasurementsAsync(IList<MeasurementDto> measurements, CancellationToken ct);
}
