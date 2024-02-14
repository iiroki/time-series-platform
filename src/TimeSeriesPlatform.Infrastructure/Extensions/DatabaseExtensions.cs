using Iiroki.TimeSeriesPlatform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    /// <summary>
    /// Adds Time Series Platform database to services:<br/>
    /// - EF Core DB context<br/>
    /// - Npgsql data source.
    /// </summary>
    public static void AddTspDatabase(this IServiceCollection services, IConfiguration config)
    {
        var source = TspDbContext.CreateSource(config);
        services.AddSingleton(source);
        services.AddDbContext<TspDbContext>(opt => opt.UseNpgsql(source));
    }
}
