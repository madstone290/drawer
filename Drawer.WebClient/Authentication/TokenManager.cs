using Drawer.Contract;
using Drawer.Contract.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace Drawer.WebClient.Authentication
{
    public class TokenManager : ITokenManager
    {
        class LoginState
        {
            public bool IsLoggedIn { get; }
            public string? Email { get; }
            public string? RefreshToken { get;}

            public LoginState(bool isLoggedIn, string? email, string? refreshToken)
            {
                IsLoggedIn = isLoggedIn;
                Email = email;
                RefreshToken = refreshToken;
            }
            public static LoginState False() => new(false, null, null);
            public static LoginState True(string email, string refreshToken) => new(true, email, refreshToken);

        }

        private readonly AuthenticationStateProvider _stateProvider;

        private readonly HttpClient _httpClient;

        private readonly ITokenStorage _tokenStorage;

        public TokenManager(HttpClient httpClient, AuthenticationStateProvider stateProvider, ITokenStorage tokenStorage)
        {
            _httpClient = httpClient;
            _stateProvider = stateProvider;
            _tokenStorage = tokenStorage;
        }

        public async Task<TokenResult> RefreshAccessTokenAsync()
        {
            var loginState = await GetLoginStateAsync();
            if (!loginState.IsLoggedIn)
                return new TokenResult(false, null);

            var responseMessage = await _httpClient.PostAsJsonAsync(ApiRoutes.Account.Refresh, 
                new RefreshRequest(loginState.Email!, loginState.RefreshToken!));

            var response = await responseMessage.Content.ReadFromJsonAsync<RefreshResponse>();

            await _tokenStorage.SaveAccessTokenAsync(response!.AccessToken);
            return new TokenResult(true, response!.AccessToken);
        }

        public async Task<TokenResult> GetAccessTokenAsync()
        {
            var loginState = await GetLoginStateAsync();
            if (!loginState.IsLoggedIn)
                return new TokenResult(false, null);

            var accessToken = await _tokenStorage.GetAccessTokenAsync();

            if (!string.IsNullOrWhiteSpace(accessToken))
                return new TokenResult(true, accessToken);
            else
                return new TokenResult(false, null);
        }

        /// <summary>
        /// 현재 사용자가 인증된 상태인지 확인한다.
        /// </summary>
        /// <returns></returns>
        async Task<LoginState> GetLoginStateAsync()
        {
            // 쿠키로 인증상태 판단
            var state = await _stateProvider.GetAuthenticationStateAsync();
            if (state == null)
                return LoginState.False();

            if (state.User.Identity == null)
                return LoginState.False();

            if (!state.User.Identity.IsAuthenticated)
                return LoginState.False();

            var emailClaim = state.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var refreshTokenClaim = state.User.Claims.First(x => x.Type == "RefreshToken");
            return LoginState.True(emailClaim.Value, refreshTokenClaim.Value);
        }
    }
}
