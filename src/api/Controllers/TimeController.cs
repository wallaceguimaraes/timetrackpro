using api.Models.ResultModel.Errors;
using api.Models.ResultModel.Successes.Times;
using api.Models.ServiceModel.Times;
using api.Models.ViewModel.Times;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v{n}/times")]
    public class TimeController : Controller
    {
        private readonly TimeService _service;

        public TimeController(TimeService timeService)
        {
            _service = timeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TimeModel model)
        {
            if (!await _service.CreateTime(model.Map()))
                return new TimeErrorResult(_service);

            return new TimeJson(_service.Time);
        }


    }
}