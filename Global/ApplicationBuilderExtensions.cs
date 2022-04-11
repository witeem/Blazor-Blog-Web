namespace BlazorServerApp.Global;
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobal(this IApplicationBuilder app)
    {
        app.UseMiddleware<CookieMiddleware>();
        return app;
    }
}
