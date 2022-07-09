using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Contract.Authentication
{
    /// <summary>
    /// 인증을 완료하고 RedirectUri로 리디렉트한다.
    /// </summary>
    /// <param name="Email">인증 이메일</param>
    /// <param name="RedirectUri">인증 완료 후 리디렉트할 Uri</param>
    public record ConfirmEmailRequest(string Email, string RedirectUri);
}
