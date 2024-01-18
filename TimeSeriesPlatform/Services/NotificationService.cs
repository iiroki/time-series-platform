using System.Text.Json;
using Iiroki.TimeSeriesPlatform.Models.Internal;

namespace Iiroki.TimeSeriesPlatform.Services;

public class NotificationService : BackgroundService
{
    private readonly IMeasurementListenerService _measurementListenerService;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IMeasurementListenerService measurementListenerService,
        ILogger<NotificationService> logger
    )
    {
        _measurementListenerService = measurementListenerService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        _measurementListenerService.RegisterListener(OnMeasurementChange);
        await _measurementListenerService.RunAsync(ct);
    }

    private void OnMeasurementChange(MeasurementChange change)
    {
        _logger.LogInformation("Received change: {C}", JsonSerializer.Serialize(change));
    }
}
