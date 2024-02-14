using Microsoft.EntityFrameworkCore;

namespace Iiroki.TimeSeriesPlatform.Infrastructure.Extensions;

public static class EntityExtensions
{
    public static async Task<bool> SaveDeleteAsync(this DbContext dbContext, CancellationToken ct)
    {
        try
        {
            await dbContext.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            // The query did not affect any rows -> Nothing was deleted
            return false;
        }
    }
}
