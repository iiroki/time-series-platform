using Iiroki.TimeSeriesPlatform.Constants;
using Iiroki.TimeSeriesPlatform.Dto;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Controllers;

[ApiController]
[Route("location")]
[Authorize(Roles = AuthenticationKind.Admin)]
public class LocationController(IMetadataService metadataService) : ControllerBase
{
    private readonly IMetadataService _metadataService = metadataService;

    /// <summary>
    /// Gets all locations.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = AuthenticationKind.ReaderOrAdmin)]
    public async Task<List<LocationDto>> GetAsync(CancellationToken ct)
    {
        var locations = await _metadataService.GetLocationsAsync(ct);
        return locations.ToDto().ToList();
    }

    /// <summary>
    /// Creates a new location.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<LocationDto>> CreateAsync([FromBody] LocationCreateDto data, CancellationToken ct)
    {
        var location = await _metadataService.CreateLocationAsync(data.Name, data.Slug, data.Type, ct);
        return location.ToDto();
    }
}
