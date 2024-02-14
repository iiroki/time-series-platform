using Iiroki.TimeSeriesPlatform.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Database.Entities;

/// <summary>
/// Location schema.
/// </summary>
[Index(nameof(Slug), IsUnique = true)]
public class LocationEntity : IdentifierEntityBase
{
    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;

    public LocationType? Type { get; set; }

    // TODO: Coordinates if a physical location?

    public DateTime VersionTimestamp { get; set; }

    // public override string ToString() => this.Stringify();
}
