using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class MyWorkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
