using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Database.Entities;

/// <summary>
/// Tag schema.
/// </summary>
[Index(nameof(Slug), IsUnique = true)]
internal class TagEntity : IdentifierEntityBase
{
    public string Slug { get; set; } = default!;

    public string? Name { get; set; }

    public DateTime VersionTimestamp { get; set; }

    // public override string ToString() => this.Stringify();
}
