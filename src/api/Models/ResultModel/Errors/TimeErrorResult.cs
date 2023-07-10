using api.Models.ServiceModel.Times;
using api.Results.Errors;

namespace api.Models.ResultModel.Errors
{
    public class TimeErrorResult : UnprocessableEntityJson
    {
        public TimeErrorResult() { }

        public TimeErrorResult(TimeService service)
        {
            if (service.TimeRegisterError)
                Message = "TIME_REGISTER_ERROR";

        }
    }
}