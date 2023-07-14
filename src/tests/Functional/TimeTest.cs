using System.Net;
using tests.Factories.Models.ViewModels;
using Newtonsoft.Json;
using test.Fakes;
using Xunit;
using api.Models.ViewModel.Times;
using api.Extensions.Http;

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
        public async Task ShouldNotCreateTimeWhenHasNotAuthorization()
        {
            var model = new TimeModel().Build();

            var response = await _fakeServer.CreateUnauthenticatedClient().PostJsonAsync($"{_basePath}", model);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task ShouldNotUpdateTimeWhenHasNotAuthorization()
        {
            var model = new TimeModel().Build();
            var jsonModel = JsonConvert.SerializeObject(model);

            var response = await _fakeServer.CreateUnauthenticatedClient().PutJsonAsync($"{_basePath}/2", jsonModel);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}