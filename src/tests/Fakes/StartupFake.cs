using api;
using api.Authorization;
using api.Data.Context;
using api.Filters;
using api.Infrastructure.Mvc;
using api.Models.Interfaces;
using api.Models.ServiceModel;
using api.Models.ServiceModel.Projects;
using api.Models.ServiceModel.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using test.Fakes;

namespace tests.Fakes
{
    public class StartupFake
    {
        public IWebHostEnvironment Environment { get; }
        public IConfigurationRoot Configuration { get; }
        private SqliteConnection connection;

        public StartupFake()
        {
            Environment = new HostEnvironmentFake();

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.ContentRootPath)
                .AddEnvironmentVariables()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DbContextFake>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddDbContext<DbContextFake>(options =>
            {
                options.UseSqlite(connection)
                .EnableSensitiveDataLogging();
                // .LogTo(Console.WriteLine, LogLevel.Debug);
                options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));

            }, ServiceLifetime.Singleton);

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DbContextFake>();
                dbContext.Database.EnsureCreated();
            }

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(ErrorHandlerAttribute));
                options.Filters.Add(typeof(ModelValidationAttribute));
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new CustomEnumConverter());
            })
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
            .AddApplicationPart(typeof(Startup).Assembly);


            services.Configure<AuthOptions>(options =>
            {
                options.Issuer = "6144F75E2";
                options.Audience = "B706E107AFE8CB7";
                options.Key = "50D2C5353238D86B047079FCD2F79DA79F55CE79ABFC333EAA2E9D3072211BBD";
                options.Secret = "9180F9D5DD38A06CE42";
                options.ExpireTokenIn = 3;
            });

            services.AddSingleton(Environment);
            services.AddSingleton<ApiDbContext, DbContextFake>();
            services.AddSingleton<IUserAuthentication, UserAuthentication>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IProjectService, ProjectService>();
        }


        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseRouting();
            app.UseEndpoints(opt => opt.MapControllers());
            app.UseAuthentication();
        }
    }
}
