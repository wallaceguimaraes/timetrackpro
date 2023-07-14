using api.Models.EntityModel;
using Microsoft.Net.Http.Headers;

namespace api.Extensions.Http
{
    public static class HttpContextExtensions
    {
        private const string WhoAmIItem = "WhoAmI";
        private const int CharacterLimitForIP = 150;

        public static string Token(this HttpContext httpContext)
        {
            return httpContext.Request.Headers[HeaderNames.Authorization];
        }

        public static WhoAmI? WhoAmI(this HttpContext httpContext)
        {
            return httpContext.Items[WhoAmIItem] as WhoAmI;
        }

        public static void SetWhoAmI(this HttpContext httpContext, WhoAmI whoAmI)
        {
            httpContext.Items[WhoAmIItem] = whoAmI;
        }
    }
}