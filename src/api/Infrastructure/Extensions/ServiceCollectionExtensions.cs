// using api.Authorization;
using System.Text;
using api.Authorization;
using api.Models.ServiceModel;
// using api.Models.ServiceModel.Companies;
// using api.Models.ServiceModel.Employees;
// using api.Models.ServiceModel.Roles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace api.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, Action<AuthOptions> configure)
        {
            services.Configure<AuthOptions>(configure);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var authOptions = new AuthOptions();

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidAudience = authOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sua-chave-secreta")) // Defina a chave secreta para validar o token
                };
            });
        }

        public static void AddTransientServices(this IServiceCollection services)
        {
            services.AddTransient<UserAuthentication>();
            // services.AddTransient<EmployeeService>();
            // services.AddTransient<CompanyService>();
            // services.AddTransient<RoleService>();

        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions();
            services.Configure<AuthOptions>(config.GetSection("Authorization"));

        }
    }
}