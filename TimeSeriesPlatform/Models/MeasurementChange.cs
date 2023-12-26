namespace Iiroki.TimeSeriesPlatform.Models;

public record MeasurementChange
{
    public required long IntegrationId { get; init; }

    public required long TagId { get; init; }

    public required DateTime Timestamp { get; init; }

    public required double Value { get; init; }

    public class Builder
    {
        public long? IntegrationId { get; set; }
        public long? TagId { get; set; }
        public DateTime? Timestamp { get; set; }
        public double? Value { get; set; }

        public MeasurementChange? Build() =>
            IntegrationId.HasValue && TagId.HasValue && Timestamp.HasValue && Value.HasValue
                ? new MeasurementChange
                {
                    IntegrationId = IntegrationId.Value,
                    TagId = TagId.Value,
                    Timestamp = Timestamp.Value,
                    Value = Value.Value
                }
                : null;
    }
}
