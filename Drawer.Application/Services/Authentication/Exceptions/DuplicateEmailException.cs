using Drawer.Application.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.Exceptions
{
    /// <summary>
    /// 중복된 이메일이 존재하는 경우 발생하는 예외
    /// </summary>
    public class DuplicateEmailException : AppException
    {
        public DuplicateEmailException() : base(Messages.DuplicateEmail)
        {

        }
    }
}
