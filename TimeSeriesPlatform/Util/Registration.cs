namespace Iiroki.TimeSeriesPlatform.Util;

public class Registration : IDisposable
{
    private readonly Action _disposeAction;

    public Registration(Action disposeAction)
    {
        _disposeAction = disposeAction;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _disposeAction();
    }
}
