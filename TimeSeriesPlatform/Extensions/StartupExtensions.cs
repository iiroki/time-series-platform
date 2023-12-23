using Iiroki.TimeSeriesPlatform.Database;
using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Extensions;

public static class StartupExtensions
{
    public static void AddTspDbContext(this IServiceCollection services)
    {
        var connection = "";
        services.AddDbContext<TspDbContext>(opt => opt.UseNpgsql(connection));
    }
}
