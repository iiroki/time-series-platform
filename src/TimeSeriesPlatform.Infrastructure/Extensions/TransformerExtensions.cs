using Iiroki.TimeSeriesPlatform.Domain.Models;
using Iiroki.TimeSeriesPlatform.Infrastructure.Database.Entities;
using Sqids;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Extensions;

internal static class TransformerExtensions
{
    public static Integration ToDomain(this IntegrationEntity integration, SqidsEncoder<long> sqids) =>
        new()
        {
            Id = sqids.Encode(integration.Id),
            Name = integration.Name,
            Slug = integration.Slug
        };

    public static IEnumerable<Integration> ToDomain(
        this IEnumerable<IntegrationEntity> integrations,
        SqidsEncoder<long> sqids
    ) => integrations.Select(i => i.ToDomain(sqids));

    public static Tag ToDomain(this TagEntity tag, SqidsEncoder<long> sqids) =>
        new()
        {
            Id = sqids.Encode(tag.Id),
            Name = tag.Name ?? tag.Slug, // Fallback if the tag was created automatically
            Slug = tag.Slug
        };

    public static IEnumerable<Tag> ToDomain(this IEnumerable<TagEntity> tags, SqidsEncoder<long> sqids) =>
        tags.Select(t => t.ToDomain(sqids));

    public static Location ToDomain(this LocationEntity location, SqidsEncoder<long> sqids) =>
        new()
        {
            Id = sqids.Encode(location.Id),
            Name = location.Name,
            Slug = location.Slug,
            Type = location.Type
        };

    public static IEnumerable<Location> ToDomain(
        this IEnumerable<LocationEntity> locations,
        SqidsEncoder<long> sqids
    ) => locations.Select(l => l.ToDomain(sqids));
}
