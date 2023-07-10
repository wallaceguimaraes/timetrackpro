using Microsoft.AspNetCore.Mvc.Filters;
using api.Infrastructure.Mvc;
using api.Results.Errors;

namespace api.Filters
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionArguments.Any(arg => arg.Value == null))
            {
                filterContext.Result = new BadRequestJson("Request body cannot be blank.");
            }
            else if (!filterContext.ModelState.IsValid)
            {
                var errors = filterContext.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => new ModelErrorMessage(e))
                    .ToList();

                filterContext.Result = new BadRequestJson(errors);
            }
        }
    }
}