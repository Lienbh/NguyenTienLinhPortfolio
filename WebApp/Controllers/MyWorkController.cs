using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class MyWorkController : Controller
    {
        private readonly IConfiguration _configuration;

        public MyWorkController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            string requestURL = $"{apiUrl}/api/Categories";
            HttpClient client = new HttpClient();
            var response = client.GetFromJsonAsync<IEnumerable<CategoriesDTO>>(requestURL).Result;
            return View(response);
        }
        public IActionResult Manage(int categoriesId)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            string requestURL = $"{apiUrl}/api/Videos/get-video-by-categoryid/{categoriesId}";
            HttpClient client = new HttpClient();
            var response = client.GetFromJsonAsync<ManageVideoDTO>(requestURL).Result;
            return View(response);

        }
        [HttpPost]
        public IActionResult CreateVideo(VideoDTO videoDTO)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            string requestURL = $"{apiUrl}/api/Videos";
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync(requestURL, videoDTO).Result;

            string requestGetURL = $"{apiUrl}/api/Videos/get-video-by-categoryid/{videoDTO.IdCategories}";
            var responseGet = client.GetFromJsonAsync<ManageVideoDTO>(requestGetURL).Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Thêm mới video thất bại", SweetAlertMessageType.error);
                return View("Manage", responseGet);
            }
            ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Thêm mới video thành công", SweetAlertMessageType.success);
            return View("Manage", responseGet);
        }
        [HttpPost]
        public IActionResult UpdateVideo(VideoDTO videoDTO)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            string requestURL = $"{apiUrl}/api/Videos/{videoDTO.IdVideo}";
            HttpClient client = new HttpClient();
            var response = client.PutAsJsonAsync(requestURL, videoDTO).Result;



            string requestGetURL = $"{apiUrl}/api/Videos/get-video-by-categoryid/{videoDTO.IdCategories}";
            var responseGet = client.GetFromJsonAsync<ManageVideoDTO>(requestGetURL).Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Cập nhật video thất bại", SweetAlertMessageType.error);
                return View("Manage", responseGet);
            }
            ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Cập nhật video thành công", SweetAlertMessageType.success);
            return View("Manage", responseGet);
        }

        public IActionResult DeleteVideo(int id, int IdCategories)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            string requestURL = $"{apiUrl}/api/Videos/{id}";
            HttpClient client = new HttpClient();
            var response = client.DeleteAsync(requestURL).Result;

            string requestGetURL = $"{apiUrl}/api/Videos/get-video-by-categoryid/{IdCategories}";
            var responseGet = client.GetFromJsonAsync<ManageVideoDTO>(requestGetURL).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Xóa video thất bại", SweetAlertMessageType.error);
                return View("Manage", responseGet);
            }
            ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Xóa video thành công", SweetAlertMessageType.success);
            return View("Manage", responseGet);
        }

        public IActionResult RedirectMyWorkPartialView(int id)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            return Json(new { redirectToUrl = Url.Action("MyWorkPartialView", "MyWork") });
        }
        public IActionResult MyWorkPartialView(int id)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            string requestURL = $" {apiUrl}/api/Categories/get-detail/{id}";
            HttpClient client = new HttpClient();
            var response = client.GetFromJsonAsync<MyWorkPartialDTO>(requestURL).Result;


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

            var imgName = GenerateImgName(16) + ".jpeg";
            var HostPath = Path.Combine("wwwroot", "assets", "projects", imgName);
            //Xây dựng 1 đường dẫn để lưu ảnh trong thư mục wwwroot
            string path = Path.Combine(Directory.GetCurrentDirectory(), HostPath);
            //Kết quả thu được có dạng như sau: wwwroot/img/concho.pngs
            //Thực hiện việc sao chép fike được chọn vào thư mục root

            using (var stream = new FileStream(path, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            return Json(new { imgNew = imgName });
        }
    }
}
