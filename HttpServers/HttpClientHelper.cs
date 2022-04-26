using Blazored.LocalStorage;
using BlazorServerApp.Global.Base.Enums;
using BlazorServerApp.HttpServers.Dtos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace BlazorServerApp.HttpServers;

public class HttpClientHelper
{
    private readonly ILogger<HttpClientHelper> _logger;
    private readonly AppSetting _appSetting;
    private readonly CookiesStorageService _cookiesStorage;
    private readonly ILocalStorageService _localStorage;

    private string _baseUri = string.Empty;
    private bool _isSign = true;
    private bool _withAccToken = false;
    public RestClient _client = null;

    public HttpClientHelper(ILogger<HttpClientHelper> logger, IOptionsMonitor<AppSetting> appsetting, CookiesStorageService cookiesStorage, ILocalStorageService localStorage)
    {
        _logger = logger;
        _appSetting = appsetting.CurrentValue;
        _cookiesStorage = cookiesStorage;
        _localStorage = localStorage;
    }

    public RestClient CreateClient()
    {
        _client = new RestClient(BaseUri);
        return _client;
    }

    public string BaseUri
    {
        get { return _baseUri; }
        set { _baseUri = value; }
    }

    public bool IsSign
    {
        get { return _isSign; }
        set { _isSign = value; }
    }

    public bool WithAccToken
    {
        get { return _withAccToken; }
        set { _withAccToken = value; }
    }

    public async Task<ApiResponce<T>> GetAsync<T>(string serverMethod, Dictionary<string, string> queryParams, int timeoutSecond = 180, CancellationToken cancellationToken = default)
    {
        return await GetAsync<T>(serverMethod, queryParams, null, timeoutSecond, cancellationToken);
    }

    public async Task<ApiResponce<T>> GetAsync<T>(string serverMethod, Dictionary<string, string> queryParams, ICollection<KeyValuePair<string, string>> headerParams, int timeoutSecond = 180, CancellationToken cancellationToken = default)
    {
        try
        {
            _client = CreateClient();
            var request = new RestRequest(serverMethod);
            if (_isSign) queryParams = CreateSign<string>(queryParams, null);
            if (_withAccToken) await AddAccToken(request);

            if (queryParams != null)
            {
                foreach (var item in queryParams)
                {
                    request.AddQueryParameter(item.Key, item.Value);
                }
            }


            if (headerParams != null)
            {
                request.AddOrUpdateHeaders(headerParams);
            }

            request.Timeout = timeoutSecond * 1000;
            var response = await _client.GetAsync<ApiResponce<T>>(request, cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"HttpGet:{serverMethod} Error:{ex.ToString()}");
            return ApiResponce<T>.Fail((int)ApiResultEnum.Error, ex.Message);
        }
    }

    public async Task<ApiResponce<T>> PostAsync<T, N>(string serverMethod, N requestBody, int timeoutSecond = 180, CancellationToken cancellationToken = default)
    {
        return await PostAsync<T, N>(serverMethod, requestBody, null, null, timeoutSecond, cancellationToken);
    }

    public async Task<ApiResponce<T>> PostAsync<T, N>(string serverMethod, N requestBody, Dictionary<string, string> queryParams, int timeoutSecond = 180, CancellationToken cancellationToken = default)
    {
        return await PostAsync<T, N>(serverMethod, requestBody, queryParams, null, timeoutSecond, cancellationToken);
    }

    public async Task<ApiResponce<T>> PostAsync<T, N>(string serverMethod, N requestBody, Dictionary<string, string> queryParams, ICollection<KeyValuePair<string, string>> headerParams, int timeoutSecond = 180, CancellationToken cancellationToken = default)
    {
        try
        {
            _client = CreateClient();
            var request = new RestRequest(serverMethod);
            request.Timeout = timeoutSecond * 1000;
            if (_isSign) queryParams = CreateSign(queryParams, requestBody);
            if (_withAccToken) await AddAccToken(request);

            if (queryParams != null)
            {
                foreach (var item in queryParams)
                {
                    request.AddQueryParameter(item.Key, item.Value);
                }
            }


            if (headerParams != null)
            {
                request.AddOrUpdateHeaders(headerParams);
            }

            request.AddStringBody(JsonConvert.SerializeObject(requestBody), "application/json");
            var response = await _client.PostAsync<ApiResponce<T>>(request, cancellationToken);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"HttpGet:{serverMethod} Error:{ex.ToString()}");
            return ApiResponce<T>.Fail((int)ApiResultEnum.Error, ex.Message);
        }
    }

    private Dictionary<string, string> CreateSign<T>(Dictionary<string, string> queryParams, T requestBody)
    {
        DateTimeOffset offTime = new DateTimeOffset(DateTime.Now);
        string timeSpan = offTime.ToUnixTimeMilliseconds().ToString();
        string nonce = Guid.NewGuid().ToString();
        string secretKey = string.Empty;
        if (requestBody == null)
            secretKey = GetSecretKey(queryParams);
        else
            secretKey = GetSecretKey(requestBody);

        string sign = EncyptHelper.MD5(secretKey + timeSpan + nonce);
        if (queryParams == null)
            queryParams = new Dictionary<string, string>();
        queryParams.Add("timespan", timeSpan);
        queryParams.Add("nonce", nonce);
        queryParams.Add("sign", sign);
        return queryParams;
    }

    private async Task AddAccToken(RestRequest restRequest)
    {

        // var accToken = await _localStorage.GetItemAsync<string>(GlobalConfig.TokenKey);
        var accToken = await _cookiesStorage.GetItemAsync(GlobalConfig.TokenKey);
        if (!string.IsNullOrEmpty(accToken))
        {
            restRequest.AddOrUpdateHeader("X-Token", accToken);
        }

        await Task.CompletedTask;
    }

    private string GetSecretKey(Dictionary<string, string> queryParams)
    {
        if (queryParams != null)
        {
            return EncyptHelper.AESEncrypt(JsonConvert.SerializeObject(queryParams), _appSetting.ConnKey, _appSetting.ConnIV);
        }
        else
        {
            return EncyptHelper.AESEncrypt("{}", _appSetting.ConnKey, _appSetting.ConnIV);
        }
    }

    private string GetSecretKey<T>(T t)
    {
        if (t != null)
        {
            return EncyptHelper.AESEncrypt(JsonConvert.SerializeObject(t), _appSetting.ConnKey, _appSetting.ConnIV);
        }
        else
        {
            return EncyptHelper.AESEncrypt("{}", _appSetting.ConnKey, _appSetting.ConnIV);
        }
    }

    private string GetSecretKey(string requestBody)
    {
        if (!string.IsNullOrEmpty(requestBody))
        {
            return EncyptHelper.AESEncrypt(requestBody, _appSetting.ConnKey, _appSetting.ConnIV);
        }
        else
        {
            return EncyptHelper.AESEncrypt("{}", _appSetting.ConnKey, _appSetting.ConnIV);
        }
    }
}
