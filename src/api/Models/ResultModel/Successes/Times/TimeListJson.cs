using api.Models.EntityModel.Times;
using Microsoft.AspNetCore.Mvc;

namespace api.Models.ResultModel.Successes.Times
{
    public class TimeListJson : IActionResult
    {
        public TimeListJson() { }

        public TimeListJson(ICollection<Time> times)
        {
            Time = times.Select(time => new TimeJson(time)).ToList();
        }

        public ICollection<TimeJson> Time { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}