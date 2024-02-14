namespace Iiroki.TimeSeriesPlatform.Api.Services;

public interface IApiKeyService
{
    public string? GetIntegrationFromApiKey(string apiKey);

    public bool IsAdminApiKey(string apiKey);

    public bool IsReaderApiKey(string apiKey);
}
