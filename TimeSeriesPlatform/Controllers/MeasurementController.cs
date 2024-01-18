using Iiroki.TimeSeriesPlatform.Constants;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Models.Dto;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Controllers;

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
