using Iiroki.TimeSeriesPlatform.Api.Constants;
using Iiroki.TimeSeriesPlatform.Core.Models;
using Iiroki.TimeSeriesPlatform.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Api.Controllers;

[ApiController]
[Route("location")]
public class LocationController(IMetadataService metadataService) : ControllerBase
{
    private readonly IMetadataService _metadataService = metadataService;

    /// <summary>
    /// Gets all locations.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = AuthenticationKind.ReaderOrAdmin)]
    public async Task<ActionResult<List<Location>>> GetAsync(CancellationToken ct) =>
        await _metadataService.GetLocationsAsync(ct);

    /// <summary>
    /// Creates a new location.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = AuthenticationKind.Admin)]
    public async Task<ActionResult<Location>> CreateAsync([FromBody] LocationData data, CancellationToken ct) =>
        await _metadataService.CreateLocationAsync(data, ct);

    /// <summary>
    /// Deletes a location.
    /// </summary>
    [HttpDelete(":id")]
    [Authorize(Roles = AuthenticationKind.Admin)]
    public async Task<ActionResult> DeleteAsync(string id, CancellationToken ct) =>
        await _metadataService.DeleteLocationAsync(id, ct)
            ? new StatusCodeResult(StatusCodes.Status204NoContent)
            : new StatusCodeResult(StatusCodes.Status404NotFound);
}
