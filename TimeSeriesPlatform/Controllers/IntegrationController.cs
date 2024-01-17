using Iiroki.TimeSeriesPlatform.Constants;
using Iiroki.TimeSeriesPlatform.Dto;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Controllers;

[ApiController]
[Route("integration")]
[Authorize(Roles = AuthenticationKind.Admin)]
public class IntegrationController(IMetadataService metadataService) : ControllerBase
{
    private readonly IMetadataService _metadataService = metadataService;

    /// <summary>
    /// Gets all integrations.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = AuthenticationKind.ReaderOrAdmin)]
    public async Task<List<IntegrationDto>> GetAsync(CancellationToken ct)
    {
        var integrations = await _metadataService.GetIntegrationsAsync(ct);
        return integrations.ToDto().ToList();
    }

    /// <summary>
    /// Creates a new integration.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<IntegrationDto>> CreateAsync(
        [FromBody] IntegrationCreateDto data,
        CancellationToken ct
    )
    {
        var integration = await _metadataService.CreateIntegrationAsync(data.Name, data.Slug, ct);
        return integration.ToDto();
    }
}
