namespace BlazorServerApp.Shared;
public partial class Search
{
    [Parameter]
    public string SearchContent { get; set; }

    List<NavModel> GetNavs(string search)
    {
        var output = new List<NavModel>();
        if (search is not null && search != "")
        {
            output.AddRange(NavHelper.SameLevelNavs.Where(n => n.Href is not null && GetI18nFullTitle(n.FullTitle).Contains(search, StringComparison.OrdinalIgnoreCase)));
        }
        return output;
    }

    string GetI18nFullTitle(string fullTitle)
    {
        var arr = fullTitle.Split(' ').ToList();
        if (arr.Count == 1) return T(fullTitle);
        else
        {
            var parent = arr[0];
            arr.RemoveAt(0);
            return $"{T(parent)} {T(string.Join(' ', arr))}";
        }
    }
}
