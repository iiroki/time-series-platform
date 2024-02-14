using Iiroki.TimeSeriesPlatform.Domain.Models.Internal;

namespace Iiroki.TimeSeriesPlatform.Domain.Services;

public interface IMeasurementListenerService
{
    public Task RunAsync(CancellationToken ct);

    public void RegisterListener(Action<MeasurementChange> listener);
}
