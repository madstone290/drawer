using Blazored.LocalStorage;
using Drawer.Shared;

namespace Drawer.Web.Authentication
{
    public class TokenStorage : ITokenStorage
    {
        private const string AccessTokenKey = DrawerClaimTypes.AccessToken;

        private readonly ILocalStorageService _localStorageService;

        public TokenStorage(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task SaveAccessTokenAsync(string accessToken)
        {
            await _localStorageService.SetItemAsync(AccessTokenKey, accessToken);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            if (await _localStorageService.ContainKeyAsync(AccessTokenKey))
                return await _localStorageService.GetItemAsync<string>(AccessTokenKey);
            else
                return null;
        }

        public async Task ClearAccessTokenAsync()
        {
            await _localStorageService.RemoveItemAsync(AccessTokenKey);
        }


    }
}
