using Iiroki.TimeSeriesPlatform.Constants;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Models;
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
    public async Task<ActionResult<List<Location>>> GetAsync(CancellationToken ct) =>
        (await _metadataService.GetLocationsAsync(ct)).ToDto().ToList();

    /// <summary>
    /// Creates a new location.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Location>> CreateAsync([FromBody] LocationData data, CancellationToken ct) =>
        (await _metadataService.CreateLocationAsync(data, ct)).ToDto();

    /// <summary>
    /// Deletes a location.
    /// </summary>
    [HttpDelete(":id")]
    public async Task<ActionResult> DeleteAsync(long id, CancellationToken ct) =>
        await _metadataService.DeleteLocationAsync(id, ct)
            ? new StatusCodeResult(StatusCodes.Status204NoContent)
            : new StatusCodeResult(StatusCodes.Status404NotFound);
}
