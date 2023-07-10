using Microsoft.AspNetCore.Mvc;

namespace api.Results.Errors
{
    public class UnprocessableEntityJson : IActionResult
    {
        public UnprocessableEntityJson() { }

        public UnprocessableEntityJson(string error)
        {
            Message = error;
        }

        public string Message { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var json = new JsonResult(this) { StatusCode = 422 };
            await json.ExecuteResultAsync(context);
        }
    }
}