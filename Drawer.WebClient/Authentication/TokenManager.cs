using Drawer.Contract.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace Drawer.WebClient.Authentication
{
    public class TokenManager : ITokenManager
    {
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
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var emailClaim = state.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var refreshTokenClaim = state.User.Claims.FirstOrDefault(x => x.Type == "RefreshToken");

            if (emailClaim == null || refreshTokenClaim == null)
                return new TokenResult(false, null);

            var responseMessage = await _httpClient.PostAsJsonAsync("/api/account/refresh", new RefreshRequest(emailClaim.Value, refreshTokenClaim.Value));
            var response = await responseMessage.Content.ReadFromJsonAsync<RefreshResponse>();

            await _tokenStorage.SaveAccessTokenAsync(response!.AccessToken);
            return new TokenResult(true, response!.AccessToken);
        }

        public async Task<TokenResult> GetAccessTokenAsync()
        {
            var accessToken = await _tokenStorage.GetAccessTokenAsync();
            
            if (!string.IsNullOrWhiteSpace(accessToken))
                return new TokenResult(true, accessToken);
            else
                return new TokenResult(false, null);



        }
    }
}
