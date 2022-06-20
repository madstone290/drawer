using Drawer.Application.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Exceptions
{
    /// <summary>
    /// 리프레시 토큰이 유효하지 않을 경우 발생하는 예외
    /// </summary>
    public class InvalidRefreshTokenException : AppException
    {
        public InvalidRefreshTokenException() : base(Messages.InvalidRefreshToken)
        {
        }
    }
}
