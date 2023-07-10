using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Times;
using Microsoft.AspNetCore.Mvc;

namespace api.Models.ResultModel.Successes.Projects
{
    public class ProjectJson : IActionResult
    {
        public ProjectJson() { }

        public ProjectJson(Project project)
        {
            Id = project.Id.ToString();
            Title = project.Title;
            Description = project.Description;
            Times = project.Times;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Time>? Times { get; set; }


        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}