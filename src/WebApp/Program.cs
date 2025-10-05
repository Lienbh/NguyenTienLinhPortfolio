using System.Net;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromSeconds(864000);
});
var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine("/root/ntl-sln/NguyenTienLinhPortfolio/src/NguyenTienLinh/wwwroot/assets")
    ),
    RequestPath = "/gallery-images",

    // Tùy chọn header, cache, security
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=2592000"; // 30 ngày
    }
});
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "gallery-index",
    pattern: "Gallery",
    defaults: new { controller = "Gallery", action = "Index" });

app.MapControllerRoute(
    name: "gallery-slug",
    pattern: "Gallery/{slug}",
    defaults: new { controller = "Gallery", action = "View" });

app.MapControllerRoute(
    name: "gallery-load",
    pattern: "Gallery/LoadGalleries",
    defaults: new { controller = "Gallery", action = "LoadGalleries" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
