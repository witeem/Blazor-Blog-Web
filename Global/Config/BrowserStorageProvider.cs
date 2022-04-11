using Microsoft.JSInterop;
namespace BlazorServerApp.Global.Config;
public class BrowserStorageProvider : IStorageProvider
{
    private readonly IJSRuntime _jSRuntime;
    private readonly IJSInProcessRuntime _jSInProcessRuntime;

    public BrowserStorageProvider(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
        _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
    }

    public async ValueTask<string> GetCookiesItemAsync(string key, CancellationToken? cancellationToken = null)
           => await _jSRuntime.InvokeAsync<string>("Cookies.get", cancellationToken ?? CancellationToken.None, key);

    public ValueTask RemoveCookiesItemAsync(string key, CancellationToken? cancellationToken = null)
    => _jSRuntime.InvokeVoidAsync("Cookies.expire", cancellationToken ?? CancellationToken.None, key);

    public ValueTask SetCookiesItemAsync(string key, string data, CancellationToken? cancellationToken = null)
        => _jSRuntime.InvokeVoidAsync("Cookies.set", cancellationToken ?? CancellationToken.None, key, data);

    public ValueTask SetCookiesItemAsync(string key, string data, string domain, bool secure, CancellationToken? cancellationToken = null)
    => _jSRuntime.InvokeVoidAsync("Cookies.set", cancellationToken ?? CancellationToken.None, key, data, new { domain, secure });

    public ValueTask SetCookiesItemAsync(string key, string data, TimeSpan exprires, CancellationToken? cancellationToken = null)
    => _jSRuntime.InvokeVoidAsync("Cookies.set", cancellationToken ?? CancellationToken.None, key, data, new { exprires });

    public ValueTask SetCookiesItemAsync(string key, string data, DateTime exprires, CancellationToken? cancellationToken = null)
    => _jSRuntime.InvokeVoidAsync("Cookies.set", cancellationToken ?? CancellationToken.None, key, data, new { exprires });

}
