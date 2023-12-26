using System.Text.Json;
using Iiroki.TimeSeriesPlatform.Models;

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
        var recipe = _measurementListenerService.RegisterListener(OnMeasurementChange);
        await _measurementListenerService.StartAsync(ct);
        recipe.Dispose();
    }

    private void OnMeasurementChange(MeasurementChange change)
    {
        _logger.LogInformation("Measurement changed: {C}", JsonSerializer.Serialize(change));
    }
}
