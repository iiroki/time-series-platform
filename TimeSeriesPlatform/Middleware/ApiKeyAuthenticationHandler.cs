using System.Security.Claims;
using System.Text.Encodings.Web;
using Iiroki.TimeSeriesPlatform.Constants;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Iiroki.TimeSeriesPlatform.Middleware;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string ApiKeyHeader = "X-API-KEY";

    private readonly IApiKeyService _apiKeyService;

    public ApiKeyAuthenticationHandler(
        IApiKeyService apiKeyService,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
    )
        : base(options, logger, encoder)
    {
        _apiKeyService = apiKeyService;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var apiKey = Context.Request.Headers[ApiKeyHeader].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim>();
        if (_apiKeyService.IsAdminApiKey(apiKey))
        {
            claims.Add(new(AuthenticationClaim.Kind, AuthenticationKind.Admin));
        }
        else if (_apiKeyService.IsReaderApiKey(apiKey))
        {
            claims.Add(new(AuthenticationClaim.Kind, AuthenticationKind.Reader));
        }
        else
        {
            var integration = _apiKeyService.GetIntegrationFromApiKey(apiKey);
            if (!string.IsNullOrWhiteSpace(integration))
            {
                claims.Add(new(AuthenticationClaim.Kind, AuthenticationKind.Integration));
                claims.Add(new(AuthenticationClaim.Id, integration));
            }
        }

        if (claims.Count == 0)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API key"));
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name, AuthenticationClaim.Id, AuthenticationClaim.Kind);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
