namespace Iiroki.TimeSeriesPlatform.Dto;

public record IntegrationCreateDto
{
    public required string Name { get; init; }

    public required string Slug { get; init; }
}
