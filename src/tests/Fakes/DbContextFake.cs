
using api.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace test.Fakes
{
    public class DbContextFake : ApiDbContext
    {
        public DbContextFake(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

        }
    }
}