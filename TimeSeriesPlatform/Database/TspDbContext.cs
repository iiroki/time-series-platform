using Iiroki.TimeSeriesPlatform.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Database;

/// <summary>
/// Time Series Platform database context.
/// </summary>
public class TspDbContext : DbContext
{
    public const string Schema = "tsp";

    public DbSet<IntegrationEntity> Integration { get; set; } = default!;
    public DbSet<TagEntity> Tag { get; set; } = default!;
    public DbSet<MeasurementEntity> Measurement { get; set; } = default!;

    public TspDbContext(DbContextOptions<TspDbContext> opt)
        : base(opt)
    {
        // NOP
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(Schema);
    }

    public static string ToTableName(string name) => $"{Schema}.\"{name}\"";
}
