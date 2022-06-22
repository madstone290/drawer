using Drawer.Application.Exceptions;
using Drawer.Contract.Common;

namespace Drawer.Api.ActionFilters
{
    /// <summary>
    /// 예외에 맞는 에러코드를 제공한다.
    /// </summary>
    public class ExceptionCodeProvider
    {
        public string GetErrorCode(Exception exception)
        {
            if (exception is UnconfirmedEmailException)
                return ErrorCodes.UnconfirmedEmail;

            return string.Empty;
        }
    }
}
