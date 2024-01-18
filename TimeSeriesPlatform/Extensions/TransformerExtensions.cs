using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Models;

namespace Iiroki.TimeSeriesPlatform.Extensions;

public static class TransformerExtensions
{
    public static Integration ToDto(this IntegrationEntity integration) =>
        new()
        {
            Id = integration.Id,
            Name = integration.Name,
            Slug = integration.Slug
        };

    public static IEnumerable<Integration> ToDto(this IEnumerable<IntegrationEntity> integrations) =>
        integrations.Select(i => i.ToDto());

    public static Tag ToDto(this TagEntity tag) =>
        new()
        {
            Id = tag.Id,
            Name = tag.Name ?? tag.Slug, // Fallback if the tag was created automatically
            Slug = tag.Slug
        };

    public static IEnumerable<Tag> ToDto(this IEnumerable<TagEntity> tags) => tags.Select(t => t.ToDto());

    public static Location ToDto(this LocationEntity location) =>
        new()
        {
            Id = location.Id,
            Name = location.Name,
            Slug = location.Slug,
            Type = location.Type
        };

    public static IEnumerable<Location> ToDto(this IEnumerable<LocationEntity> locations) =>
        locations.Select(l => l.ToDto());
}
