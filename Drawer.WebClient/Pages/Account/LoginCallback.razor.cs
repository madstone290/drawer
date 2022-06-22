using Drawer.WebClient.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Drawer.WebClient.Pages.Account
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

        [Inject] AuthenticationStateProvider StateProvider { get; set; } = null!;
        [Inject] ITokenStorage TokenStorage { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            // 로그인 쿠키에 포함된 액세스 토큰을 저장소에 보관한다.
            var state = await StateProvider.GetAuthenticationStateAsync();
            var accessTokenClaim = state.User.Claims.FirstOrDefault(x => x.Type == TokenClaimTypes.AccessToken);
            if (accessTokenClaim != null)
            {
                await TokenStorage.SaveAccessTokenAsync(accessTokenClaim.Value);
            }

            if (RedirectUri == null)
                NavManager.NavigateTo(Paths.Base);
            else
                NavManager.NavigateTo(RedirectUri);


        }
    }
}
