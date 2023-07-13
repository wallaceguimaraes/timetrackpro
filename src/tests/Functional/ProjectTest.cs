using System.Net;
using api.Extensions.Http;
using api.Models.EntityModel.Projects;
using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Projects;
using api.Models.ViewModel.Projects;
using api.Results.Errors;
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
            var project = new Project().Build();
            _fakeServer.DbContext.Projects.Add(project);
            _fakeServer.DbContext.SaveChanges();

            var response = await _client.GetAsync($"{_basePath}/{project.Id}");
            var json = await response.Content.ReadAsJsonAsync<ProjectJson>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(json.Project);
            Assert.Equal(project.Title, json.Project.Title);
            Assert.Equal(project.Description, json.Project.Description);
        }

        [Fact]
        public async Task ShouldNotFindProjectWhenUserHasNotAuthorization()
        {
            var response = await _fakeServer.CreateUnauthenticatedClient().GetAsync($"{_basePath}/2");
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData("gh")]
        [InlineData("%")]
        public async Task ShouldNotFindUserWhenIdIsInvalid(string id)
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
            var model = new ProjectModel().Build();
            var response = await _fakeServer.CreateUnauthenticatedClient().PostJsonAsync($"{_basePath}", model);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

    }
}