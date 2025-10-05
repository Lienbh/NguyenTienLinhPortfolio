using System.Net;

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
