using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
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

            HttpClient client = new HttpClient();
            string requestBackGroundURL = $"{apiUrl}/api/BackGroud";
            string requestCategoriesURL = $"{apiUrl}/api/Categories";

            var responseBackGround = client.GetFromJsonAsync<List<BackGroudDTO>>(requestBackGroundURL).Result;
            var responseCategories = client.GetFromJsonAsync<List<CategoriesDTO>>(requestCategoriesURL).Result;

            HomeDTO homeDTO = new HomeDTO();
            homeDTO.BackGroudDTOs = responseBackGround;
            homeDTO.CategoriesDTOs = responseCategories;
            return View(homeDTO);
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
            else
            {
                return RedirectToAction("Index", "Home");
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
        [HttpPost]
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

        public IActionResult Manage()
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

            string requestURL = $"{apiUrl}/api/BackGroud";
            HttpClient client = new HttpClient();
            var reponse = client.GetFromJsonAsync<List<BackGroudDTO>>(requestURL).Result;
            return View(reponse);

        }
        [HttpPost]
        public IActionResult CreateBackground(BackGroudDTO backGroudDTO)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            var apiUrl = _configuration["AppSettings:ApiUrl"];

            string requestURL = $"{apiUrl}/api/BackGroud";
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync(requestURL, backGroudDTO).Result;



            string requestGetURL = $"{apiUrl}/api/BackGroud";
            List<BackGroudDTO> backGroudDTOs = new List<BackGroudDTO>();
            backGroudDTOs = client.GetFromJsonAsync<List<BackGroudDTO>>(requestGetURL).Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Thêm mới ảnh background thất bại", SweetAlertMessageType.error);
                return View("Manage", backGroudDTOs);
            }
            ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Thêm mới ảnh background thành công", SweetAlertMessageType.success);
            return View("Manage", backGroudDTOs);
        }
        [HttpPost]
        public IActionResult UpdateBackground(BackGroudDTO backGroudDTO)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            string requestURL = $"{apiUrl}/api/BackGroud/{backGroudDTO.IdBackGround}";
            HttpClient client = new HttpClient();
            var response = client.PutAsJsonAsync(requestURL, backGroudDTO).Result;



            string requestGetURL = $"{apiUrl}/api/BackGroud";
            List<BackGroudDTO> backGroudDTOs = new List<BackGroudDTO>();
            backGroudDTOs = client.GetFromJsonAsync<List<BackGroudDTO>>(requestGetURL).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Cập nhật ảnh background thất bại", SweetAlertMessageType.error);
                return View("Manage", backGroudDTOs);
            }
            ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Cập nhật ảnh background thành công", SweetAlertMessageType.success);
            return View("Manage", backGroudDTOs);
        }

        public IActionResult DeleteBackground(int IdBackGround)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            var apiUrl = _configuration["AppSettings:ApiUrl"];

            string requestURL = $"{apiUrl}/api/BackGroud/{IdBackGround}";
            HttpClient client = new HttpClient();
            var response = client.DeleteAsync(requestURL).Result;

            string requestGetURL = $"{apiUrl}/api/BackGroud";
            List<BackGroudDTO> backGroudDTOs = new List<BackGroudDTO>();
            backGroudDTOs = client.GetFromJsonAsync<List<BackGroudDTO>>(requestGetURL).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Xóa ảnh background thất bại", SweetAlertMessageType.error);
                return View("Manage", backGroudDTOs);
            }
            ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Xóa ảnh background thành công", SweetAlertMessageType.success);
            return View("Manage", backGroudDTOs);
        }



    }
}
