using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace api.Extensions.Http
{
    public static class HttpContentExtensions
    {
        public static async Task<TResult> ReadAsJsonAsync<TResult>(this HttpContent httpContent)
        {
            return JsonConvert.DeserializeObject<TResult>(await httpContent.ReadAsStringAsync());
        }
    }
}