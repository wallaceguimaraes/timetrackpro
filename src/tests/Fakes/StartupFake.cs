using api;
using api.Authorization;
using api.Data.Context;
using api.Filters;
using api.Models.Interfaces;
using api.Models.ServiceModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using test.Fakes;

namespace tests.Fakes
{
    public class StartupFake
    {
        public IWebHostEnvironment Environment { get; }
        public IConfigurationRoot Configuration { get; }
        // private SqliteConnection connection;


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
            services.AddRouting();


            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(ErrorHandlerAttribute));
                options.Filters.Add(typeof(ModelValidationAttribute));
            })
            .AddApplicationPart(typeof(Startup).Assembly);


            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DbContextFake>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            services.Configure<AuthOptions>(options =>
            {
                options.Issuer = "6144F75E2";
                options.Audience = "B706E107AFE8CB7";
                options.Key = "50D2C5353238D86B047079FCD2F79DA79F55CE79ABFC333EAA2E9D3072211BBD";
                options.Secret = "9180F9D5DD38A06CE42";
                options.ExpireTokenIn = 3;
            });

            services.AddSingleton(Environment);
            // services.AddSingleton<UserAuthentication>();
            // services.AddSingleton<IUserAuthentication, UserAuthenticationFake>();
            services.AddSingleton<DbContextFake>();
            services.AddSingleton<IUserAuthentication, UserAuthentication>();


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
