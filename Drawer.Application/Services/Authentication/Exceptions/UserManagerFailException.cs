using Drawer.Application.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Exceptions
{
    /// <summary>
    /// UserManager의 동작이 실패했을 경우 발생하는 예외
    /// </summary>
    public class UserManagerFailException: AppException
    {
        public UserManagerFailException(string? message) : base(message)
        {
        }
    }
}
