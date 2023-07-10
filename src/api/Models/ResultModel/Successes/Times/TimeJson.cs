using api.Models.EntityModel.Times;
using Microsoft.AspNetCore.Mvc;

namespace api.Models.ResultModel.Successes.Times
{
    public class TimeJson : IActionResult
    {
        public TimeJson() { }

        public TimeJson(Time time)
        {
            Time = time;
        }

        public Time Time { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}