using Drawer.Application.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Exceptions
{
    /// <summary>
    /// 등록된 회사가 존재하는 경우 발생하는 예외
    /// </summary>
    public class CompanyAlreadyExistException : AppException
    {
        public CompanyAlreadyExistException() : base(Messages.CompanyAlreadyExist)
        {
        }
    }
}
