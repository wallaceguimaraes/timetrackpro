using System.Net;
using tests.Factories.Models.ViewModels;
using Newtonsoft.Json;
using test.Fakes;
using Xunit;
using api.Models.ViewModel.Times;
using api.Extensions.Http;
using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Users;
using tests.Factories.Models.EntityModels;
using api.Models.EntityModel.Times;
using api.Models.ResultModel.Successes.Times;
using api.Results.Errors;
using api.Models.ResultModel.Errors;
using Microsoft.EntityFrameworkCore;
using api.Extensions;

namespace tests.Functional
{
    public class TimeTest
    {
        private FakeServer _fakeServer;
        private HttpClient _client;
        private const string _basePath = "api/v1/times";

        public TimeTest()
        {
            _fakeServer = new FakeServer();
            _client = _fakeServer.CreateAuthenticatedClient();
        }

        [Fact]
        public async Task ShouldNotGetTimesByProjectWhenUserHasNotAuthorization()
        {
            var response = await _fakeServer.CreateUnauthenticatedClient().GetAsync($"{_basePath}/2");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task ShouldGetTimesByProject()
        {
            var userAuthenticated = _fakeServer.DbContext.Users.SingleOrDefault();
            var project = new Project().Build().WithUser(userAuthenticated);
            var time = new Time().Build(user: userAuthenticated, project: project);
            var time2 = new Time().Build(user: userAuthenticated,
                                         project: project,
                                         startDate: DateTime.Now.AddHours(3),
                                         endDate: DateTime.Now.AddHours(4));

            var otherUser = new User().Build(login: "usuario2", password: "yuihkjjhfh", name: "usuario novo", email: "usuario@mail.com");
            var time3 = new Time().Build(user: otherUser, project: project);

            _fakeServer.DbContext.Times.AddRange(time, time2, time3);
            _fakeServer.DbContext.SaveChanges();

            var response = await _client.GetAsync($"{_basePath}/{project.Id}");
            var json = await response.Content.ReadAsJsonAsync<TimeListJson>();


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(json.Time);
            Assert.Equal(2, json.Time.Count);
            Assert.Contains(json.Time, time => time.Time.ProjectId == project.Id &&
                                               time.Time.UserId == userAuthenticated.Id);
            Assert.Contains(json.Time, x => x.Time.Id == time.Id ||
                                            x.Time.Id == time2.Id);

        }


        [Theory]
        [InlineData("$")]
        [InlineData("@")]
        [InlineData("I")]
        public async Task ShouldNotGetTimesByProjectWhenProjectIdIsInvalid(string projectId)
        {
            var response = await _client.GetAsync($"{_basePath}/{projectId}");
            var json = await response.Content.ReadAsJsonAsync<BadRequestJson>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Id: Invalid.", json.Message);
        }

        [Fact]
        public async Task ShouldNotGetTimesByProjectWhenProjectHasNotTimeRegistered()
        {
            var userAuthenticated = _fakeServer.DbContext.Users.SingleOrDefault();
            var project1 = new Project().Build().WithUser(userAuthenticated);
            var project2 = new Project().Build().WithUser(userAuthenticated);

            var time1 = new Time().Build(user: userAuthenticated, project: project2);

            _fakeServer.DbContext.Projects.Add(project1);
            _fakeServer.DbContext.Times.Add(time1);
            _fakeServer.DbContext.SaveChanges();

            var response = await _client.GetAsync($"{_basePath}/{project1.Id}");
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("TIME_NOT_REGISTERED", json.Message);
        }


        [Fact]
        public async Task ShouldNotCreateTimeWhenHasNotAuthorization()
        {
            var model = new TimeModel().Build();

            var response = await _fakeServer.CreateUnauthenticatedClient().PostJsonAsync($"{_basePath}", model);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task ShouldCreateTime()
        {
            var userAuthenticated = _fakeServer.DbContext.Users.SingleOrDefault();
            var project1 = new Project().Build().WithUser(userAuthenticated);

            _fakeServer.DbContext.Projects.Add(project1);
            _fakeServer.DbContext.SaveChanges();

            var model = new TimeModel().Build(projectId: project1.Id,
                                              userId: userAuthenticated.Id);

            var response = await _client.PostJsonAsync($"{_basePath}", model);
            var json = await response.Content.ReadAsJsonAsync<TimeJson>();

            var time = _fakeServer.DbContext.Times.WhereId(json.Time.Id).SingleOrDefault();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(json.Time);
            Assert.Equal(model.UserId, json.Time.UserId);
            Assert.Equal(model.ProjectId, json.Time.ProjectId);
            Assert.Equal(model.StartedAt, json.Time.StartedAt);
            Assert.Equal(model.EndedAt, json.Time.EndedAt);
            Assert.Equal(model.StartedAt, time.StartedAt);
            Assert.Equal(model.EndedAt, time.EndedAt);
            Assert.Equal(model.UserId, time.UserId);
            Assert.Equal(model.ProjectId, time.ProjectId);
        }

        [Theory]
        [InlineData("USER_NOT_FOUND")]
        [InlineData("PROJECT_NOT_FOUND")]
        public async Task ShouldNotCreateTimeWhenProjectOrUserNotFound(string error)
        {
            var userAuthenticated = _fakeServer.DbContext.Users.SingleOrDefault();
            var project1 = new Project().Build().WithUser(userAuthenticated);

            _fakeServer.DbContext.Projects.Add(project1);
            _fakeServer.DbContext.SaveChanges();

            int projectId = project1.Id;
            int userId = userAuthenticated.Id;

            if (error.Equals("USER_NOT_FOUND"))
                userId = 10;

            if (error.Equals("PROJECT_NOT_FOUND"))
                projectId = 10;

            var model = new TimeModel().Build(projectId: projectId,
                                              userId: userId);

            var response = await _client.PostJsonAsync($"{_basePath}", model);
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(error, json.Message);
        }

        [Theory]
        [InlineData("USER_NOT_FOUND")]
        [InlineData("PROJECT_NOT_FOUND")]
        public async Task ShouldNotUpdateTimeWhenProjectOrUserNotFound(string error)
        {
            var userAuthenticated = _fakeServer.DbContext.Users.SingleOrDefault();
            var project1 = new Project().Build().WithUser(userAuthenticated);

            var time1 = new Time().Build(user: userAuthenticated, project: project1);

            _fakeServer.DbContext.Times.Add(time1);
            _fakeServer.DbContext.SaveChanges();

            int projectId = project1.Id;
            int userId = userAuthenticated.Id;

            if (error.Equals("USER_NOT_FOUND"))
                userId = 10;

            if (error.Equals("PROJECT_NOT_FOUND"))
                projectId = 10;

            var model = new TimeModel().Build(projectId: projectId,
                                              userId: userId);

            var jsonModel = JsonConvert.SerializeObject(model);
            var response = await _client.PutJsonAsync($"{_basePath}/{time1.Id}", jsonModel);
            var json = await response.Content.ReadAsJsonAsync<NotFoundRequestJson>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(error, json.Message);
        }

        [Fact]
        public async Task ShouldNotUpdateTimeWhenHasNotAuthorization()
        {
            var model = new TimeModel().Build();
            var jsonModel = JsonConvert.SerializeObject(model);

            var response = await _fakeServer.CreateUnauthenticatedClient().PutJsonAsync($"{_basePath}/2", jsonModel);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ShouldUpdateTime()
        {
            var userAuthenticated = _fakeServer.DbContext.Users.SingleOrDefault();
            var project1 = new Project().Build().WithUser(userAuthenticated);

            var time1 = new Time().Build(user: userAuthenticated, project: project1);

            _fakeServer.DbContext.Projects.Add(project1);
            _fakeServer.DbContext.Times.Add(time1);
            _fakeServer.DbContext.SaveChanges();

            var oldTime = _fakeServer.DbContext.Times.WhereId(time1.Id)
                                                     .AsNoTracking()
                                                     .SingleOrDefault();

            var model = new TimeModel
            {
                StartedAt = oldTime.StartedAt.AddHours(1),
                EndedAt = oldTime.EndedAt.AddHours(4),
                UserId = userAuthenticated.Id,
                ProjectId = project1.Id
            };

            var jsonModel = JsonConvert.SerializeObject(model);
            var response = await _client.PutJsonAsync($"{_basePath}/{time1.Id}", jsonModel);
            var json = await response.Content.ReadAsJsonAsync<TimeJson>();

            _fakeServer.DbContext.ReloadAllEntries();

            var updatedTime = _fakeServer.DbContext.Times.WhereId(json.Time.Id).SingleOrDefault();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEqual(oldTime.StartedAt, json.Time.StartedAt);
            Assert.NotEqual(oldTime.EndedAt, json.Time.EndedAt);
            Assert.Equal(oldTime.UserId, json.Time.UserId);
            Assert.Equal(oldTime.ProjectId, json.Time.ProjectId);
            Assert.NotEqual(oldTime.StartedAt, updatedTime.StartedAt);
            Assert.NotEqual(oldTime.EndedAt, updatedTime.EndedAt);
            Assert.Equal(oldTime.UserId, updatedTime.UserId);
            Assert.Equal(oldTime.ProjectId, updatedTime.ProjectId);
        }

        [Theory]
        [InlineData("post")]
        [InlineData("put")]
        public async Task ShouldNotCreateOrUpdateTimesWhenTimeIsInvalid(string verbHttp)
        {
            var model = new TimeModel().Build(endDate: DateTime.Now.AddHours(-6));
            HttpResponseMessage response = new HttpResponseMessage();

            if (verbHttp.Equals("post"))
                response = await _client.PostJsonAsync($"{_basePath}", model);
            if (verbHttp.Equals("put"))
            {
                var jsonModel = JsonConvert.SerializeObject(model);
                response = await _client.PutJsonAsync($"{_basePath}/1", jsonModel);
            }

            var json = await response.Content.ReadAsJsonAsync<BadRequestJson>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("ended_at cannot be less than or equal to started_at", json.Message);
        }

    }
}