using api.Extensions.Http;
using api.Filters;
using api.Models.Interfaces;
using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Projects;
using api.Models.ViewModel.Projects;
using api.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v1/projects")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _service;

        public ProjectController(IProjectService service)
        {
            _service = service;
        }

        [HttpGet, Auth]
        public async Task<IActionResult> GetAllProjects()
        {
            var whoami = HttpContext.WhoAmI();
            var projects = await _service.GetAllProjects(whoami.User.Id);

            return new ProjectListJson(projects);
        }

        [HttpGet, Route("{project_id}"), Auth]
        public async Task<IActionResult> FindProject([FromRoute] string project_id)
        {
            if (String.IsNullOrEmpty(project_id))
                return new BadRequestJson("Project_id: Required.");

            int id;
            if (!int.TryParse(project_id, out id))
                return new BadRequestJson("Id: Invalid.");

            var response = await _service.FindProject(id);

            if (!response.success)
                return new NotFoundRequestJson(response.error);

            return new ProjectJson(response.project);
        }

        [HttpPost, Auth]
        public async Task<IActionResult> Create([FromBody] ProjectModel model)
        {
            var response = await _service.CreateProject(model);

            if (!String.IsNullOrEmpty(response.error))
            {
                if (response.error.Equals("USER_NOT_FOUND"))
                    return new NotFoundRequestJson(response.error);

                return new UnprocessableEntityJson(response.error);
            }

            return new ProjectJson(response.project);
        }

        [HttpPut, Route("{project_id}"), Auth]
        public async Task<IActionResult> Update([FromBody] ProjectModel model, [FromRoute] string project_id)
        {
            if (String.IsNullOrEmpty(project_id))
                return new BadRequestJson("Project_id: Required.");

            int id;

            if (!int.TryParse(project_id, out id))
                return new BadRequestJson("Id: Invalid.");

            var response = await _service.UpdateProject((model), id);

            if (!String.IsNullOrEmpty(response.error))
            {
                if (response.error.Equals("USER_NOT_FOUND") || response.error.Equals("PROJECT_NOT_FOUND"))
                    return new NotFoundRequestJson(response.error);

                return new UnprocessableEntityJson(response.error);
            }

            return new ProjectJson(response.project);
        }
    }
}