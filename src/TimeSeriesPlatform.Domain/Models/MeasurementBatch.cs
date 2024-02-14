namespace Iiroki.TimeSeriesPlatform.Domain.Models;

public record MeasurementBatch
{
    /// <summary>(Tag slug)</summary>
    public required string Tag { get; init; }

    /// <summary>(Location slug)</summary>
    public string? Location { get; init; }

    public required IList<MeasurementData> Data { get; init; }

    public DateTime? VersionTimestamp { get; init; }
}
