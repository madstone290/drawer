using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Drawer.WebClient.Authentication
{
    public class TokenStorage : ITokenStorage
    {
        private const string AccessTokenKey = TokenClaimTypes.AccessToken;

        private readonly ProtectedLocalStorage _localStorage;

        public TokenStorage(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SaveAccessTokenAsync(string accessToken)
        {
            await _localStorage.SetAsync(AccessTokenKey, accessToken);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            var result = await _localStorage.GetAsync<string>(AccessTokenKey);
            if (result.Success)
                return result.Value;
            else
                return null;
        }

        public async Task ClearAccessTokenAsync()
        {
            await _localStorage.DeleteAsync(AccessTokenKey);
        }


    }
}
