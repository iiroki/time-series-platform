using Iiroki.TimeSeriesPlatform.Constants;
using Iiroki.TimeSeriesPlatform.Models;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Controllers;

[ApiController]
[Route("integration")]
public class IntegrationController(IMetadataService metadataService) : ControllerBase
{
    private readonly IMetadataService _metadataService = metadataService;

    /// <summary>
    /// Gets all integrations.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = AuthenticationKind.ReaderOrAdmin)]
    public async Task<ActionResult<List<Integration>>> GetAsync(CancellationToken ct) =>
        await _metadataService.GetIntegrationsAsync(ct);

    /// <summary>
    /// Creates a new integration.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = AuthenticationKind.Admin)]
    public async Task<ActionResult<Integration>> CreateAsync([FromBody] IntegrationData data, CancellationToken ct) =>
        await _metadataService.CreateIntegrationAsync(data, ct);

    /// <summary>
    /// Deletes an integration.
    /// </summary>
    [HttpDelete(":id")]
    [Authorize(Roles = AuthenticationKind.Admin)]
    public async Task<ActionResult> DeleteAsync(string id, CancellationToken ct) =>
        await _metadataService.DeleteIntegrationAsync(id, ct)
            ? new StatusCodeResult(StatusCodes.Status204NoContent)
            : new StatusCodeResult(StatusCodes.Status404NotFound);
}
