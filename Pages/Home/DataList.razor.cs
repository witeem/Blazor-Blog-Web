namespace BlazorServerApp.Pages.Home;
using BlazorServerApp.Data.Blogger;
using BlazorServerApp.HttpServers.Dtos;

public partial class DataList
{
    [Inject] public HttpClientHelper _httpClientHelper { get; set; }
    [Inject] public IOptionsMonitor<AppSetting> _appSetting { get; set; }

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
                var datas = await GetBloggerArticlesAsync();
                if (datas.Successed)
                {
                    foreach (var item in datas.Data)
                    {
                        _items.Add(new DataDemo { 
                            Id = item.Id,
                            Title = item.ArticleTitle,
                            SubTtile = item.Introduction,
                            Avatar = "https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fpic.51yuansu.com%2Fpic2%2Fcover%2F00%2F32%2F66%2F5810fec833d03_610.jpg&refer=http%3A%2F%2Fpic.51yuansu.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=auto?sec=1652262505&t=b71a8a1c448b8149147f788ff6e2ad6e"
                        });
                    }
                }
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private class DataDemo
    {
        public long Id { get; set; }
        public string Header { get; set; }
        public string Avatar { get; set; }
        public string Title { get; set; }
        public string SubTtile { get; set; }
        public bool Divider { get; set; }
        public bool Inset { get; set; }
    }

    private List<DataDemo> _items = new List<DataDemo>();

    /// <summary>
    /// 获取博主文章列表
    /// </summary>
    /// <returns></returns>
    private async Task<ApiResponce<List<BloggerArticleDto>>> GetBloggerArticlesAsync()
    {
#if DEBUG
        _httpClientHelper.BaseUri = _appSetting.CurrentValue.ApiUri;
#else
                _httpClientHelper.BaseUri = _appSetting.CurrentValue.GatewayUri;
#endif
        _httpClientHelper.WithAccToken = true;
        Dictionary<string, string> reqParams = new Dictionary<string, string>();
        reqParams.Add("Type", "1");
        var bloggerInfo = await _httpClientHelper.GetAsync<List<BloggerArticleDto>>("/api/AdverUser/GetBloggerArticles", reqParams);
        return bloggerInfo;
    }
}
