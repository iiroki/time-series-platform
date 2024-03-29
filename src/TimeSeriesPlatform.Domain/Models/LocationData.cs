namespace Iiroki.TimeSeriesPlatform.Domain.Models;

public record LocationData
{
    public required string Name { get; init; }

    public required string Slug { get; init; }

    public LocationType? Type { get; init; }
}
