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

        public DefaultExceptionFilter(ILogger<DefaultExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is DomainException domainException)
            {
                _logger.LogInformation(domainException, "BadRequest");

                context.Result = new BadRequestObjectResult(new ErrorResponse(domainException.Message));
                return;
            }

            if (context.Exception is AppException appException)
            {
                _logger.LogInformation(appException, "BadRequest");

                context.Result = new BadRequestObjectResult(new ErrorResponse(appException.Message, appException.Code));
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
