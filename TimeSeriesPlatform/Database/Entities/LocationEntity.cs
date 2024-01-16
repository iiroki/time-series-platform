using System.ComponentModel.DataAnnotations.Schema;
using Iiroki.TimeSeriesPlatform.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Database.Entities;

/// <summary>
/// Location schema.
/// </summary>
[PrimaryKey(nameof(Id))]
[Index(nameof(Slug), IsUnique = true)]
public class LocationEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;

    public LocationType? Type { get; set; }

    // TODO: Coordinates?

    public override string ToString() => this.Stringify();
}

public enum LocationType
{
    Physical,
    Virtual
}
