using api.Data.Context;
using api.Extensions.DependencyInjection;
using api.Filters;
using api.Infrastructure.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
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
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

            services.AddJwtAuthentication(options =>
            {
                Configuration.GetSection("Authorization").Bind(options);
            });

            services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:ConnectionString"], mssql =>
                {
                    mssql.MigrationsHistoryTable(tableName: "__MigrationHistory", schema: "cadastro");
                });
            });

            services.AddTransientServices();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(policy => policy
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.UseHttpsRedirection();
            app.UseAuthentication();
        }
    }
}
