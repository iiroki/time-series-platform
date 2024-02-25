using Iiroki.TimeSeriesPlatform.Domain.Services;
using Iiroki.TimeSeriesPlatform.Infrastructure.Database;
using Iiroki.TimeSeriesPlatform.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace Iiroki.TimeSeriesPlatform.Infrastructure;

/// <summary>
/// Extensions to inject the domain service implementations to the application services
/// </summary>
public static class DependencyExtensions
{
    private const string SqidsAlphabets = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";

    /// <summary>
    /// Adds Time Series Platform database to the services:<br/>
    /// - EF Core DB context<br/>
    /// - Npgsql data source.
    /// </summary>
    public static IServiceCollection AddTspDatabase(this IServiceCollection services, IConfiguration config)
    {
        var source = TspDbContext.CreateSource(config);
        return services.AddSingleton(source).AddDbContext<TspDbContext>(opt => opt.UseNpgsql(source));
    }

    /// <summary>
    /// Adds Sqids encoder to the services.
    /// </summary>
    public static IServiceCollection AddSqids(this IServiceCollection services) =>
        services.AddSingleton(new SqidsEncoder<long>(new() { Alphabet = SqidsAlphabets, MinLength = 6 }));

    /// <summary>
    /// Adds metadata service to the services.
    /// </summary>
    public static IServiceCollection AddMetadataService(this IServiceCollection services) =>
        services.AddScoped<IMetadataService, MetadataService>();

    /// <summary>
    /// Adds measurement service to the services.
    /// </summary>
    public static IServiceCollection AddMeasurementService(this IServiceCollection services) =>
        services.AddScoped<IMeasurementService, MeasurementService>();
}
