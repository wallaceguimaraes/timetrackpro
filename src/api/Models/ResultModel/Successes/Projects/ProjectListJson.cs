using api.Models.EntityModel.Projects;
using Microsoft.AspNetCore.Mvc;

namespace api.Models.ResultModel.Successes.Projects
{
    public class ProjectListJson : IActionResult
    {
        public ProjectListJson() { }

        public ProjectListJson(ICollection<Project> projects)
        {
            Projects = projects.Select(project => new ProjectJson(project)).ToList();
        }

        public ICollection<ProjectJson> Projects { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new JsonResult(this).ExecuteResultAsync(context);
        }
    }
}