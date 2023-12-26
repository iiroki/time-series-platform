namespace Iiroki.TimeSeriesPlatform.Util;

public class RegistrationAsync : IAsyncDisposable
{
    private readonly Func<Task> _disposeAction;

    public RegistrationAsync(Func<Task> disposeAction)
    {
        _disposeAction = disposeAction;
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await _disposeAction();
    }
}
