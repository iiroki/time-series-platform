namespace Iiroki.TimeSeriesPlatform.Models;

public record IntegrationData
{
    public required string Name { get; init; }

    public required string Slug { get; init; }
}
