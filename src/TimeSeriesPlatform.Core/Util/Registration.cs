namespace Iiroki.TimeSeriesPlatform.Core.Util;

public sealed class Registration(Action disposeAction) : IDisposable
{
    private readonly Action _disposeAction = disposeAction;

    public void Dispose()
    {
        _disposeAction();
    }
}
