using api.Infrastructure.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace api.Models.ResultModel.Errors
{
    public class NotFoundRequestJson : IActionResult
    {
        public NotFoundRequestJson() { }

        public NotFoundRequestJson(string error)
        {
            Message = error;
        }

        public NotFoundRequestJson(ModelErrorMessage modelErrorMessage)
        {
            Message = modelErrorMessage.ToString();
        }

        public string Message { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var json = new JsonResult(this) { StatusCode = 404 };
            await json.ExecuteResultAsync(context);
        }
    }
}