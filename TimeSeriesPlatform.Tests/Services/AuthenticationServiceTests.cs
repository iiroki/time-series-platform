using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.Extensions.Configuration;

namespace Iiroki.TimeSeriesPlatform.Tests.Servces;

public class AuthenticationServiceTests
{
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void AuthenticationService_IsAdminApiKey_Ok(bool shouldMatch)
    {
        var apiKey = Guid.NewGuid().ToString();
        var config = InitConfig(adminApiKey: apiKey);
        var authService = new ApiKeyService(config);
        var result = authService.IsAdminApiKey(shouldMatch ? apiKey : Guid.NewGuid().ToString());
        Assert.That(result, Is.EqualTo(shouldMatch));
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void AuthenticationService_IsReaderApiKey_Ok(bool shouldMatch)
    {
        var apiKey = Guid.NewGuid().ToString();
        var config = InitConfig(readerApiKey: apiKey);
        var authService = new ApiKeyService(config);
        var result = authService.IsReaderApiKey(shouldMatch ? apiKey : Guid.NewGuid().ToString());
        Assert.That(result, Is.EqualTo(shouldMatch));
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void AuthenticationService_GetIntegrationFromApiKey_Ok(bool shouldMatch)
    {
        var apiKeys = new[]
        {
            ("first-integration", Guid.NewGuid().ToString()),
            ("second-integration", Guid.NewGuid().ToString())
        };

        var config = InitConfig(integrationApiKeys: apiKeys);
        var authService = new ApiKeyService(config);
        // var result = authService.IsReaderApiKey(shouldMatch ? apiKey : Guid.NewGuid().ToString());
        // Assert.That(result, Is.EqualTo(shouldMatch));
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

        var builder = new ConfigurationBuilder();
        builder.AddEnvironmentVariables();
        return builder.Build();
    }
}
