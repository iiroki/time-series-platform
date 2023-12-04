namespace Iiroki.TimeSeriesPlatform.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly Dictionary<string, string> _integrationApiKeys = new();
    private readonly string _adminApiKey;
    private readonly string _readerApiKey;

    public ApiKeyService(IConfiguration config)
    {
        var integrationApiKeys = config.GetSection(Config.ApiKeyIntegration).GetChildren().ToList();
        foreach (var apiKey in integrationApiKeys)
        {
            if (!string.IsNullOrWhiteSpace(apiKey.Value))
            {
                _integrationApiKeys.Add(apiKey.Key, apiKey.Value);
            }
        }

        _adminApiKey = config.GetRequired(Config.ApiKeyAdmin);
        _readerApiKey = config.GetRequired(Config.ApiKeyReader);
    }

    public string? GetIntegrationFromApiKey(string apiKey) =>
        _integrationApiKeys.TryGetValue(apiKey, out var slug) ? slug : null;

    public bool IsAdminApiKey(string apiKey) => apiKey == _adminApiKey;

    public bool IsReaderApiKey(string apiKey) => apiKey == _readerApiKey;
}
