using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Times;
using api.Models.EntityModel.Users;
using Microsoft.AspNetCore.Mvc;

namespace api.Models.ResultModel.Successes.Times
{
    public class TimeJson : IActionResult
    {
        public TimeJson() { }

        public TimeJson(Time time)
        {
            Id = time.Id.ToString();
            StartedAt = time.StartedAt;
            EndedAt = time.EndedAt;
            Project = time.Project;
            User = time.User;

        }

        public string Id { get; set; }
        public Project Project { get; set; }
        public User User { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }


        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}