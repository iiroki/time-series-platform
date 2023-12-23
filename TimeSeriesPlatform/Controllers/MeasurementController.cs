using Iiroki.TimeSeriesPlatform.Constants;
using Iiroki.TimeSeriesPlatform.Dto;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iiroki.TimeSeriesPlatform.Controllers;

[ApiController]
[Route("measurement")]
[Authorize]
public class MeasurementController : ControllerBase
{
    private readonly IMeasurementService _measurementService;

    public MeasurementController(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
    }

    [HttpPost]
    [Authorize(Roles = AuthenticationKind.Integration)]
    public async Task SaveAsync(IList<MeasurementDto> measurements, CancellationToken ct) =>
        await _measurementService.SaveMeasurementsAsync(measurements, HttpContext.User.GetIntegrationSlug(), ct);
}
