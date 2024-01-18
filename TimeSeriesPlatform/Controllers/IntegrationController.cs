using Iiroki.TimeSeriesPlatform.Constants;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Models;
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
    public async Task<ActionResult<List<Integration>>> GetAsync(CancellationToken ct) =>
        (await _metadataService.GetIntegrationsAsync(ct)).ToDto().ToList();

    /// <summary>
    /// Creates a new integration.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Integration>> CreateAsync([FromBody] IntegrationData data, CancellationToken ct) =>
        (await _metadataService.CreateIntegrationAsync(data, ct)).ToDto();

    /// <summary>
    /// Deletes an integration.
    /// </summary>
    [HttpDelete(":id")]
    public async Task<ActionResult> DeleteAsync(long id, CancellationToken ct) =>
        await _metadataService.DeleteIntegrationAsync(id, ct)
            ? new StatusCodeResult(StatusCodes.Status204NoContent)
            : new StatusCodeResult(StatusCodes.Status404NotFound);
}
