using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Dto;

namespace Iiroki.TimeSeriesPlatform.Extensions;

public static class TransformerExtensions
{
    public static IntegrationDto ToDto(this IntegrationEntity integration) =>
        new()
        {
            Id = integration.Id,
            Name = integration.Name,
            Slug = integration.Slug
        };

    public static IEnumerable<IntegrationDto> ToDto(this IEnumerable<IntegrationEntity> integrations) =>
        integrations.Select(i => i.ToDto());

    // TODO: Tags

    public static LocationDto ToDto(this LocationEntity location) =>
        new()
        {
            Id = location.Id,
            Name = location.Name,
            Slug = location.Slug,
            Type = location.Type
        };

    public static IEnumerable<LocationDto> ToDto(this IEnumerable<LocationEntity> locations) =>
        locations.Select(l => l.ToDto());
}
