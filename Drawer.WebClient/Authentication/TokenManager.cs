using Drawer.Contract;
using Drawer.Contract.Authentication;

namespace Drawer.WebClient.Authentication
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

            var responseMessage = await _httpClient.PostAsJsonAsync(ApiRoutes.Account.Refresh, 
                new RefreshRequest(state.Email!, state.RefreshToken!));

            var response = await responseMessage.Content.ReadFromJsonAsync<RefreshResponse>();

            await _tokenStorage.SaveAccessTokenAsync(response!.AccessToken);
            return new TokenResult(true, response!.AccessToken);
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
