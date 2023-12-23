using System.Security.Claims;
using Iiroki.TimeSeriesPlatform.Constants;

namespace Iiroki.TimeSeriesPlatform.Extensions;

public static class ClaimExtensions
{
    public static string GetIntegrationSlug(this ClaimsPrincipal principal)
    {
        var kind = principal.Claims.FirstOrDefault(c => c.Type == AuthenticationClaim.Kind);
        if (kind?.Value == AuthenticationKind.Integration)
        {
            var id = principal.Claims.FirstOrDefault(c => c.Type == AuthenticationClaim.Id);
            return id?.Value ?? throw new InvalidOperationException("ID claim must be defined");
        }

        throw new InvalidOperationException($"Authentication kind claim must be '{AuthenticationKind.Integration}'");
    }
}
