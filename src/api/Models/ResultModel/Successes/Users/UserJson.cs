using api.Models.EntityModel.Users;
using Microsoft.AspNetCore.Mvc;

namespace api.Models.ResultModel.Successes.Employees
{
    public class UserJson : IActionResult
    {
        public UserJson() { }

        public UserJson(User user)
        {
            User = user;
        }

        public User User { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}