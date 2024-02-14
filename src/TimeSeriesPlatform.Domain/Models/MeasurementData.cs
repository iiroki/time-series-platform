namespace Iiroki.TimeSeriesPlatform.Domain.Models;

public record MeasurementData
{
    public required double Value { get; init; }

    public required DateTime Timestamp { get; init; }
}
