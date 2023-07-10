using api.Models.ServiceModel.Users;
using api.Results.Errors;

namespace api.Models.ResultModel.Errors
{
    public class UserErrorResult : UnprocessableEntityJson
    {
        public UserErrorResult() { }

        public UserErrorResult(UserService service)
        {
            if (service.UserRegisterError)
                Message = "USER_REGISTER_ERROR";
            if (service.UserUpdateError)
                Message = "USER_UPDATE_ERROR";
            if (service.UserNotFound)
                Message = "USER_NOT_FOUND";

        }
    }
}