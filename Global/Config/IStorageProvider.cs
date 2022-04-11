namespace BlazorServerApp.Global.Config;
public interface IStorageProvider
{
    ValueTask<string> GetCookiesItemAsync(string key, CancellationToken? cancellationToken = null);

    ValueTask RemoveCookiesItemAsync(string key, CancellationToken? cancellationToken = null);

    ValueTask SetCookiesItemAsync(string key, string data, CancellationToken? cancellationToken = null);

    ValueTask SetCookiesItemAsync(string key, string data, string domain, bool secure, CancellationToken? cancellationToken = null);

    ValueTask SetCookiesItemAsync(string key, string data, TimeSpan exprires, CancellationToken? cancellationToken = null);

    ValueTask SetCookiesItemAsync(string key, string data, DateTime exprires, CancellationToken? cancellationToken = null);
}