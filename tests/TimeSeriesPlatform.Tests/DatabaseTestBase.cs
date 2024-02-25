using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Iiroki.TimeSeriesPlatform.Infrastructure;
using Iiroki.TimeSeriesPlatform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Iiroki.TimeSeriesPlatform.Tests;

[Category("Database")]
public class DatabaseTestBase
{
    private const int DbPort = 5678;
    private const string DbName = "_test_db";
    private const string DbPassword = "Passw0rd!";

    private static readonly IContainer DbContainer = new ContainerBuilder()
        .WithImage("timescale/timescaledb:latest-pg16")
        .WithEnvironment("POSTGRES_DB", DbName)
        .WithEnvironment("POSTGRES_PASSWORD", DbPassword)
        .WithPortBinding(DbPort, 5432)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
        .Build();

    protected static readonly string DbConnection = string.Join(
        ';',
        $"Host=localhost:{DbPort}",
        $"Database={DbName}",
        "Username=postgres",
        $"Password={DbPassword}",
        "Pooling=false"
    );

    private IConfiguration? _config;
    private DbContextOptions<TspDbContext>? _dbOptions;

    [OneTimeSetUp]
    public async Task SetupDbContainerAsync()
    {
        Environment.SetEnvironmentVariable(Config.DatabaseUrl, DbConnection);
        _config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        _dbOptions = TspDbContext.CreateOptions(_config);

        if (DbContainer.State == TestcontainersStates.Undefined)
        {
            await DbContainer.StartAsync();
        }
    }

    // [OneTimeTearDown]
    // public async Task StopDbContainerAsync() => await DbContainer.StopAsync();

    [SetUp]
    public async Task InitDbAsync()
    {
        await using var dbContext = CreateDbContext();
        await dbContext.Database.ExecuteSqlRawAsync($"DROP SCHEMA IF EXISTS {TspDbContext.Schema} CASCADE;");
        await dbContext.Database.EnsureCreatedAsync();
    }

    internal TspDbContext CreateDbContext() => new(_dbOptions!);

    internal NpgsqlDataSource CreateDbSource() => TspDbContext.CreateSource(_config!);
}
