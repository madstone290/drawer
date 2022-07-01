using Drawer.WebClient.Authentication;
using Drawer.WebClient.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drawer.WebClient.Pages.Account
{
    /// <summary>
    /// ���� ���¸� �����Ѵ�.
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
        /// �������¸� �����Ѵ�.
        /// </summary>
        /// <param name="isCompanyMember">ȸ�� ������ ����</param>
        /// <param name="isCompanyOwner">ȸ�� ������ ����</param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(string redirectUri, bool isCompanyMember, bool isCompanyOwner)
        {
            await _authenticationManager.RefreshAsync(isCompanyMember, isCompanyOwner);
            return Redirect(Paths.Account.LoginCallback.AddQueryParam("redirectUri", redirectUri));
        }
        
    }
}
