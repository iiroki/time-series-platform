namespace Iiroki.TimeSeriesPlatform.Models;

public record Location : LocationData
{
    public required string Id { get; init; }
}
