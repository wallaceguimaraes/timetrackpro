using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace api.Extensions.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostJsonAsync(this HttpClient httpClient, string requestUri, object content)
        {
            var jsonContent = JsonConvert.SerializeObject(content);
            return await httpClient.PostJsonAsync(requestUri, jsonContent);
        }

        public static async Task<HttpResponseMessage> PostJsonAsync(this HttpClient httpClient, string requestUri, string content)
        {
            var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(requestUri, jsonContent);
        }

        public static async Task<HttpResponseMessage> PostJsonAsync(this HttpClient httpClient, string requestUri)
        {
            var jsonContent = new StringContent("{}", Encoding.UTF8, "application/json");
            return await httpClient.PostAsync(requestUri, jsonContent);
        }

        public static async Task<HttpResponseMessage> PutJsonAsync(this HttpClient httpClient, string requestUri, object content)
        {
            return await httpClient.PutAsJsonAsync(requestUri, content);
        }

        public static async Task<HttpResponseMessage> PutJsonAsync(this HttpClient httpClient, string requestUri, string content)
        {
            var jsonContent = new StringContent(content, Encoding.UTF8, "application/json");
            return await httpClient.PutAsync(requestUri, jsonContent);
        }

        public static async Task<HttpResponseMessage> PutJsonAsync(this HttpClient httpClient, string requestUri)
        {
            var jsonContent = new StringContent("{}", Encoding.UTF8, "application/json");
            return await httpClient.PutAsync(requestUri, jsonContent);
        }

        public static bool ContainsAuthorizationHeader(this HttpClient httpClient)
        {
            return httpClient.DefaultRequestHeaders.Contains(HeaderNames.Authorization);
        }
    }
}