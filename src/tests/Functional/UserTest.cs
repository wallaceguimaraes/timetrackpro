using System.Net;
using api.Extensions;
using api.Extensions.Http;
using api.Models.EntityModel.Users;
using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Employees;
using api.Models.ViewModel.Users;
using api.Results.Errors;
using Microsoft.EntityFrameworkCore;
using test.Fakes;
using tests.Factories.Models.EntityModels;
using tests.Factories.Models.ViewModels;
using Xunit;

namespace tests.Functional
{
    public class UserTest
    {
        private FakeServer _fakeServer;
        private HttpClient _client;
        private const string _basePath = "api/v1/users";

        public UserTest()
        {
            _fakeServer = new FakeServer();
            _client = _fakeServer.CreateAuthenticatedClient();
        }

        [Fact]
        public async Task ShouldFindUser()
        {
            var user = new User().Build();
            _fakeServer.DbContext.Users.Add(user);
            _fakeServer.DbContext.SaveChanges();

            var response = await _client.GetAsync($"{_basePath}/2");
            var json = await response.Content.ReadAsJsonAsync<UserJson>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(json.User);
            Assert.Equal(user.Email, json.User.Email);
            Assert.Equal(user.Email, json.User.Email);
            Assert.Equal(user.Login, json.User.Login);
            Assert.Equal(user.Name, json.User.Name);
            Assert.Equal(user.Password, json.User.Password);
            Assert.Equal(user.Salt, json.User.Salt);
            Assert.Equal(user.Id, json.User.Id);
            Assert.Equal(user.CreatedAt, json.User.CreatedAt);
        }

        [Theory]
        [InlineData("r")]
        [InlineData("*")]
        public async Task ShouldNotFindUserWhenIdIsInvalid(string id)
        {
            var user = new User().Build();
            _fakeServer.DbContext.Users.Add(user);
            _fakeServer.DbContext.SaveChanges();

            var response = await _client.GetAsync($"{_basePath}/{id}");
            var json = await response.Content.ReadAsJsonAsync<BadRequestJson>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Id: Invalid.", json.Message);
        }

        [Fact]
        public async Task ShouldNotFindUserWhenUserNotExists()
        {
            var response = await _client.GetAsync($"{_basePath}/2");
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("USER_NOT_FOUND", json.Message);
        }

        [Fact]
        public async Task ShouldNotFindUserWhenUserHasNotAuthorization()
        {
            var response = await _fakeServer.CreateUnauthenticatedClient().GetAsync($"{_basePath}/2");
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldNotCreateUserWhenHasNotAuthorization()
        {
            var response = await _fakeServer.CreateUnauthenticatedClient().PostJsonAsync($"{_basePath}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldCreateUser()
        {
            var model = new UserModel().Build();

            var response = await _client.PostJsonAsync($"{_basePath}", model);
            var json = await response.Content.ReadAsJsonAsync<UserJson>();

            var user = _fakeServer.DbContext.Users.WhereId(json.User.Id).SingleOrDefault();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(json.User);
            Assert.Equal(model.Email, json.User.Email);
            Assert.Equal(model.Email, json.User.Email);
            Assert.Equal(model.Login, json.User.Login);
            Assert.Equal(model.Name, json.User.Name);
            Assert.Equal(model.Email, user.Email);
            Assert.Equal(model.Login, user.Login);
            Assert.Equal(model.Name, user.Name);
        }

        [Fact]
        public async Task ShouldNotCreateUserWhenEmailExists()
        {
            var model = new UserModel().Build(email: "teste1@mail.com");

            var response = await _client.PostJsonAsync($"{_basePath}", model);
            var json = await response.Content.ReadAsJsonAsync<UnprocessableEntityJson>();

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("EMAIL_ALREADY_EXISTS", json.Message);
        }

        [Fact]
        public async Task ShouldNotCreateUserWhenHasNotFieldsRequired()
        {
            var user = new User().Build();
            _fakeServer.DbContext.Users.Add(user);
            _fakeServer.DbContext.SaveChanges();

            var model = new UserModel();

            var response = await _client.PostJsonAsync($"{_basePath}", model);
            var json = await response.Content.ReadAsJsonAsync<BadRequestJson>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Login: Required.", json.Message);
            Assert.Contains("Email: Required.", json.Message);
            Assert.Contains("Name: Required.", json.Message);
            Assert.Contains("Password: Required.", json.Message);
        }

        [Fact]
        public async Task ShouldNotUpdateUserWhenHasNotAuthorization()
        {
            var model = new UserModel();

            var response = await _fakeServer.CreateUnauthenticatedClient().PutJsonAsync($"{_basePath}/2", model);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldUpdateUser()
        {
            var user = new User().Build();
            _fakeServer.DbContext.Users.Add(user);
            _fakeServer.DbContext.SaveChanges();

            var userOld = _fakeServer.DbContext.Users.WhereId(user.Id).AsNoTracking().SingleOrDefault();

            var model = new UserModel
            {
                Name = user.Name,
                Login = "novoatualizado",
                Email = "novoatualizado@mail.com",
                Password = "765432"
            };

            var response = await _client.PutJsonAsync($"{_basePath}/{user.Id}", model);
            var json = await response.Content.ReadAsJsonAsync<UserJson>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEqual(userOld.Login, json.User.Login);
            Assert.NotEqual(userOld.Email, json.User.Email);
            Assert.NotEqual(userOld.Password.Encrypt(userOld.Salt), json.User.Password);
            Assert.Null(userOld.LastUpdateAt);
            Assert.Equal(userOld.Name, json.User.Name);
            Assert.NotNull(json.User.LastUpdateAt);
        }

        [Fact]
        public async Task ShouldNotUpdateUserWhenEmailExists()
        {
            var user = new User().Build();
            _fakeServer.DbContext.Users.Add(user);
            _fakeServer.DbContext.SaveChanges();

            var userOld = _fakeServer.DbContext.Users.WhereId(user.Id).AsNoTracking().SingleOrDefault();

            var model = new UserModel
            {
                Name = user.Name,
                Login = "novoatualizado",
                Email = "teste1@mail.com",
                Password = "765432"
            };

            var response = await _client.PutJsonAsync($"{_basePath}/{user.Id}", model);
            var json = await response.Content.ReadAsJsonAsync<UnprocessableEntityJson>();

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("EMAIL_ALREADY_EXISTS", json.Message);
        }

        [Fact]
        public async Task ShouldNotUpdateUserWhenHasNotFieldsRequired()
        {

            var user = new User().Build();
            _fakeServer.DbContext.Users.Add(user);
            _fakeServer.DbContext.SaveChanges();

            var userOld = _fakeServer.DbContext.Users.WhereId(user.Id).AsNoTracking().SingleOrDefault();

            var model = new UserModel
            {
                Name = user.Name,
                Password = "765432"
            };

            var response = await _client.PutJsonAsync($"{_basePath}/{user.Id}", model);
            var json = await response.Content.ReadAsJsonAsync<BadRequestJson>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Login: Required.", json.Message);
            Assert.Contains("Email: Required.", json.Message);
        }

        [Fact]
        public async Task ShouldNotUpdateUserWhenUserNotExists()
        {
            var userAuthenticated = new User().Build();
            _fakeServer.DbContext.Users.Add(userAuthenticated);
            _fakeServer.DbContext.SaveChanges();

            var model = new UserModel
            {
                Name = "teste",
                Login = "novoatualizado",
                Email = "novoatualizado@mail.com",
                Password = "765432"
            };

            var response = await _client.PutJsonAsync($"{_basePath}/5", model);
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("USER_NOT_FOUND", json.Message);
        }

    }
}