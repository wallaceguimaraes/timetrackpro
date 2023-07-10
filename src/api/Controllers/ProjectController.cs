using api.Filters;
using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Projects;
using api.Models.ServiceModel.Projects;
using api.Models.ViewModel.Projects;
using api.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v1/projects")]
    public class ProjectController : Controller
    {
        private readonly ProjectService _service;

        public ProjectController(ProjectService service)
        {
            _service = service;
        }

        [HttpGet, Auth]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _service.GetAllProjects();

            return new ProjectListJson(projects);
        }

        [HttpGet, Route("{project_id}"), Auth]
        public async Task<IActionResult> FindProject([FromRoute] string project_id)
        {
            if (String.IsNullOrEmpty(project_id))
                return new BadRequestJson("project_id: required.");

            int id;
            if (!int.TryParse(project_id, out id))
                return new BadRequestJson("id: invalid.");

            if (!await _service.FindProject(id))
                return new NotFoundRequestJson("PROJECT_NOT_FOUND");

            return new ProjectJson(_service.Project);
        }

        [HttpPost, Auth]
        public async Task<IActionResult> Create([FromBody] ProjectModel model)
        {
            if (!await _service.CreateProject(model))
                return new ProjectErrorResult(_service);

            return new ProjectJson(_service.Project);
        }

        [HttpPut, Route("{project_id}"), Auth]
        public async Task<IActionResult> Update([FromBody] ProjectModel model, [FromRoute] string project_id)
        {
            if (String.IsNullOrEmpty(project_id))
                return new BadRequestJson("project_id: required.");

            int id;

            if (!int.TryParse(project_id, out id))
                return new BadRequestJson("id: invalid.");

            if (!await _service.UpdateProject((model), id))
                return new ProjectErrorResult(_service);

            return new ProjectJson(_service.Project);
        }
    }
}