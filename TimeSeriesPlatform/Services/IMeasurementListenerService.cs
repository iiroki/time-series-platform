using Iiroki.TimeSeriesPlatform.Models;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IMeasurementListenerService
{
    public delegate void OnMeasurement(long tagId, double value);

    public Task StartAsync(CancellationToken ct);

    public IDisposable RegisterListener(Action<MeasurementChange> listener);
}
