using api.Models.EntityModel.Projects;
using Microsoft.AspNetCore.Mvc;

namespace api.Models.ResultModel.Successes.Projects
{
    public class ProjectJson : IActionResult
    {
        public ProjectJson() { }

        public ProjectJson(Project project)
        {
            Project = project;
        }

        public Project Project { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}