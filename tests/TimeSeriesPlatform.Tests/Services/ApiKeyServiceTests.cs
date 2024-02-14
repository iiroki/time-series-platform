using Iiroki.TimeSeriesPlatform.Api;
using Iiroki.TimeSeriesPlatform.Api.Services;
using Microsoft.Extensions.Configuration;

namespace Iiroki.TimeSeriesPlatform.Tests.Services;

public class ApiKeyServiceTests
{
    [Test]
    public void ApiKeyService_IsAdminApiKey_Ok()
    {
        var apiKey = Guid.NewGuid().ToString();
        var config = InitConfig(adminApiKey: apiKey);
        var apiKeyService = new ApiKeyService(config);
        var result = apiKeyService.IsAdminApiKey(apiKey);
        Assert.That(result, Is.True);
    }

    [Test]
    public void ApiKeyService_IsAdminApiKey_Unknown()
    {
        var config = InitConfig();
        var apiKeyService = new ApiKeyService(config);
        var result = apiKeyService.IsAdminApiKey(Guid.NewGuid().ToString());
        Assert.That(result, Is.False);
    }

    [Test]
    public void ApiKeyService_IsReaderApiKey_Ok()
    {
        var apiKey = Guid.NewGuid().ToString();
        var config = InitConfig(readerApiKey: apiKey);
        var apiKeyService = new ApiKeyService(config);
        var result = apiKeyService.IsReaderApiKey(apiKey);
        Assert.That(result, Is.True);
    }

    [Test]
    public void ApiKeyService_IsAReaderApiKey_Unknown()
    {
        var config = InitConfig();
        var apiKeyService = new ApiKeyService(config);
        var result = apiKeyService.IsAdminApiKey(Guid.NewGuid().ToString());
        Assert.That(result, Is.False);
    }

    [Test]
    public void ApiKeyService_GetIntegrationFromApiKey_Ok()
    {
        var integrations = new[]
        {
            ("first-integration", Guid.NewGuid().ToString()),
            ("second-integration", Guid.NewGuid().ToString())
        };

        var config = InitConfig(integrationApiKeys: integrations);
        var apiKeyService = new ApiKeyService(config);
        Assert.Multiple(() =>
        {
            foreach (var i in integrations)
            {
                var result = apiKeyService.GetIntegrationFromApiKey(i.Item2);
                Assert.That(result, Is.EqualTo(i.Item1));
            }
        });
    }

    [Test]
    public void ApiKeyService_GetIntegrationFromApiKey_Unknown()
    {
        var config = InitConfig();
        var apiKeyService = new ApiKeyService(config);
        var result = apiKeyService.GetIntegrationFromApiKey(Guid.NewGuid().ToString());
        Assert.That(result, Is.Null);
    }

    private static IConfiguration InitConfig(
        string? adminApiKey = null,
        string? readerApiKey = null,
        IList<(string, string)>? integrationApiKeys = null
    )
    {
        Environment.SetEnvironmentVariable(Config.ApiKeyAdmin, adminApiKey ?? Guid.NewGuid().ToString());
        Environment.SetEnvironmentVariable(Config.ApiKeyReader, readerApiKey ?? Guid.NewGuid().ToString());
        if (integrationApiKeys != null)
        {
            foreach (var i in integrationApiKeys)
            {
                Environment.SetEnvironmentVariable($"{Config.ApiKeyIntegration}__{i.Item1}", i.Item2);
            }
        }

        return new ConfigurationBuilder().AddEnvironmentVariables().Build();
    }
}
