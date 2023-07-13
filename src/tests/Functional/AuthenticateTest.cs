using System.Net;
using api.Extensions.Http;
using api.Models.EntityModel.Users;
using api.Models.ViewModel;
using test.Fakes;
using tests.Factories.Models.ViewModels;
using tests.Factories.Models.EntityModels;

using Xunit;
using api.ResultModel.Successes.Authentication;
using api.Results.Errors;

namespace tests.Functional
{
    public class AuthenticateTest
    {
        private FakeServer _fakeServer;
        private HttpClient _client;
        private const string _basePath = "api/v1/authenticate";

        public AuthenticateTest()
        {
            _fakeServer = new FakeServer();
            _client = _fakeServer.CreateUnauthenticatedClient();
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task ShouldNotAuthenticateUserWhenHasNotRequiredFields(bool hasLogin, bool hasPassword)
        {
            var model = new object();

            if (hasLogin)
                model = new { Login = "teste" };

            if (hasPassword)
                model = new { Password = "teste" };

            var response = await _client.PostJsonAsync($"{_basePath}", model);
            var json = await response.Content.ReadAsJsonAsync<BadRequestJson>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            if (hasLogin)
                Assert.Contains("Password: Required.", json.Message);

            if (hasPassword)
                Assert.Contains("Login: Required.", json.Message);

            if (!hasLogin && !hasPassword)
            {
                Assert.Contains("Login: Required.", json.Message);
                Assert.Contains("Password: Required.", json.Message);
            }
        }

        [Fact]
        public async Task ShouldNotAuthenticateUserWhenUserNotExists()
        {
            var model = new CredentialModel().Build(login: "test");
            var response = await _client.PostJsonAsync($"{_basePath}", model);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldAuthenticateUser()
        {
            var user = new User().Build();

            _fakeServer.DbContext.Users.Add(user);
            _fakeServer.DbContext.SaveChanges();

            var model = new CredentialModel().Build(login: user.Login);
            var response = await _client.PostJsonAsync($"{_basePath}", model);
            var json = await response.Content.ReadAsJsonAsync<TokenJson>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(json.Token);
            Assert.NotNull(json.User);
            Assert.Equal(user.Login, json.User.Login);
            Assert.Equal(user.Salt, json.User.Salt);
        }
    }
}