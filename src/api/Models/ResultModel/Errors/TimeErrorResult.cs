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
            if (service.TimeNotRegistered)
                Message = "TIME_NOT_REGISTERED";
            if (service.TimeNotFound)
                Message = "TIME_NOT_FOUND";
            if (service.TimeUpdateError)
                Message = "TIME_UPDATE_ERROR";
            if (service.ProjectNotFound)
                Message = "PROJECT_NOT_FOUND";
            if (service.UserNotFound)
                Message = "USER_NOT_FOUND";
        }
    }
}