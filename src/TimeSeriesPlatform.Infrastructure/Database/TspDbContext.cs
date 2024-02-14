using Iiroki.TimeSeriesPlatform.Infrastructure.Extensions;
using Iiroki.TimeSeriesPlatform.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Iiroki.TimeSeriesPlatform.Utilities.Extensions;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Database;

/// <summary>
/// Time Series Platform database context.
/// </summary>
public class TspDbContext(DbContextOptions<TspDbContext> opt) : DbContext(opt)
{
    public const string Schema = "tsp";

    public DbSet<IntegrationEntity> Integration { get; set; } = default!;
    public DbSet<TagEntity> Tag { get; set; } = default!;
    public DbSet<LocationEntity> Location { get; set; } = default!;

    public DbSet<MeasurementEntity> Measurement { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(Schema);

        builder.Entity<LocationEntity>().Property(l => l.Type).HasConversion<short>();

        builder
            .Entity<MeasurementEntity>()
            .HasIndex(
                [
                    nameof(MeasurementEntity.IntegrationId),
                    nameof(MeasurementEntity.TagId),
                    nameof(MeasurementEntity.LocationId),
                    nameof(MeasurementEntity.Timestamp)
                ]
            )
            .IsUnique()
            .AreNullsDistinct(false); // Location is nullable -> Treat null locations as equals
    }

    public static string ToTableName(string name) => $"{Schema}.\"{name}\"";

    public static NpgsqlDataSource CreateSource(IConfiguration config)
    {
        var connection = config.GetRequired(Config.DatabaseUrl);
        return new NpgsqlDataSourceBuilder(connection).Build();
    }

    public static DbContextOptions<TspDbContext> CreateOptions(IConfiguration config)
    {
        var builder = new DbContextOptionsBuilder<TspDbContext>();
        builder.UseNpgsql(CreateSource(config));
        return builder.Options;
    }

    public static TspDbContext Create(IConfiguration config) => new(CreateOptions(config));
}
