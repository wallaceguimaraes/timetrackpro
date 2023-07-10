using api.Models.EntityModel.Users;
using Microsoft.AspNetCore.Mvc;

namespace api.ResultModel.Successes.Authentication
{
    public class TokenJson : IActionResult
    {
        public TokenJson() { }

        public TokenJson(string token, User user)
        {
            Token = token;
            User = user;
        }

        public string Token { get; set; }
        public User User { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}