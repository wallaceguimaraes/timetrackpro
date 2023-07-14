using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/request")]
    public class RequestController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Ok();
    }
}