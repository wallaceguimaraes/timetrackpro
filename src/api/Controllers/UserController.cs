using api.Filters;
using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Employees;
using api.Models.ServiceModel.Users;
using api.Models.ViewModel.Users;
using api.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet, Route("{id}"), Auth]
        public async Task<IActionResult> FindUser([FromRoute] string id)
        {
            if (String.IsNullOrEmpty(id))
                return new BadRequestJson("id: required.");

            int userId;

            if (!int.TryParse(id, out userId))
                return new BadRequestJson("id: invalid.");

            if (!await _service.FindUser(userId))
                return new NotFoundRequestJson("USER_NOT_FOUND");

            return new UserJson(_service.User);
        }

        [HttpPost, Auth]
        public async Task<IActionResult> Create([FromBody] UserModel model)
        {
            if (!await _service.CreateUser(model))
                return new UserErrorResult(_service);

            return new UserJson(_service.User);
        }

        [HttpPut, Route("{id}"), Auth]
        public async Task<IActionResult> Update([FromBody] UserModel model, [FromRoute] string id)
        {
            if (String.IsNullOrEmpty(id))
                return new BadRequestJson("id: required.");

            int userId;

            if (!int.TryParse(id, out userId))
                return new BadRequestJson("id: invalid.");

            if (!await _service.UpdateUser(model, userId))
                return new UserErrorResult(_service);

            return new UserJson(_service.User);
        }
    }
}