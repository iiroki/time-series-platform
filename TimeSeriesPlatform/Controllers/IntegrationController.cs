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
    public IEnumerable<IntegrationDto> Get()
    {
        return new List<IntegrationDto>();
    }

    [HttpPost]
    [Authorize(Roles = AuthenticationKind.Admin)]
    public async Task<ActionResult<IntegrationDto>> Create(
        [FromBody] string name,
        [FromBody] string slug,
        CancellationToken ct
    )
    {
        var integration = await _metadataService.CreateIntegrationAsync(name, slug, ct);
        return new IntegrationDto
        {
            Id = integration.Id,
            Name = integration.Name,
            Slug = integration.Name
        };
    }
}
