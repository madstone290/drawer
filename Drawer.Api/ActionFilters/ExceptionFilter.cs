using Drawer.Application.Config;
using Drawer.Domain.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Drawer.Shared.Contracts.Common;

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
            if (context.Exception is DomainException || context.Exception is AppException)
            {
                _logger.LogInformation(context.Exception, "BadRequest");

                var error = new ErrorResponse(context.Exception.Message, _codeProvider.GetErrorCode(context.Exception));
                context.Result = new BadRequestObjectResult(error);
                return;
            }

            if (context.Exception is DbUpdateException updateException)
            {
                if (updateException.InnerException is PostgresException postgresException)
                {
                    if (postgresException.SqlState == PostgresErrorCodes.ForeignKeyViolation)
                    {
                        var error = new ErrorResponse("외래키 제약 조건을 위반하였습니다", ErrorCodes.FOREIGN_KEY_VIOLATION);
                        context.Result = new BadRequestObjectResult(error);
                        _logger.LogInformation(context.Exception, "BadRequest");
                        return;
                    }
                }

                _logger.LogError(context.Exception, "InternalServerError");
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            _logger.LogError(context.Exception, "InternalServerError");
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);


        }
    }
}
