using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Iiroki.TimeSeriesPlatform.Database;
using Microsoft.Extensions.Configuration;

namespace Iiroki.TimeSeriesPlatform.Tests.Servces;

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
        $"Password={DbPassword}"
    );

    [OneTimeSetUp]
    public async Task SetupDbContainerAsync()
    {
        Environment.SetEnvironmentVariable(Config.DatabaseUrl, DbConnection);
        if (DbContainer.State == TestcontainersStates.Undefined)
        {
            await DbContainer.StartAsync();
        }
    }

    [OneTimeTearDown]
    public async Task DestroyDbContainerAsync() => await DbContainer.DisposeAsync();

    protected static TspDbContext CreateDbContext()
    {
        var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

        return TspDbContext.Create(config);
    }

    [SetUp]
    public async Task InitDbAsync()
    {
        var dbContext = CreateDbContext();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }
}
