using Drawer.WebClient.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Drawer.WebClient.Pages.Account
{
    /// <summary>
    /// 서버에서 로그아웃 후 이 페이지로 리디렉트된다.
    /// </summary>
    public partial class LogoutCallback
    {
        /// <summary>
        /// 로그아웃 콜백 실행 후 리디렉트 될 Uri
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string? RedirectUri { get; set; }

        [Inject] ITokenStorage TokenStorage { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            // 저장소에 보관된 토큰을 삭제한다.
            await TokenStorage.ClearAccessTokenAsync();

            if (RedirectUri == null)
                _navigationManager.NavigateTo(Paths.Base);
            else
                _navigationManager.NavigateTo(RedirectUri);
        }
    }
}
