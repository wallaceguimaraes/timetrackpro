using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace api.Filters
{
    public class ErrorHandlerAttribute : ExceptionFilterAttribute
    {
        private IWebHostEnvironment _env;
        private ILogger<ErrorHandlerAttribute> _logger;

        public ErrorHandlerAttribute(IWebHostEnvironment env, ILogger<ErrorHandlerAttribute> logger)
        {
            _env = env;
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (!_env.IsDevelopment())
            {
                _logger.LogError(context.Exception, "Error");
            }

            context.Result = new StatusCodeResult(500);
        }
    }
}
