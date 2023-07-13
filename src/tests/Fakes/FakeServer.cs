using System.Net.Http.Headers;
using api.Data.Context;
using api.Models.EntityModel.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using tests.Factories.Models.EntityModels;
using tests.Fakes;

namespace test.Fakes
{
    public class FakeServer : TestServer
    {
        public ApiDbContext DbContext => Host.Services.GetRequiredService<DbContextFake>();

        public FakeServer() : base(new WebHostBuilder().UseStartup<StartupFake>().UseEnvironment("Testing")) { }
        public HttpClient CreateAuthenticatedClient()
        {
            var client = base.CreateClient();

            var user = new User().Build(email: "teste1@mail.com");
            DbContext.Users.Add(user);
            DbContext.SaveChanges();

            var tokenFake = new TokenFake();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{tokenFake.Token}");

            return client;
        }

        public HttpClient CreateUnauthenticatedClient()
        {
            var client = base.CreateClient();

            return client;
        }
    }
}