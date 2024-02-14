using Iiroki.TimeSeriesPlatform.Api.Constants;
using Iiroki.TimeSeriesPlatform.Api.Extensions;
using Iiroki.TimeSeriesPlatform.Domain.Models;
using Iiroki.TimeSeriesPlatform.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Api.Controllers;

[ApiController]
[Route("measurement")]
[Authorize]
public class MeasurementController(IMeasurementService measurementService) : ControllerBase
{
    private readonly IMeasurementService _measurementService = measurementService;

    /// <summary>
    /// Ingests measurements (integration is resolved from the API key).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = AuthenticationKind.Integration)]
    public async Task<ActionResult> SaveAsync(IList<MeasurementBatch> measurements, CancellationToken ct)
    {
        await _measurementService.SaveMeasurementsAsync(measurements, HttpContext.User.GetIntegrationSlug(), ct);
        return new StatusCodeResult(StatusCodes.Status204NoContent);
    }
}
