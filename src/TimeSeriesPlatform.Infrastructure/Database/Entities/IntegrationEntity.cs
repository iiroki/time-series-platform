using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Database.Entities;

/// <summary>
/// Integration schema.
/// </summary>
[Index(nameof(Slug), IsUnique = true)]
public class IntegrationEntity : IdentifierEntityBase
{
    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;

    // TODO: Integration capabilities?

    public DateTime VersionTimestamp { get; set; }

    // public override string ToString() => this.Stringify();
}
