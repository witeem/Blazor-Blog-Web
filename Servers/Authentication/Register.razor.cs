using BlazorServerApp.Data.Blogger;
using BlazorServerApp.Global.Base.Enums;
using BlazorServerApp.HttpServers.Dtos;

namespace BlazorServerApp.Pages.Authentication;
public partial class Register
{
    private bool _show;
    private bool _isPass = true;
    private string _account = "";
    private string _passwd = "";
    private string _passwdVerify = "";
    private bool _loading = false;
    private string _verifyMsg = "账号密码错误，请重试";

    [Inject] public NavigationManager _navigation { get; set; }
    [Inject] public HttpClientHelper _httpClientHelper { get; set; }
    [Inject] public UserInfoServers _userInfoServers { get; set; }
    [Inject] public IStorageProvider _localStorage { get; set; }
    [Inject] public IOptionsMonitor<AppSetting> _appSetting { get; set; }

    // private Func<string, StringBoolean> _phoneRule = value => System.Text.RegularExpressions.Regex.Match(value, "^1[3456789]\\d{9}$").Success ? true : "Invalid Phone.";
    private Func<string, bool> accountVerify = value => string.IsNullOrEmpty(value) ? false : true;
    private Func<string, bool> passwdVerify = value => string.IsNullOrEmpty(value) ? false : true;

    private IEnumerable<Func<string, StringBoolean>> verifyRules => new List<Func<string, StringBoolean>>
    {

    };

    /// <summary>
    /// 注册
    /// </summary>
    /// <returns></returns>
    private async Task Login()
    {
        try
        {
            await Task.CompletedTask;
            if (!accountVerify(_account))
            {
                _isPass = false;
                _verifyMsg = "账号不能为空，请重试";
                return;
            }

            if (!passwdVerify(_passwd))
            {
                _isPass = false;
                _verifyMsg = "密码不能为空，请重试";
                return;
            }

            if (!_passwdVerify.Equals(_passwd))
            {
                _isPass = false;
                _verifyMsg = "密码匹对不正确，请重试";
                return;
            }

            ApiResponce<LoginResult> authInfo = await _userInfoServers.GetUserInfoAsync(_account, _passwd);
            if (authInfo.Code == (int)ApiResultEnum.Succeed)
            {

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
}
