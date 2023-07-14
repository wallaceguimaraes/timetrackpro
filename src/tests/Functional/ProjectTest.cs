using System.Net;
using api.Extensions.Http;
using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Users;
using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Projects;
using api.Models.ViewModel.Projects;
using api.Results.Errors;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using test.Fakes;
using tests.Factories.Models.EntityModels;
using tests.Factories.Models.ViewModels;
using Xunit;

namespace tests.Functional
{
    public class ProjectTest
    {
        private FakeServer _fakeServer;
        private HttpClient _client;
        private const string _basePath = "api/v1/projects";

        public ProjectTest()
        {
            _fakeServer = new FakeServer();
            _client = _fakeServer.CreateAuthenticatedClient();
        }

        [Fact]
        public async Task ShouldFindProject()
        {
            var userAuthenticated = _fakeServer.DbContext.Users.SingleOrDefault();
            var project = new Project().Build().WithUser(userAuthenticated);

            var otherUser = new User().Build(login: "usuario2", password: "yuihkjjhfh", name: "usuario novo", email: "usuario@mail.com");
            var project2 = new Project().Build().WithUser(otherUser);

            _fakeServer.DbContext.Projects.AddRange(project, project2);
            _fakeServer.DbContext.SaveChanges();

            var response = await _client.GetAsync($"{_basePath}/{project.Id}");
            var json = await response.Content.ReadAsJsonAsync<ProjectJson>();
            var userIds = json.Project.UserProjects.Select(x => x.UserId);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(json.Project);
            Assert.Equal(project.Title, json.Project.Title);
            Assert.Equal(project.Description, json.Project.Description);
            Assert.Contains(userIds, id => id == userAuthenticated.Id && id != otherUser.Id);
            Assert.Equal(project.Title, json.Project.Title);

        }

        [Fact]
        public async Task ShouldNotFindProjectWhenUserHasNotAuthorization()
        {
            var response = await _fakeServer.CreateUnauthenticatedClient().GetAsync($"{_basePath}/2");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("gh")]
        [InlineData("%")]
        public async Task ShouldNotFindProjectWhenIdIsInvalid(string id)
        {
            var project = new Project().Build();
            _fakeServer.DbContext.Projects.Add(project);
            _fakeServer.DbContext.SaveChanges();

            var response = await _client.GetAsync($"{_basePath}/{id}");
            var json = await response.Content.ReadAsJsonAsync<BadRequestJson>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Id: Invalid.", json.Message);
        }

        [Fact]
        public async Task ShouldNotFindProjectWhenProjectNotExists()
        {
            var response = await _client.GetAsync($"{_basePath}/2");
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("PROJECT_NOT_FOUND", json.Message);
        }

