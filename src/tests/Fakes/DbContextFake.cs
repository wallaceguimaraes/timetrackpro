using api.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace test.Fakes
{
    public class DbContextFake : ApiDbContext
    {
        public DbContextFake(DbContextOptions options) : base(options)
        {
            Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF;");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

        }

    }
}