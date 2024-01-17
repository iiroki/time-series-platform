namespace Iiroki.TimeSeriesPlatform.Dto;

public record MeasurementBatchDto
{
    /// <summary>(Tag slug)</summary>
    public required string Tag { get; init; }

    /// <summary>(Location slug)</summary>
    public string? Location { get; init; }

    public required IList<MeasurementDataDto> Data { get; init; }

    public DateTime? VersionTimestamp { get; init; }
}
