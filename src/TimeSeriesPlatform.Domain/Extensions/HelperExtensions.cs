namespace Iiroki.TimeSeriesPlatform.Application.Extensions;

public static class HelperExtensions
{
    public static string Stringify(this object obj)
    {
        var props = obj.GetType().GetProperties();
        return string.Join(", ", props.Select(p => $"{p.Name}: " + p.GetValue(obj)?.ToString() ?? "<null>"));
    }
}
