namespace Iiroki.TimeSeriesPlatform.Dto;

public record MeasurementDataDto
{
    public required double Value { get; init; }

    public required DateTime Timestamp { get; init; }
}
