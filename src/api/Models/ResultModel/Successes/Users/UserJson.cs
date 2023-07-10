using api.Models.EntityModel.Users;
using Microsoft.AspNetCore.Mvc;

namespace api.Models.ResultModel.Successes.Employees
{
    public class UserJson : IActionResult
    {
        public UserJson() { }

        public UserJson(User user)
        {
            Id = user.Id.ToString();
            Name = user.Name;
            Email = user.Email;
            Login = user.Login;
            CreatedAt = user.CreatedAt;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string TaxDocument { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public DateTime CreatedAt { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}