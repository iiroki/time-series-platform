using System.ComponentModel.DataAnnotations.Schema;
using Iiroki.TimeSeriesPlatform.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Database.Entities;

/// <summary>
/// Tag schema.
/// </summary>
[PrimaryKey(nameof(Id))]
[Index(nameof(Slug), IsUnique = true)]
public class TagEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Slug { get; set; } = default!;

    public string? Name { get; set; }

    public DateTime VersionTimestamp { get; set; }

    public override string ToString() => this.Stringify();
}
