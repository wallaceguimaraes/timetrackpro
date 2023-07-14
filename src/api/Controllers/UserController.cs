using api.Filters;
using api.Models.Interfaces;
using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Employees;
using api.Models.ViewModel.Users;
using api.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet, Route("{id}"), Auth]
        public async Task<IActionResult> FindUser([FromRoute] string id)
        {
            if (String.IsNullOrEmpty(id))
                return new BadRequestJson("Id: Required.");

            int userId;

            if (!int.TryParse(id, out userId))
                return new BadRequestJson("Id: Invalid.");

            var (success, user, error) = await _service.FindUser(userId);

            if (!success)
                return new NotFoundRequestJson(error);

            return new UserJson(user);
        }

        [HttpPost, Auth]
        public async Task<IActionResult> Create([FromBody] UserModel model)
        {
            var (user, error) = await _service.CreateUser(model);

            if (!String.IsNullOrEmpty(error))
                return new UnprocessableEntityJson(error);

            return new UserJson(user);
        }

        [HttpPut, Route("{id}"), Auth]
        public async Task<IActionResult> Update([FromBody] UserModel model, [FromRoute] string id)
        {
            if (String.IsNullOrEmpty(id))
                return new BadRequestJson("Id: Required.");

            int userId;

            if (!int.TryParse(id, out userId))
                return new BadRequestJson("Id: Invalid.");

            var (user, error) = await _service.UpdateUser(model, userId);

            if (!String.IsNullOrEmpty(error))
            {
                if (error.Equals("USER_NOT_FOUND"))
                    return new NotFoundRequestJson(error);

                return new UnprocessableEntityJson(error);
            }

            return new UserJson(user);
        }
    }
}