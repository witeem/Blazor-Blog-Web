using Blazored.LocalStorage;
using BlazorServerApp.Global.Base;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMasaBlazor(builder =>
{
    builder.UseTheme(option =>
    {
        option.Primary = "#4318FF";
        option.Accent = "#4318FF";
    }
    );
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.Configure<AppSetting>(builder.Configuration.GetSection(BaseContst.AppSetting));
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddGlobalForServer();
builder.Services.AddCors(x => x.AddPolicy("externalRequests", policy => policy.WithOrigins("https://jsonip.com")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseCors("externalRequests");
app.UseRouting();
app.UseGlobal();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
