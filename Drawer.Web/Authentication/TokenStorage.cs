using Blazored.LocalStorage;
using Drawer.Shared;
using Drawer.Web.Frontend;

namespace Drawer.Web.Authentication
{
    public class TokenStorage : ITokenStorage
    {
        private const string AccessTokenKey = DrawerClaimTypes.AccessToken;

        private readonly ILocalStorage _localStorage;

        public TokenStorage(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SaveAccessTokenAsync(string accessToken)
        {
            await _localStorage.SetItemAsync(AccessTokenKey, accessToken);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            if (await _localStorage.ContainKeyAsync(AccessTokenKey))
                return await _localStorage.GetItemAsync(AccessTokenKey);
            else
                return null;
        }

        public async Task ClearAccessTokenAsync()
        {
            await _localStorage.RemoveItemAsync(AccessTokenKey);
        }


    }
}
