using Drawer.WebClient.Authentication;
using Drawer.WebClient.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drawer.WebClient.Pages.Account
{
    /// <summary>
    /// 인증 상태를 갱신한다.
    /// </summary>
    public class RefreshHandlerModel : PageModel
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ITokenManager _tokenManager;

        public RefreshHandlerModel(IAuthenticationManager authenticationManager, ITokenManager tokenManager)
        {
            _authenticationManager = authenticationManager;
            _tokenManager = tokenManager;
        }

        /// <summary>
        /// 인증상태를 갱신한다.
        /// </summary>
        /// <param name="isCompanyMember">회사 구성원 여부</param>
        /// <param name="isCompanyOwner">회사 소유주 여부</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(string redirectUri, bool isCompanyMember, bool isCompanyOwner)
        {
            await _authenticationManager.RefreshAsync(isCompanyMember, isCompanyOwner);
            return Redirect(Paths.Account.LoginCallback.AddQueryParam("redirectUri", redirectUri));
        }
        
    }
}
