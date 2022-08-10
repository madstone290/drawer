using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.CommandModels
{
    public class EmailConfirmationCommandModel
    {
        /// <summary>
        /// 수신 이메일 주소
        /// </summary>
        public string Email { get; set; } = default!;

        /// <summary>
        /// 리디렉트 주소
        /// </summary>
        public string RedirectUri { get; set; } = default!;
    }
}
