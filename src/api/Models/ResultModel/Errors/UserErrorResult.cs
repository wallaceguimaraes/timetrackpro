using api.Results.Errors;

namespace api.Models.ResultModel.Errors
{
    public class UserErrorResult : UnprocessableEntityJson
    {
        public UserErrorResult() { }

        public UserErrorResult(string error)
        {
            Message = error;
        }
    }
}