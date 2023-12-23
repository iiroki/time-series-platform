using Iiroki.TimeSeriesPlatform.Constants;
using Iiroki.TimeSeriesPlatform.Dto;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Controllers;

[ApiController]
[Route("integration")]
[Authorize]
public class IntegrationController : ControllerBase
{
    private readonly IMetadataService _metadataService;

    public IntegrationController(IMetadataService metadataService)
    {
        _metadataService = metadataService;
    }

    [HttpGet]
    [Authorize(Roles = AuthenticationKind.ReaderOrAdmin)]
    public async Task<List<IntegrationDto>> Get(CancellationToken ct)
    {
        var integrations = await _metadataService.GetIntegrationsAsync(ct);
        return integrations.Select(i => new IntegrationDto{ Id = i.Id, Name = i.Name, Slug = i.Slug }).ToList();
    }

    [HttpPost]
    [Authorize(Roles = AuthenticationKind.Admin)]
    public async Task<ActionResult<IntegrationDto>> Create(
        [FromBody] IntegrationCreateDto data,
        CancellationToken ct
    )
    {
        var integration = await _metadataService.CreateIntegrationAsync(data.Name, data.Slug, ct);
        return new IntegrationDto
        {
            Id = integration.Id,
            Name = integration.Name,
            Slug = integration.Name
        };
    }
}
