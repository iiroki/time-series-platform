using Iiroki.TimeSeriesPlatform.Api.Constants;
using Iiroki.TimeSeriesPlatform.Core.Models;
using Iiroki.TimeSeriesPlatform.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Api.Controllers;

[ApiController]
[Route("tag")]
public class TagController(IMetadataService metadataService) : ControllerBase
{
    private readonly IMetadataService _metadataService = metadataService;

    /// <summary>
    /// Gets all tags.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = AuthenticationKind.ReaderOrAdmin)]
    public async Task<ActionResult<List<Tag>>> GetAsync(CancellationToken ct) =>
        await _metadataService.GetTagsAsync(ct);

    /// <summary>
    /// Creates a new tag.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = AuthenticationKind.Admin)]
    public async Task<ActionResult<Tag>> CreateAsync([FromBody] TagData data, CancellationToken ct) =>
        await _metadataService.CreateTagAsync(data, ct);

    /// <summary>
    /// Deletes a tag.
    /// </summary>
    [HttpDelete(":id")]
    [Authorize(Roles = AuthenticationKind.Admin)]
    public async Task<ActionResult> DeleteAsync(string id, CancellationToken ct) =>
        await _metadataService.DeleteTagAsync(id, ct)
            ? new StatusCodeResult(StatusCodes.Status204NoContent)
            : new StatusCodeResult(StatusCodes.Status404NotFound);
}
