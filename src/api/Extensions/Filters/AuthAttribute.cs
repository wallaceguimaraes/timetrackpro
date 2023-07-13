using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using api.Authorization;
using System.IdentityModel.Tokens.Jwt;
using api.Extensions.Http;
using api.Models.EntityModel;
using api.Models.Interfaces;

namespace api.Filters
{
    public class AuthAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private const string Service = "TimeTrack";

        public AuthAttribute()
        {

        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            var token = context.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (String.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                context.HttpContext.Response.Clear();
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                context.Result = new UnauthorizedResult();
                context.HttpContext.Response.Clear();
                return;
            }

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ApiClaimTypes.UserId);
            var saltClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ApiClaimTypes.Salt);

            if (userIdClaim != null && saltClaim != null)
            {
                var userId = userIdClaim.Value;
                var salt = saltClaim.Value;

                var user = await service.FindUserAuthenticated(userId);

                if (user != null && salt == user.Salt)
                {
                    var whoAmI = new WhoAmI
                    {
                        User = user,
                        AccessGranted = true,
                    };

                    context.HttpContext.SetWhoAmI(whoAmI);
                    return;
                }
            }

            context.Result = new UnauthorizedResult();
            context.HttpContext.Response.Clear();
        }

        private bool SecretValid(AuthOptions options, string secretReceived)
        {
            if (!string.IsNullOrEmpty(secretReceived) && (options?.Secret == secretReceived))
                return true;

            return false;
        }
    }
}