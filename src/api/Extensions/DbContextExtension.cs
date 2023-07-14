using Microsoft.EntityFrameworkCore;

namespace api.Extensions
{
    public static class DbContextExtension
    {
        public static async Task ReloadAllEntries(this DbContext context)
        {
            var tasks = context.ChangeTracker
                .Entries().ToList().Select(e => e.ReloadAsync())
                .ToList();

            await Task.WhenAll(tasks);
        }
    }
}
