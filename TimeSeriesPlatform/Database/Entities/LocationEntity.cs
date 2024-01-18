using System.ComponentModel.DataAnnotations.Schema;
using Iiroki.TimeSeriesPlatform.Extensions;
using Iiroki.TimeSeriesPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Database.Entities;

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

    public override string ToString() => this.Stringify();
}
