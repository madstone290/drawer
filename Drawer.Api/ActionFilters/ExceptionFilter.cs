using Drawer.Application.Config;
using Drawer.Contract.Common;
using Drawer.Domain.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Drawer.Api.ActionFilters
{
    public class DefaultExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<DefaultExceptionFilter> _logger;
        private readonly ExceptionCodeProvider _codeProvider;

        public DefaultExceptionFilter(ILogger<DefaultExceptionFilter> logger, ExceptionCodeProvider codeProvider)
        {
            _logger = logger;
            _codeProvider = codeProvider;
        }

        public void OnException(ExceptionContext context)
        {
            if(context.Exception is DomainException ||
                context.Exception is AppException)
            {
                _logger.LogInformation(context.Exception, "BadRequest");

                var error = new ErrorResponse(context.Exception.Message, _codeProvider.GetErrorCode(context.Exception));
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
