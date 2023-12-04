namespace Iiroki.TimeSeriesPlatform.Services;

public interface IApiKeyService
{
    public string? GetIntegrationFromApiKey(string apiKey);

    public bool IsAdminApiKey(string apiKey);

    public bool IsReaderApiKey(string apiKey);
}
