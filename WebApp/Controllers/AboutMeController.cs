using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AboutMeController : Controller
    {
        public IActionResult Index()
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            string requestURL = $"https://localhost:7130/api/About/AboutProfile";
            // Bước 2 Lấy response (body) 
            HttpClient client = new HttpClient();
            var response = client.GetFromJsonAsync<AboutDTO>(requestURL).Result;
            return View(response);
        }




        public IActionResult Edit(string id)
        {
            string requestURL = $"https://localhost:7130/api/About/{id}";
            // Bước 2 Lấy response (body) 
            HttpClient client = new HttpClient();
            var response = client.GetFromJsonAsync<AboutDTO>(requestURL).Result;
            return View(response);
        }
        private string GenerateImgName(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
        public IActionResult HandleFileChange(IFormFile photo)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }


            var imgName = GenerateImgName(16) + ".jpeg";
            var HostPath = Path.Combine("wwwroot", "assets", "ntlinh", imgName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), HostPath);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            return Json(new { imgNew = imgName });
        }


        [HttpPost]
        public IActionResult UpdateAbout(AboutDTO about)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }

            string requestURL = $"https://localhost:7130/api/About/{about.IdAbout}";

            HttpClient client = new HttpClient();
            var response = client.PutAsJsonAsync(requestURL, about).Result;
            TempData["responseMessage"] = "Chỉnh sửa thông tin cá nhân thành công ";
            return RedirectToAction("Index");
        }
    }
}
