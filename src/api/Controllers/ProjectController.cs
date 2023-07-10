using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Projects;
using api.Models.ServiceModel.Projects;
using api.Models.ViewModel.Projects;
using api.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v{n}/projects")]
    public class ProjectController : Controller
    {
        private readonly ProjectService _service;

        public ProjectController(ProjectService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _service.GetAllProjects();

            return new ProjectListJson(projects);
        }

        [HttpGet, Route("{project_id}")]
        public async Task<IActionResult> FindProject([FromRoute] string projectId)
        {
            if (String.IsNullOrEmpty(projectId))
                return new BadRequestJson("project_id: required.");

            if (!await _service.FindProject(projectId))
                return new ProjectErrorResult(_service);

            return new ProjectJson(_service.Project);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectModel model)
        {
            if (!await _service.CreateProject(model))
                return new ProjectErrorResult(_service);

            return new ProjectJson(_service.Project);
        }

        [HttpPut, Route("{project_id}")]
        public async Task<IActionResult> Update([FromBody] ProjectModel model, [FromRoute] string projectId)
        {
            if (String.IsNullOrEmpty(projectId))
                return new BadRequestJson("project_id: required.");

            if (!await _service.UpdateProject((model), Convert.ToInt32(projectId)))
                return new ProjectErrorResult(_service);

            return new ProjectJson(_service.Project);
        }
    }
}