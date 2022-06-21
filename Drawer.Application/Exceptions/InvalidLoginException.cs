using Drawer.Application.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Exceptions
{
    /// <summary>
    /// 로그인이 유효하지 않을 때 발생하는 예외
    /// </summary>
    public class InvalidLoginException : AppException
    {
        public InvalidLoginException() : base(Messages.InvalidLogin)
        {
        }
    }
}
