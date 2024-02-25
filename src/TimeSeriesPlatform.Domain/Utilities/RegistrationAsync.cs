namespace Iiroki.TimeSeriesPlatform.Domain.Util;

public sealed class RegistrationAsync(Func<Task> disposeAction) : IAsyncDisposable
{
    private readonly Func<Task> _disposeAction = disposeAction;

    public async ValueTask DisposeAsync()
    {
        await _disposeAction();
    }
}
