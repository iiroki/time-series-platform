namespace Iiroki.TimeSeriesPlatform.Dto;

public record IntegrationDto
{
    public required long Id { get; init; }

    public required string Name { get; init; }

    public required string Slug { get; init; }
}
