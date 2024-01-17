using Iiroki.TimeSeriesPlatform.Models;

namespace Iiroki.TimeSeriesPlatform.Dto;

public record LocationBaseDto
{
    public required string Name { get; init; }

    public required string Slug { get; init; }

    public LocationType? Type { get; init; }
}
