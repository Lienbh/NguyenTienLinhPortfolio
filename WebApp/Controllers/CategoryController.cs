using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            string requestURL = $"https://localhost:7130/api/Categories";
            HttpClient client = new HttpClient();
            var response = client.GetFromJsonAsync<IEnumerable<CategoriesDTO>>(requestURL).Result;
            return View(response);
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

            string requestURL = $"https://localhost:7130/api/Categories";
            HttpClient client = new HttpClient();
            ManageCategoriesDTO manageCategoriesDTO = new ManageCategoriesDTO();
            manageCategoriesDTO.Categories = client.GetFromJsonAsync<List<CategoriesDTO>>(requestURL).Result;
            return View(manageCategoriesDTO);

        }
        [HttpPost]
        public IActionResult CreateMyWork(CategoriesDTO categoriesDTO)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            string requestURL = $"https://localhost:7130/api/Categories";
            HttpClient client = new HttpClient();
            var response = client.PostAsJsonAsync(requestURL, categoriesDTO).Result;



            string requestGetURL = $"https://localhost:7130/api/Categories";
            ManageCategoriesDTO manageCategoriesDTO = new ManageCategoriesDTO();
            manageCategoriesDTO.Categories = client.GetFromJsonAsync<List<CategoriesDTO>>(requestGetURL).Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Thêm mới danh mục thất bại", SweetAlertMessageType.error);
                return View("Manage", manageCategoriesDTO);
            }
            ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Thêm mới danh mục thành công", SweetAlertMessageType.success);
            return View("Manage", manageCategoriesDTO);
        }
        [HttpPost]
        public IActionResult UpdateMyWork(CategoriesDTO categoriesDTO)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            string requestURL = $"https://localhost:7130/api/Categories/{categoriesDTO.IdCategories}";
            HttpClient client = new HttpClient();
            var response = client.PutAsJsonAsync(requestURL, categoriesDTO).Result;

            string requestGetURL = $"https://localhost:7130/api/Categories";
            ManageCategoriesDTO manageCategoriesDTO = new ManageCategoriesDTO();
            manageCategoriesDTO.Categories = client.GetFromJsonAsync<List<CategoriesDTO>>(requestGetURL).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Cập nhật danh mục thất bại", SweetAlertMessageType.error);
                return View("Manage", manageCategoriesDTO);
            }
            ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Cập nhật danh mục thành công", SweetAlertMessageType.success);
            return View("Manage", manageCategoriesDTO);
        }

        public IActionResult DeleteMyWork(int id)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
            string requestURL = $"https://localhost:7130/api/Categories/{id}";
            HttpClient client = new HttpClient();
            var response = client.DeleteAsync(requestURL).Result;

            string requestGetURL = $"https://localhost:7130/api/Categories";
            ManageCategoriesDTO manageCategoriesDTO = new ManageCategoriesDTO();
            manageCategoriesDTO.Categories = client.GetFromJsonAsync<List<CategoriesDTO>>(requestGetURL).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Xóa danh mục thất bại", SweetAlertMessageType.error);
                return View("Manage", manageCategoriesDTO);
            }
            ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Xóa danh mục thành công", SweetAlertMessageType.success);
            return View("Manage", manageCategoriesDTO);
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
            string requestURL = $" https://localhost:7130/api/Categories/get-detail/{id}";
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
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }
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

