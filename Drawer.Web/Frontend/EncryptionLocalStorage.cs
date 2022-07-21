using Blazored.LocalStorage;
using Drawer.Web.Encryption;

namespace Drawer.Web.Frontend
{
    public class EncryptionLocalStorage : ILocalStorage
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IEncryptionService _encryptionService;


        public EncryptionLocalStorage(ILocalStorageService localStorageService, IEncryptionService encryptionService)
        {
            _localStorageService = localStorageService;
            _encryptionService = encryptionService;
        }

        public ValueTask<bool> ContainKeyAsync(string key, CancellationToken? cancellationToken = null)
        {
            return _localStorageService.ContainKeyAsync(key, cancellationToken);
        }

        public async ValueTask<string?> GetItemAsync(string key, CancellationToken? cancellationToken = null)
        {
            var chiper = await _localStorageService.GetItemAsync<string>(key, cancellationToken);
            return _encryptionService.Decrypt(chiper);
        }

        public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
        {
            return _localStorageService.RemoveItemAsync(key, cancellationToken);
        }

        public ValueTask SetItemAsync(string key, string item, CancellationToken? cancellationToken = null)
        {
            var chiper = _encryptionService.Encrypt(item);
            return _localStorageService.SetItemAsync(key, chiper, cancellationToken);
        }
    }
}
