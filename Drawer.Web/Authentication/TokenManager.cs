using Drawer.Application.Services.Authentication.CommandModels;
using Drawer.Shared;

namespace Drawer.Web.Authentication
{
    public class TokenManager : ITokenManager
    {
        private readonly IAuthenticationManager _authenticationManager;

        private readonly HttpClient _httpClient;

        private readonly ITokenStorage _tokenStorage;

        public TokenManager(HttpClient httpClient, IAuthenticationManager authenticationManager, ITokenStorage tokenStorage)
        {
            _httpClient = httpClient;
            _authenticationManager = authenticationManager;
            _tokenStorage = tokenStorage;
        }

        public async Task<TokenResult> RefreshAccessTokenAsync()
        {
            var state = await _authenticationManager.GetUserStateAsync();
            if (!state.IsAuthenticated)
                return new TokenResult(false, null);

            var refreshDto = new RefreshCommandModel()
            {
                Email = state.Email!,
                RefreshToken = state.RefreshToken!
            };
            var response = await _httpClient.PostAsJsonAsync(
                ApiRoutes.Account.Refresh, 
                refreshDto);

            var accessToken = await response.Content.ReadFromJsonAsync<string>() ?? default!;

            await _tokenStorage.SaveAccessTokenAsync(accessToken);
            return new TokenResult(true, accessToken);
        }

        public async Task<TokenResult> GetAccessTokenAsync()
        {
            var state = await _authenticationManager.GetUserStateAsync();
            if (!state.IsAuthenticated)
                return new TokenResult(false, null);

            var accessToken = await _tokenStorage.GetAccessTokenAsync();

            if (!string.IsNullOrWhiteSpace(accessToken))
                return new TokenResult(true, accessToken);
            else
                return new TokenResult(false, null);
        }

    }
}
