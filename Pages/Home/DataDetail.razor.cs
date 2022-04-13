using BlazorServerApp.Data.Blogger;
using BlazorServerApp.HttpServers.Dtos;
using Microsoft.JSInterop;
using Markdig;

namespace BlazorServerApp.Pages.Home;
 public partial class DataDetail
{
    private string markdownValue = "";
    private MarkupString _postHtmlContent;
    public bool _hasXss { get; set; }

    [Parameter]
    public string Id { get; set; }

    [Inject] public HttpClientHelper _httpClientHelper { get; set; }
    [Inject] public IOptionsMonitor<AppSetting> _appSetting { get; set; }
    [Inject] public IJSRuntime _jsRuntime { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
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
                var ipAddress = await _jsRuntime.InvokeAsync<string>("getIpAddress");
                var datas = await GetBloggerArticleAsync(Id);
                if (datas.Successed)
                {
                    HtmlToMarkdown(datas);
                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        _ = await AddViews(Id, ipAddress);
                    }
                }

                StateHasChanged();
            }

            await _jsRuntime.InvokeVoidAsync("Prism.highlightAll");
        }
        catch (Exception ex)
        {
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    /// 文章内容转成MarkDown
    /// </summary>
    /// <param name="datas"></param>
    private void HtmlToMarkdown(ApiResponce<BloggerArticleDto> datas)
    {
        markdownValue = datas.Data.Article;
        var htmlData = Markdown.ToHtml(markdownValue ?? string.Empty);
        // 转为 prism 支持的语言标记（不是必须，可以删除）
        htmlData = htmlData.Replace("language-golang", "language-go");

        // TODO: 使用 https://github.com/mganss/HtmlSanitizer 清洗html中的xss
        if (htmlData.Contains("<script") || htmlData.Contains("<link"))
        {
            _hasXss = true;
        }

        _postHtmlContent = (MarkupString)htmlData;
    }

    /// <summary>
    /// 获取文章详情
    /// </summary>
    private async Task<ApiResponce<BloggerArticleDto>> GetBloggerArticleAsync(string Id)
    {
#if DEBUG
        _httpClientHelper.BaseUri = _appSetting.CurrentValue.ApiUri;
#else
                _httpClientHelper.BaseUri = _appSetting.CurrentValue.GatewayUri;
#endif
        Dictionary<string, string> reqParams = new Dictionary<string, string>();
        reqParams.Add("Type", "1");
        reqParams.Add("Id", Id);
        var bloggerInfo = await _httpClientHelper.GetAsync<BloggerArticleDto>("/api/AdverUser/GetArticleDetails", reqParams);
        return bloggerInfo;
    }

    /// <summary>
    /// 新增文章浏览数量
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Ip"></param>
    private async Task<bool> AddViews(string Id, string Ip)
    {
#if DEBUG
        _httpClientHelper.BaseUri = _appSetting.CurrentValue.ApiUri;
#else
                _httpClientHelper.BaseUri = _appSetting.CurrentValue.GatewayUri;
#endif
        Dictionary<string, string> reqParams = new Dictionary<string, string>();
        reqParams.Add("Id", Id);
        reqParams.Add("IP", Ip);
        var responce = await _httpClientHelper.GetAsync<bool>("/api/Article/ArticleAddViews", reqParams);
        return responce.Data;
    }
}
