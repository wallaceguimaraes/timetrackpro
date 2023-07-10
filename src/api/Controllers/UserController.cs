using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Employees;
using api.Models.ServiceModel.Users;
using api.Models.ViewModel.Users;
using api.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v{n}/users")]
    public class UserController : Controller
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> FindUser([FromRoute] string id)
        {
            if (String.IsNullOrEmpty(id))
                return new BadRequestJson("id: required.");

            if (!await _service.FindUser(id))
                return new UserErrorResult(_service);

            return new UserJson(_service.User);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserModel model)
        {
            if (!await _service.CreateUser(model))
                return new UserErrorResult(_service);

            return new UserJson(_service.User);
        }

        [HttpPut, Route("{id}")]
        public async Task<IActionResult> Update([FromBody] UserModel model, [FromRoute] string userId)
        {
            if (!await _service.UpdateUser(model, (Convert.ToInt32(userId))))
                return new UserErrorResult(_service);

            return new UserJson(_service.User);
        }
    }
}