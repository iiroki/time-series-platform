using Iiroki.TimeSeriesPlatform.Api.Constants;
using Iiroki.TimeSeriesPlatform.Api.Extensions;
using Iiroki.TimeSeriesPlatform.Domain.Models;
using Iiroki.TimeSeriesPlatform.Domain.Services;
using JsonRpcX.Attributes;
using JsonRpcX.Controllers;
using JsonRpcX.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Iiroki.TimeSeriesPlatform.Api.Controllers.JsonRpc;

[Authorize(Roles = AuthenticationKind.Integration)]
public class JsonRpcMeasurementController(
    JsonRpcContext ctx,
    IMeasurementService measurementService
) : IJsonRpcController
{
    private readonly JsonRpcContext _ctx = ctx;
    private readonly IMeasurementService _measurementService = measurementService;

    [JsonRpcMethod]
    public async Task SaveMeasurementsAsync(MeasurementBatch[] measurements, CancellationToken ct = default)
    {
        await _measurementService.SaveMeasurementsAsync(measurements, _ctx.User.GetIntegrationSlug(), ct);
    }
}
