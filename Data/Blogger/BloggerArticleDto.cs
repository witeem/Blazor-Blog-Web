namespace BlazorServerApp.Data.Blogger
{
    public struct BloggerArticleDto
    {
        public long Id { get; set; }

        public long BloggerId { get; set; }

        public string ArticleTitle { get; set; }

        public string Introduction { get; set; }

        public string Article { get; set; }
    }
}
