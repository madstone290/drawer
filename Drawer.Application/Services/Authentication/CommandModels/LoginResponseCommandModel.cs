using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Application.Services.Authentication.CommandModels
{
    /// <summary>
    /// 로그인 응답 모델
    /// </summary>
    public class LoginResponseCommandModel
    {
        public bool IsCompanyMember { get; set; }
        public bool IsCompanyOwner { get; set; }
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
