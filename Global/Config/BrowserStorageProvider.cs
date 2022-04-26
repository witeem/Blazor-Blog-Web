using Microsoft.JSInterop;
namespace BlazorServerApp.Global.Config;
public class BrowserStorageProvider : IStorageProvider
{
    private readonly IJSRuntime _jSRuntime;

    public BrowserStorageProvider(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }

    /// <summary>
    /// 根据Key值获取Cookie
    /// </summary>
    /// <param name="key">key</param>
    public async ValueTask<string> GetCookiesItemAsync(string key, CancellationToken? cancellationToken = null)
           => await _jSRuntime.InvokeAsync<string>("Cookies.get", cancellationToken ?? CancellationToken.None, key);

    /// <summary>
    /// 根据Key值移除Cookie
    /// </summary>
    /// <param name="key">key</param>
    public ValueTask RemoveCookiesItemAsync(string key, CancellationToken? cancellationToken = null)
    => _jSRuntime.InvokeVoidAsync("Cookies.expire", cancellationToken ?? CancellationToken.None, key);

    /// <summary>
    /// 设置Cookie
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="data">data</param>
    public ValueTask SetCookiesItemAsync(string key, string data, CancellationToken? cancellationToken = null)
        => _jSRuntime.InvokeVoidAsync("Cookies.set", cancellationToken ?? CancellationToken.None, key, data);

    /// <summary>
    /// 设置Cookie在某域名下
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="data">data</param>
    /// <param name="domain">域名</param>
    /// <param name="secure">secure</param>
    public ValueTask SetCookiesItemAsync(string key, string data, string domain, bool secure, CancellationToken? cancellationToken = null)
    => _jSRuntime.InvokeVoidAsync("Cookies.set", cancellationToken ?? CancellationToken.None, key, data, new { domain, secure });

    public ValueTask SetCookiesItemAsync(string key, string data, TimeSpan exprires, CancellationToken? cancellationToken = null)
    => _jSRuntime.InvokeVoidAsync("Cookies.set", cancellationToken ?? CancellationToken.None, key, data, new { exprires });

    public ValueTask SetCookiesItemAsync(string key, string data, DateTime exprires, CancellationToken? cancellationToken = null)
    => _jSRuntime.InvokeVoidAsync("Cookies.set", cancellationToken ?? CancellationToken.None, key, data, new { exprires });
}
