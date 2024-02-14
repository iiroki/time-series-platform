using Iiroki.TimeSeriesPlatform.Utilities.Extensions;
using Microsoft.Extensions.Configuration;

namespace Iiroki.TimeSeriesPlatform.Api.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly Dictionary<string, string> _integrationApiKeys = []; // API key -> Slug
    private readonly string _adminApiKey;
    private readonly string _readerApiKey;

    public ApiKeyService(IConfiguration config)
    {
        // Try to parse the integration API keys from the raw keys...
        var integrationApiKeyPrefix = $"{Config.ApiKeyIntegration}__";
        var integrationApiKeys = config
            .AsEnumerable()
            .Where(c => c.Key.StartsWith(integrationApiKeyPrefix))
            .Select(c => KeyValuePair.Create(c.Key[integrationApiKeyPrefix.Length..], c.Value))
            .ToList();

        // ...if no integrations weren't found, try to read them from the configuration section.
        if (integrationApiKeys.Count == 0)
        {
            integrationApiKeys = config
                .GetSection(Config.ApiKeyIntegration)
                .GetChildren()
                .Select(c => KeyValuePair.Create(c.Key, c.Value))
                .ToList();
        }

        // Add the integration API keys to a dictionary.
        foreach (var apiKey in integrationApiKeys)
        {
            if (!string.IsNullOrWhiteSpace(apiKey.Value))
            {
                _integrationApiKeys.Add(apiKey.Value, apiKey.Key);
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
