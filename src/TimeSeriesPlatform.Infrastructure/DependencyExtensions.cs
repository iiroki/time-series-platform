using Iiroki.TimeSeriesPlatform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace Iiroki.TimeSeriesPlatform.Infrastructure;

public static class DependencyExtensions
{
    private const string SqidsAlphabets = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";

    /// <summary>
    /// Adds Time Series Platform database to services:<br/>
    /// - EF Core DB context<br/>
    /// - Npgsql data source.
    /// </summary>
    public static IServiceCollection AddTspDatabase(this IServiceCollection services, IConfiguration config)
    {
        var source = TspDbContext.CreateSource(config);
        return services
            .AddSingleton(source)
            .AddDbContext<TspDbContext>(opt => opt.UseNpgsql(source));
    }

    /// <summary>
    /// Adds Sqids encoder to services.
    /// </summary>
    public static IServiceCollection AddSqids(this IServiceCollection services) =>
        services.AddSingleton(new SqidsEncoder<long>(new() { Alphabet = SqidsAlphabets, MinLength = 6 }));
}
