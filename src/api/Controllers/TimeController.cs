using api.Filters;
using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Times;
using api.Models.ServiceModel.Times;
using api.Models.ViewModel.Times;
using api.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v1/times")]
    public class TimeController : Controller
    {
        private readonly TimeService _service;

        public TimeController(TimeService timeService)
        {
            _service = timeService;
        }

        [HttpPost, Auth]
        public async Task<IActionResult> Create([FromBody] TimeModel model)
        {
            if (!await _service.CreateTime(model.Map()))
                return new TimeErrorResult(_service);

            return new TimeJson(_service.Time);
        }

        [HttpGet, Route("{project_id}"), Auth]
        public async Task<IActionResult> GetTimesByProject([FromRoute] string project_id)
        {
            if (String.IsNullOrEmpty(project_id))
                return new BadRequestJson("project_id: required.");

            int id;

            if (!int.TryParse(project_id, out id))
                return new BadRequestJson("id: invalid.");


            if (!await _service.GetTimesByProject(id))
                return new TimeErrorResult(_service);

            return new TimeListJson(_service.Times);
        }

        [HttpPut, Route("{time_id}"), Auth]
        public async Task<IActionResult> UpdateTime([FromBody] TimeModel model, [FromRoute] string time_id)
        {
            if (String.IsNullOrEmpty(time_id))
                return new BadRequestJson("project_id: required.");

            int id;

            if (!int.TryParse(time_id, out id))
                return new BadRequestJson("id: invalid.");

            if (!await _service.UpdateTime(model, id))
                return new TimeErrorResult(_service);

            return new TimeJson(_service.Time);
        }

    }
}