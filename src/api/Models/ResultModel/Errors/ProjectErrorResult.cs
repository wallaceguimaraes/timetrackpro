using api.Models.ServiceModel.Projects;
using api.Results.Errors;

namespace api.Models.ResultModel.Errors
{
    public class ProjectErrorResult : UnprocessableEntityJson
    {
        public ProjectErrorResult() { }

        public ProjectErrorResult(ProjectService service)
        {
            if (service.ProjectRegisterError)
                Message = "PROJECT_REGISTER_ERROR";
            if (service.ProjectExisting)
                Message = "PROJECT_EXISTING";
            if (service.ProjectNotFound)
                Message = "PROJECT_NOT_FOUND";
            if (service.ProjectUpdateError)
                Message = "PROJECT_UPDATE_ERROR";

        }
    }
}