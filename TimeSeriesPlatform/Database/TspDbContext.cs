using Iiroki.TimeSeriesPlatform.Database.Entities;
using Iiroki.TimeSeriesPlatform.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
