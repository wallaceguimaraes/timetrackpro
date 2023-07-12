using System.Net.Http.Headers;
using api.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using tests.Fakes;

namespace test.Fakes
{
    public class FakeServer : TestServer
    {
        public ApiDbContext DbContext => Host.Services.GetRequiredService<ApiDbContext>();
        // public UserAuthenticationFake? UserAuthentication => Host.Services.GetRequiredService<IUserAuthentication>() as UserAuthenticationFake;

        public FakeServer() : base(new WebHostBuilder().UseStartup<StartupFake>().UseEnvironment("Testing")) { }
        public HttpClient CreateAuthenticatedClient()
        {
            var client = base.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "USER-TOKEN");

            return client;
        }
    }
}