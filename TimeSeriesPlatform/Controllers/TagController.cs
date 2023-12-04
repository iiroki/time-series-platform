using Iiroki.TimeSeriesPlatform.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Controllers;

[ApiController]
[Route("tag")]
[Authorize(Roles = "reader")]
public class TagController : ControllerBase
{
    [HttpGet]
    public IEnumerable<TagDto> Get()
    {
        return Enumerable.Range(1, 5).Select(i => new TagDto { Id = i, Name = $"Tag {i}" });
    }
}
