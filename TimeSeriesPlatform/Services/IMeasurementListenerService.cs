using Iiroki.TimeSeriesPlatform.Models.Internal;

namespace Iiroki.TimeSeriesPlatform.Services;

public interface IMeasurementListenerService
{
    public Task RunAsync(CancellationToken ct);

    public void RegisterListener(Action<MeasurementChange> listener);
}
