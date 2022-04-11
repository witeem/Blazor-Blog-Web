using Blazored.LocalStorage;
using BlazorServerApp.HttpServers.Dtos;
using Microsoft.Extensions.Options;

namespace BlazorServerApp.Pages.Authentication;

public partial class Login
{
    private bool _show;
    private bool _isPass = true;
    private string _account = "";
    private string _passwd = "";
    private bool _loading = false;
    private string _verifyMsg = "账号密码错误，请重试";

    [Inject] public NavigationManager _navigation { get; set; }
    [Inject] public HttpClientHelper _httpClientHelper { get; set; }
    [Inject] public IStorageProvider _localStorage { get; set; }
    [Inject] public IOptionsMonitor<AppSetting> _appSetting { get; set; }

    /// <summary>
    /// 判断传入的值是否为空
    /// </summary>
    private Func<string, StringBoolean> _requiredRule = value => !string.IsNullOrEmpty(value) ? true : "Required.";
    // private Func<string, StringBoolean> _phoneRule = value => System.Text.RegularExpressions.Regex.Match(value, "^1[3456789]\\d{9}$").Success ? true : "Invalid Phone.";

    private IEnumerable<Func<string, StringBoolean>> _accountRules => new List<Func<string, StringBoolean>>
    {
        _requiredRule
    };

    private IEnumerable<Func<string, StringBoolean>> _passwdRules => new List<Func<string, StringBoolean>>
    {
        _requiredRule
    };


    /// <summary>
    /// 执行登录
    /// </summary>
    /// <returns></returns>
    private async Task OnLogin()
    {
        try
        {
            if (_requiredRule(_account) != true)
            {
                _isPass = false;
                _verifyMsg = "账号密码错误，请重试";
                return;
            }
            if (_requiredRule(_passwd) != true)
            {
                _isPass = false;
                _verifyMsg = "账号密码错误，请重试";
                return;
            }
            ApiResponce<LoginResult> authInfo = await GetAuthToken();
            if (authInfo.Code == 200 && authInfo.Data.success)
            {
                // 存Cookie
#if DEBUG
                await _localStorage.SetCookiesItemAsync(GlobalConfig.TokenKey, authInfo.Data.access_token, default);
#else
                await _localStorage.SetCookiesItemAsync(GlobalConfig.TokenKey, authInfo.Data.access_token, ".witeemv.cn", true, default);
#endif

                // 存浏览器本地缓存
                // await _localStorage.SetItemAsStringAsync(GlobalConfig.TokenKey, authInfo.Data.access_token);
                _navigation.NavigateTo($"{_navigation.BaseUri}"); // 跳转到首页，可以优化成根据返回的地址路径进行跳转
            }
            else
            {
                _isPass = false;
                _verifyMsg = "账号密码错误，请重试";
                return;
            }
        }
        catch (Exception ex)
        {
            _isPass = false;
            _verifyMsg = "账号密码错误，请重试";
            return;
        }
    }

    /// <summary>
    /// 获取登录Token
    /// </summary>
    /// <returns></returns>
    private async Task<ApiResponce<LoginResult>> GetAuthToken()
    {
        // 网关地址，请求时 Kong API 网关会根据路由匹配转发到对应的应用服务上 
        _httpClientHelper.BaseUri = _appSetting.CurrentValue.GatewayUri;
        Dictionary<string, string> reqParams = new Dictionary<string, string>();
        reqParams.Add("UserName", _account);
        reqParams.Add("Password", _passwd);
        var authInfo = await _httpClientHelper.GetAsync<LoginResult>("/api/oAuth/Login", reqParams);
        return authInfo;
    }
}
