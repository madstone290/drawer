using Drawer.Application.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Exceptions
{
    /// <summary>
    /// 검증되지 않은 이메일을 이용한 로그인시 발생하는 예외
    /// </summary>
    public class NotConfirmedEmailException : AppException
    {
        public NotConfirmedEmailException() : base(Messages.NotConfirmedEmail)
        {
        }
    }
}
