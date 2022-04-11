using Newtonsoft.Json;

namespace BlazorServerApp.Global.Config
{
    public class CookiesStorageService
    {
        private readonly IStorageProvider _storageProvider;

        public CookiesStorageService(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public async ValueTask<string> GetItemAsync(string key, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return await _storageProvider.GetCookiesItemAsync(key, cancellationToken);
        }

        public ValueTask RemoveItemAsync(string key, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return _storageProvider.RemoveCookiesItemAsync(key, cancellationToken);
        }

        public async ValueTask SetItemAsync(string key, string data, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            await _storageProvider.SetCookiesItemAsync(key, data, cancellationToken).ConfigureAwait(false);
        }
    }
}
