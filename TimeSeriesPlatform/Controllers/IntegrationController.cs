using Iiroki.TimeSeriesPlatform.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Controllers;

[ApiController]
[Route("integration")]
public class IntegrationController : ControllerBase
{
    [HttpGet]
    public IEnumerable<IntegrationDto> Get()
    {
        return Enumerable.Range(1, 5).Select(i => new IntegrationDto { Id = i, Name = $"Integration {i}" });
    }
}
