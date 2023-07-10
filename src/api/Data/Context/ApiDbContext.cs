// using api.Models.EntityModel.Addresses;
// using api.Models.EntityModel.BusinessTypes;
// using api.Models.EntityModel.Cnaes;
// using api.Models.EntityModel.Companies;
// using api.Models.EntityModel.JuristicPersons;
// using api.Models.EntityModel.NaturalPersons;
// using api.Models.EntityModel.Persons;
// using api.Models.EntityModel.Roles;
using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Times;
using api.Models.EntityModel.UserProjects;
using api.Models.EntityModel.Users;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Context
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Map();
            modelBuilder.Entity<Project>().Map();
            modelBuilder.Entity<Time>().Map();
            modelBuilder.Entity<UserProject>().Map();
        }
    }
}