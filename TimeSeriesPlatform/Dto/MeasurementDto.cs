namespace Iiroki.TimeSeriesPlatform.Dto;

public record MeasurementDto
{
    public required string Tag { get; init; }

    public string? Location { get; init; }

    public required IList<MeasurementDataDto> Data { get; init; }

    public DateTime? VersionTimestamp { get; init; }

    public record MeasurementDataDto
    {
        public required double Value { get; init; }

        public required DateTime Timestamp { get; init; }
    }
}
