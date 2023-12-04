namespace Iiroki.TimeSeriesPlatform;

public static class Config
{
    public const string DatabaseUrl = "DATABASE_URL";

    public const string ApiKey = "API_KEY";
    public const string ApiKeyIntegration = $"{ApiKey}_INTEGRATION";
    public const string ApiKeyAdmin = $"{ApiKey}_ADMIN";
    public const string ApiKeyReader = $"{ApiKey}_READER";
}
