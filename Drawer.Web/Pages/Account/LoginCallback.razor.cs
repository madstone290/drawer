using Drawer.Web.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Drawer.Web.Pages.Account
{
    /// <summary>
    /// 서버에서 로그인 후 이 페이지로 리디렉트 된다.
    /// </summary>
    public partial class LoginCallback
    {
        /// <summary>
        /// 로그인 콜백 실행 후 리디렉트 될 Uri
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string? RedirectUri { get; set; }

        [Inject] ITokenManager TokenManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            // 쿠키에 있는 리프레시 토큰을 이용해 액세스 토큰을 생성하고 로컬저장소에 보관한다.
            await TokenManager.RefreshAccessTokenAsync();
            
            if (RedirectUri == null)
                NavManager.NavigateTo(Paths.Base);
            else
                NavManager.NavigateTo(RedirectUri);


        }
    }
}
