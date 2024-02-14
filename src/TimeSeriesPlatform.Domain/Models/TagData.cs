namespace Iiroki.TimeSeriesPlatform.Domain.Models;

public record TagData
{
    public required string Name { get; init; }

    public required string Slug { get; init; }
}
