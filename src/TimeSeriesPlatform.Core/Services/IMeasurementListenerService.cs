using Iiroki.TimeSeriesPlatform.Core.Models.Internal;

namespace Iiroki.TimeSeriesPlatform.Core.Services;

public interface IMeasurementListenerService
{
    public Task RunAsync(CancellationToken ct);

    public void RegisterListener(Action<MeasurementChange> listener);
}
