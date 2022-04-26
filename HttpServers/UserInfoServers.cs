using BlazorServerApp.Data.Blogger;
using BlazorServerApp.Global.Base.Enums;
using BlazorServerApp.HttpServers.Dtos;
using Newtonsoft.Json;

namespace BlazorServerApp.HttpServers;
public class UserInfoServers
{
    private readonly ILogger<UserInfoServers> _logger;
    private readonly AppSetting _appSetting;
    private readonly HttpClientHelper _httpClientHelper;

    public UserInfoServers(ILogger<UserInfoServers> logger, IOptionsMonitor<AppSetting> appsetting, HttpClientHelper httpClientHelper)
    {
        _logger = logger;
        _appSetting = appsetting.CurrentValue;
        _httpClientHelper = httpClientHelper;
    }

    /// <summary>
    /// 获取登录Token
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResponce<LoginResult>> GetAuthToken(string account, string passwd)
    {
        // 网关地址，请求时 Kong API 网关会根据路由匹配转发到对应的应用服务上 
        _httpClientHelper.BaseUri = _appSetting.GatewayUri;
        Dictionary<string, string> reqParams = new Dictionary<string, string>();
        reqParams.Add("UserName", account);
        reqParams.Add("Password", passwd);
        var authInfo = await _httpClientHelper.GetAsync<LoginResult>("/api/oAuth/Login", reqParams);
        return authInfo;
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResponce<LoginResult>> GetUserInfoAsync(string account, string passwd)
    {
        try
        {
            // 网关地址，请求时 Kong API 网关会根据路由匹配转发到对应的应用服务上 
            _httpClientHelper.BaseUri = _appSetting.GatewayUri;
            _httpClientHelper.WithAccToken = true;
            AdverUserInfoDto userInfoDto = new AdverUserInfoDto()
            {
                Name = account,
                NickName = account,
                Password = passwd,
                Phone = passwd,
                BirthDay = new DateTime(1999, 1, 1),
                Age = 1,
                Sex = 1,
                RoleCodes = "10003",
                Picture = "https://gimg2.baidu.com/image_search/src=http%3A%2F%2Finews.gtimg.com%2Fnewsapp_bt%2F0%2F14165370162%2F1000&refer=http%3A%2F%2Finews.gtimg.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=auto?sec=1653370854&t=db13fd7e61f120642e912ff15d52ee69"
            };

            var authInfo = await _httpClientHelper.PostAsync<AdverUserInfoDto, AdverUserInfoDto>("/api/oAuth/AddUserInfo", userInfoDto);
            if (authInfo.Successed == true)
            {
                var tokenResult = await GetAuthToken(account, passwd);
                return tokenResult;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"HttpGet:{GetUserInfoAsync} Error:{ex.ToString()}");
            return ApiResponce<LoginResult>.Fail((int)ApiResultEnum.Error, ex.Message);
        }

        return ApiResponce<LoginResult>.Fail((int)ApiResultEnum.Error, "注册失败");
    }
}
