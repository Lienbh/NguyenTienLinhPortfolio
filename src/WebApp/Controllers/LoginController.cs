using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {

        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserDTO user)
        {
            var apiUrl = _configuration["AppSettings:ApiUrl"];

            string requestURL = $"{apiUrl}/api/User/login";

            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync(requestURL, user).Result;
            var messageText = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    messageText, SweetAlertMessageType.error);
                return View("Index");
            }
            HttpContext.Session.SetString("currentUser", user.UserName);
            TempData["currentUser"] = user.UserName;
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("currentUser");
            TempData["currentUser"] = "";
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult ForgotPassWord(string Email)
        {
            ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                $"Mật khẩu đã được gửi về tài khoản {Email} của Anh/chị! Vui lòng Anh/chị kiểm tra lại mật khẩu gửi về mail và đăng nhập lại hệ thống.", SweetAlertMessageType.success);
            return View("Login");
        }
    }
}
