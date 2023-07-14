using System.Text;
using api.Authorization;
using api.Models.Interfaces;
using api.Models.ServiceModel;
using api.Models.ServiceModel.Projects;
using api.Models.ServiceModel.Times;
using api.Models.ServiceModel.Users;
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sua-chave-secreta"))
                };
            });
        }

        public static void AddTransientServices(this IServiceCollection services)
        {
            services.AddTransient<IUserAuthentication, UserAuthentication>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ITimeService, TimeService>();

        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions();
            services.Configure<AuthOptions>(config.GetSection("Authorization"));

        }
    }
}