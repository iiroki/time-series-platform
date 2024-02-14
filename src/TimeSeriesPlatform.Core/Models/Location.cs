namespace Iiroki.TimeSeriesPlatform.Core.Models;

public record Location : LocationData
{
    public required string Id { get; init; }
}
