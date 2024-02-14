using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Database.Design;

public class OperationalDbDesignFactory : IDesignTimeDbContextFactory<TspDbContext>
{
    public TspDbContext CreateDbContext() => CreateDbContext(new string[] { });

    public TspDbContext CreateDbContext(string[] _)
    {
        var opt = new DbContextOptionsBuilder<TspDbContext>();

        // For database migrations, read the connection string here!
        var connectionString = Environment.GetEnvironmentVariable(Config.DatabaseUrl);
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            opt.UseNpgsql(connectionString);
        }
        else
        {
            opt.UseNpgsql();
        }

        return new TspDbContext(opt.Options);
    }
}
