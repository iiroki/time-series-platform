using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Database.Entities;

/// <summary>
/// Measurement schema (hypertable).
/// </summary>
[Keyless]
[Index(nameof(IntegrationId), nameof(TagId), nameof(Timestamp), IsUnique = true)]
public class MeasurementEntity
{
    public long TagId { get; set; }

    public long IntegrationId { get; set; }

    public double Value { get; set; }

    public DateTime Timestamp { get; set; }

    // Navigations:

    [ForeignKey(nameof(TagId))]
    public TagEntity Tag { get; set; } = default!;

    [ForeignKey(nameof(IntegrationId))]
    public IntegrationEntity Integration { get; set; } = default!;
}
