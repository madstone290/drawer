using Drawer.Application.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Exceptions
{
    /// <summary>
    /// 유효하지 않은 사용자Id를 이용해서 접근할 때 발생하는 예외
    /// </summary>
    public class InvalidUserIdException : AppException
    {
        public InvalidUserIdException() : base(Messages.InvalidEmail)
        {
        }
    }
}