        [Fact]
        public async Task ShouldNotCreateProjectWhenHasNotAuthorization()
        {
            var userIds = new List<int> { 1 };
            var model = new ProjectModel().Build(userIds: userIds);
            var response = await _fakeServer.CreateUnauthenticatedClient().PostJsonAsync($"{_basePath}", model);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldCreateProject()
        {
            var userIds = new List<int> { 1 };

            var model = new ProjectModel().Build(userIds: userIds);
            var response = await _client.PostJsonAsync($"{_basePath}", model);
            var json = await response.Content.ReadAsJsonAsync<ProjectJson>();

            var project = _fakeServer.DbContext.Projects.WhereId(json.Project.Id).SingleOrDefault();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(json.Project);
            Assert.Equal(model.Title, json.Project.Title);
            Assert.Equal(model.Description, json.Project.Description);
            Assert.Equal(model.Title, project.Title);
            Assert.Equal(model.Description, project.Description);
        }

        [Theory]
        [InlineData("post")]
        [InlineData("put")]
        public async Task ShouldNotCreateOrUpdateWhenProjectExists(string verbHttp)
        {
            var userIds = new List<int> { 1 };

            var model = new ProjectModel().Build(userIds: userIds);

            var firstProject = new Project().Build(model.Title, model.Description);

            if (verbHttp.Equals("put"))
                firstProject.Title = "outro titulo";

            _fakeServer.DbContext.Projects.Add(firstProject);
            _fakeServer.DbContext.SaveChanges();

            var oldProjectCounter = _fakeServer.DbContext.Projects.AsNoTracking().ToList().Count;


            HttpResponseMessage response = new HttpResponseMessage();

            if (verbHttp.Equals("post"))
                response = await _client.PostJsonAsync($"{_basePath}", model);
            if (verbHttp.Equals("put"))
            {
                _fakeServer.DbContext.Projects.Add(new Project().Build(model.Title, "segundo projeto"));
                _fakeServer.DbContext.SaveChanges();

                var jsonModel = JsonConvert.SerializeObject(model);
                response = await _client.PutJsonAsync($"{_basePath}/1", jsonModel);
            }

            var json = await response.Content.ReadAsJsonAsync<UnprocessableEntityJson>();

            var updatedProjectCounter = _fakeServer.DbContext.Projects.ToList().Count;

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal("PROJECT_ALREADY_EXISTS", json.Message);
        }

        [Fact]
        public async Task ShouldNotCreateWhenUserNotFound()
        {
            var userIds = new List<int> { 1, 2 };

            var model = new ProjectModel().Build(userIds: userIds);

            var oldProjectCounter = _fakeServer.DbContext.Projects.AsNoTracking().ToList().Count;

            var response = await _client.PostJsonAsync($"{_basePath}", model);
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            var updatedProjectCounter = _fakeServer.DbContext.Projects.ToList().Count;

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("USER_NOT_FOUND", json.Message);
            Assert.Equal(oldProjectCounter, updatedProjectCounter);
        }

        [Fact]
        public async Task ShouldNotUpdateProjectWhenHasNotAuthorization()
        {
            var userIds = new List<int> { 1 };
            var model = new ProjectModel().Build(userIds: userIds);
            var response = await _fakeServer.CreateUnauthenticatedClient().PutJsonAsync($"{_basePath}/2", model);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldUpdateUser()
        {
            var userAuthenticated = _fakeServer.DbContext.Users.SingleOrDefault();
            var project = new Project().Build().WithUser(userAuthenticated);
            var newUser = new User().Build(login: "novousuario", password: "rBG1oDjTq9qBhW4EI7ouNdkBxI9C/IdF/FlU1+hn786=", name: "usuario dois", email: "usuario@mail.com");

            _fakeServer.DbContext.Projects.Add(project);
            _fakeServer.DbContext.Users.Add(newUser);
            _fakeServer.DbContext.SaveChanges();

            var oldProject = _fakeServer.DbContext.Projects.WhereId(project.Id)
                                                           .IncludeUserProject()
                                                           .AsNoTracking().SingleOrDefault();

            var userIds = oldProject.UserProjects.Where(x => x.User != null)
                                                 .Select(x => x.UserId)
                                                 .ToList();
            userIds.Add(newUser.Id);

            var model = new ProjectModel
            {
                Title = oldProject.Title,
                Description = "descrição atualizada",
                UserIds = userIds
            };

            var jsonModel = JsonConvert.SerializeObject(model);
            var response = await _client.PutJsonAsync($"{_basePath}/{project.Id}", jsonModel);
            var json = await response.Content.ReadAsJsonAsync<ProjectJson>();

            var updatedProject = _fakeServer.DbContext.Projects.WhereId(json.Project.Id)
                                                               .IncludeUserProject()
                                                               .SingleOrDefault();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEqual(oldProject.Description, json.Project.Description);
            Assert.Equal(oldProject.Title, json.Project.Title);
            Assert.NotEqual(oldProject.UserProjects.Select(x => x.UserId).Count(),
                            updatedProject.UserProjects.Select(x => x.UserId).Count());
            Assert.Single(oldProject.UserProjects);
            Assert.Equal(1, oldProject.UserProjects.Select(x => x.UserId).Count());
            Assert.Equal(2, updatedProject.UserProjects.Select(x => x.UserId).Count());
        }


        [Theory]
        [InlineData("&")]
        [InlineData("*")]
        public async Task ShouldNotUpdateProjectWhenIdIsInvalid(string id)
        {
            var project = new Project().Build();
            _fakeServer.DbContext.Projects.Add(project);
            _fakeServer.DbContext.SaveChanges();

            var model = new ProjectModel
            {
                Title = "teste2",
                Description = "descrição atualizada",
                UserIds = new List<int> { 1 }
            };

            var jsonModel = JsonConvert.SerializeObject(model);

            var response = await _client.PutJsonAsync($"{_basePath}/{id}", jsonModel);
            var json = await response.Content.ReadAsJsonAsync<BadRequestJson>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Id: Invalid.", json.Message);
        }

        [Fact]
        public async Task ShouldNotUpdateProjectWhenProjectNotFound()
        {
            var project = new Project().Build();
            _fakeServer.DbContext.Projects.Add(project);
            _fakeServer.DbContext.SaveChanges();

            var model = new ProjectModel
            {
                Title = "teste2",
                Description = "descrição atualizada",
                UserIds = new List<int> { 1 }
            };

            var jsonModel = JsonConvert.SerializeObject(model);

            var response = await _client.PutJsonAsync($"{_basePath}/5", jsonModel);
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("PROJECT_NOT_FOUND", json.Message);
        }

        [Fact]
        public async Task ShouldNotUpdateProjectWhenUserNotFound()
        {
            var project = new Project().Build();
            _fakeServer.DbContext.Projects.Add(project);
            _fakeServer.DbContext.SaveChanges();

            var model = new ProjectModel
            {
                Title = "teste2",
                Description = "descrição atualizada",
                UserIds = new List<int> { 5 }
            };

            var jsonModel = JsonConvert.SerializeObject(model);

            var response = await _client.PutJsonAsync($"{_basePath}/{project.Id}", jsonModel);
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("USER_NOT_FOUND", json.Message);
        }

    }
}