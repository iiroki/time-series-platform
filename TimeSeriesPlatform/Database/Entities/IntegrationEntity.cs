using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Database.Entities;

/// <summary>
/// Integration table schema.
/// </summary>
[PrimaryKey(nameof(Id))]
[Index(nameof(Slug), IsUnique = true)]
public class IntegrationEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;
}
