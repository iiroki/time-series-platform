using Iiroki.TimeSeriesPlatform.Core.Models;
using Iiroki.TimeSeriesPlatform.Infrastructure.Database.Entities;
using Sqids;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Extensions;

public static class TransformerExtensions
{
    public static Integration ToDto(this IntegrationEntity integration, SqidsEncoder<long> sqids) =>
        new()
        {
            Id = sqids.Encode(integration.Id),
            Name = integration.Name,
            Slug = integration.Slug
        };

    public static IEnumerable<Integration> ToDto(
        this IEnumerable<IntegrationEntity> integrations,
        SqidsEncoder<long> sqids
    ) => integrations.Select(i => i.ToDto(sqids));

    public static Tag ToDto(this TagEntity tag, SqidsEncoder<long> sqids) =>
        new()
        {
            Id = sqids.Encode(tag.Id),
            Name = tag.Name ?? tag.Slug, // Fallback if the tag was created automatically
            Slug = tag.Slug
        };

    public static IEnumerable<Tag> ToDto(this IEnumerable<TagEntity> tags, SqidsEncoder<long> sqids) =>
        tags.Select(t => t.ToDto(sqids));

    public static Location ToDto(this LocationEntity location, SqidsEncoder<long> sqids) =>
        new()
        {
            Id = sqids.Encode(location.Id),
            Name = location.Name,
            Slug = location.Slug,
            Type = location.Type
        };

    public static IEnumerable<Location> ToDto(this IEnumerable<LocationEntity> locations, SqidsEncoder<long> sqids) =>
        locations.Select(l => l.ToDto(sqids));
}
