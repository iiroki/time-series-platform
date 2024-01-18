namespace Iiroki.TimeSeriesPlatform.Models.Dto;

public record MeasurementData
{
    public required double Value { get; init; }

    public required DateTime Timestamp { get; init; }
}
