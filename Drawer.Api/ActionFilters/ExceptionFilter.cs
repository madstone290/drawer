using Drawer.Application.Config;
using Drawer.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Drawer.Api.ActionFilters
{
    public class DefaultExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<DefaultExceptionFilter> _logger;

        public DefaultExceptionFilter(ILogger<DefaultExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if(context.Exception is DomainException ||
                context.Exception is AppException)
            {
                _logger.LogInformation(context.Exception, "BadRequest");
                
                var error = new
                {
                    Error = context.Exception.Message,
                };
                context.Result = new BadRequestObjectResult(error);
            }
            else
            {
                _logger.LogError(context.Exception, "InternalServerError");
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
