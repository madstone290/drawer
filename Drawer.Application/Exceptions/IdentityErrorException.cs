using Drawer.Application.Config;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Exceptions
{
    /// <summary>
    /// AspNetCore Identity기능을 사용하다 발생하는 예외
    /// </summary>
    public class IdentityErrorException : AppException
    {
        private const string PasswordMismatchCode = "PasswordMismatch";

        public IdentityErrorException(IEnumerable<IdentityError> errors) : base(LocalizeMessage(errors))
        {

        }

        /// <summary>
        /// 사용자가 인식할 수 있는 메시지로 변경한다.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static string LocalizeMessage(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error =>
            {
                if (error.Code == PasswordMismatchCode)
                {
                    return Messages.PasswordMitmatch;
                }
                else
                {
                    return error.Description;
                }
            }));
            
        }
    }
}
