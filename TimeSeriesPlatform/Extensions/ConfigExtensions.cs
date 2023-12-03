namespace Iiroki.TimeSeriesPlatform;

public static class ConfigExtensions
{
    public static string GetRequired(this IConfiguration config, string key)
    {
        var value = config[key];
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Required configuration is missing with key: {key}");
        }

        return value;
    }
}
