using System.ComponentModel.DataAnnotations.Schema;
using Iiroki.TimeSeriesPlatform.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Database.Entities;

/// <summary>
/// Measurement schema (hypertable).
/// </summary>
[Keyless]
// Unique index is defined in "TspDbContext"!
public class MeasurementEntity
{
    public long IntegrationId { get; set; }

    public long TagId { get; set; }

    /// <summary>
    /// The measurement might be bound to a location, but does not have to.
    /// </summary>
    public long? LocationId { get; set; }

    public DateTime Timestamp { get; set; }

    public double Value { get; set; }

    public DateTime VersionTimestamp { get; set; }

    // Navigations:

    [ForeignKey(nameof(TagId))]
    public TagEntity Tag { get; set; } = default!;

    [ForeignKey(nameof(IntegrationId))]
    public IntegrationEntity Integration { get; set; } = default!;

    [ForeignKey(nameof(LocationId))]
    public LocationEntity? Location { get; set; }

    public override string ToString() => this.Stringify();
}
