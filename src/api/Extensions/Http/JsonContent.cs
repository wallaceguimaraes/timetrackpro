using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace api.Extensions.Http
{
    public class JsonContent : StringContent
    {
        public JsonContent(object content) : this(JsonConvert.SerializeObject(content)) { }

        public JsonContent(string content) : base(content, Encoding.UTF8, "application/json") { }
    }
}