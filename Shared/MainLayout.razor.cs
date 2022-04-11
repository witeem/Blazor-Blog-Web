using Blazored.LocalStorage;
using BlazorServerApp.Data.Blogger;
using BlazorServerApp.HttpServers.Dtos;

namespace BlazorServerApp.Shared;
public partial class MainLayout
{
    private string _searchContent;
    private bool _isLogin = false;
    private string _localToken;
    private BloggerInfoDto _bloggerInfo;
    private AdverUserInfoDto _userInfo;

    [Inject] public NavigationManager _navigation { get; set; }

    [Inject] public HttpClientHelper _httpClientHelper { get; set; }

    [Inject] public IOptionsMonitor<AppSetting> _appSetting { get; set; }

    [Inject] public IStorageProvider _localStorage { get; set; }

    public class ItemDemo
    {
        public string Title { get; set; }
        public string Icon { get; set; }
    }

    /// <summary>
    /// 初始化组件
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// 设置参数之后
    /// </summary>
    /// <returns></returns>
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
    }

    /// <summary>
    /// 组件呈现之后
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            if (firstRender)
            {
                _localToken = await _localStorage.GetCookiesItemAsync(GlobalConfig.TokenKey);
                ApiResponce<BloggerInfoDto> bloggerInfo = await GetBloggerInfoAsync();
                if (bloggerInfo.Successed)
                    _bloggerInfo = bloggerInfo.Data;

                if (!string.IsNullOrEmpty(_localToken))
                {
                    ApiResponce<AdverUserInfoDto> userInfo = await GetUserInfoAsync();
                    if (userInfo.Successed)
                    {
                        _isLogin = true;
                        _userInfo = userInfo.Data;
                    }
                    else
                        _isLogin = false;
                }

                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            _isLogin = false;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public List<ItemDemo> GetMenus()
    {
        List<ItemDemo> items = new List<ItemDemo>
        {
            new ItemDemo { Title = "Home", Icon = "mdi-view-dashboard" },
            new ItemDemo { Title = "About", Icon = "mdi-forum" }
        };

        return items;
    }

    public async Task SearchBlogArticle()
    {
        _searchContent = "这里可以搜索";
        await Task.CompletedTask;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <returns></returns>
    public async Task Login()
    {
        await _localStorage.RemoveCookiesItemAsync(GlobalConfig.TokenKey);
        _navigation.NavigateTo($"{_navigation.BaseUri}Login");
    }

    /// <summary>
    /// 退出
    /// </summary>
    /// <returns></returns>
    public async Task Logout()
    {
        try
        {
            var localToken = await _localStorage.GetCookiesItemAsync(GlobalConfig.TokenKey);
            if (!string.IsNullOrEmpty(localToken))
            {
                await ClearLoginAsync();
                _isLogin = true;
            }

            _navigation.NavigateTo($"{_navigation.BaseUri}Login");
        }
        catch (Exception ex)
        {
            _navigation.NavigateTo($"{_navigation.BaseUri}Login");
        }
    }


    /// <summary>
    /// 清除Redis登录状态
    /// </summary>
    /// <returns></returns>
    private async Task<ApiResponce<bool>> ClearLoginAsync()
    {
        // 网关地址，请求时 Kong API 网关会根据路由匹配转发到对应的应用服务上 
        _httpClientHelper.BaseUri = _appSetting.CurrentValue.GatewayUri;
        _httpClientHelper.WithAccToken = true;
        Dictionary<string, string> reqParams = new Dictionary<string, string>();
        var authInfo = await _httpClientHelper.GetAsync<bool>("/api/oAuth/LogOut", reqParams);
        if (authInfo.Data)
        {
            await _localStorage.RemoveCookiesItemAsync(GlobalConfig.TokenKey);
        }

        return authInfo;
    }

    /// <summary>
    /// 获取博主信息
    /// </summary>
    /// <returns></returns>
    private async Task<ApiResponce<BloggerInfoDto>> GetBloggerInfoAsync()
    {
#if DEBUG
        _httpClientHelper.BaseUri = _appSetting.CurrentValue.ApiUri;
#else
                _httpClientHelper.BaseUri = _appSetting.CurrentValue.GatewayUri;
#endif
        _httpClientHelper.WithAccToken = true;
        Dictionary<string, string> reqParams = new Dictionary<string, string>();
        reqParams.Add("Type", "1");
        var bloggerInfo = await _httpClientHelper.GetAsync<BloggerInfoDto>("/api/AdverUser/GetBloggerInfo", reqParams);
        return bloggerInfo;
    }

    /// <summary>
    /// 获取登录用户信息
    /// </summary>
    /// <returns></returns>
    private async Task<ApiResponce<AdverUserInfoDto>> GetUserInfoAsync()
    {
        _httpClientHelper.BaseUri = _appSetting.CurrentValue.GatewayUri;
        _httpClientHelper.WithAccToken = true;
        Dictionary<string, string> reqParams = new Dictionary<string, string>();
        var bloggerInfo = await _httpClientHelper.GetAsync<AdverUserInfoDto>("/api/oAuth/GetAuthenticate", reqParams);
        return bloggerInfo;
    }
}
