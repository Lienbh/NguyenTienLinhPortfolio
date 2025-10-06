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

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "gallery-index",
    pattern: "Photo",
    defaults: new { controller = "Photo", action = "Index" });

app.MapControllerRoute(
    name: "gallery-slug",
    pattern: "Photo/{slug}",
    defaults: new { controller = "Photo", action = "View" });

app.MapControllerRoute(
    name: "gallery-load",
    pattern: "Photo/LoadGalleries",
    defaults: new { controller = "Photo", action = "LoadGalleries" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
