using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly IConfiguration _configuration;

        public GalleryController(ILogger<GalleryController> logger, IConfiguration configuration)
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
            string requestURL = $"{apiUrl}/api/Gallery";

            try
            {
                var response = client.GetFromJsonAsync<List<GalleryDTO>>(requestURL).Result;
                return View(response ?? new List<GalleryDTO>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching galleries");
                return View(new List<GalleryDTO>());
            }
        }

        public IActionResult Details(int id)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }

            var apiUrl = _configuration["AppSettings:ApiUrl"];
            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery/{id}";

            try
            {
                var response = client.GetFromJsonAsync<GalleryDTO>(requestURL).Result;
                if (response == null)
                {
                    return NotFound();
                }
                return View(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gallery details");
                return NotFound();
            }
        }

        public IActionResult Manage()
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }

            var apiUrl = _configuration["AppSettings:ApiUrl"];
            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery";

            try
            {
                var response = client.GetFromJsonAsync<List<GalleryDTO>>(requestURL).Result;
                return View(response ?? new List<GalleryDTO>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching galleries for management");
                return View(new List<GalleryDTO>());
            }
        }

        [HttpPost]
        public IActionResult CreateGallery(GalleryDTO galleryDTO)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }

            var apiUrl = _configuration["AppSettings:ApiUrl"];
            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery";

            try
            {
                var response = client.PostAsJsonAsync(requestURL, galleryDTO).Result;

                string requestGetURL = $"{apiUrl}/api/Gallery";
                var galleries = client.GetFromJsonAsync<List<GalleryDTO>>(requestGetURL).Result ?? new List<GalleryDTO>();

                if (response.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                        "Tạo gallery thất bại", SweetAlertMessageType.error);
                    return View("Manage", galleries);
                }

                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Tạo gallery thành công", SweetAlertMessageType.success);
                return View("Manage", galleries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating gallery");
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Tạo gallery thất bại", SweetAlertMessageType.error);
                return RedirectToAction("Manage");
            }
        }

        [HttpPost]
        public IActionResult UpdateGallery(GalleryDTO galleryDTO)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }

            var apiUrl = _configuration["AppSettings:ApiUrl"];
            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery/{galleryDTO.IdGallery}";

            try
            {
                var response = client.PutAsJsonAsync(requestURL, galleryDTO).Result;

                string requestGetURL = $"{apiUrl}/api/Gallery";
                var galleries = client.GetFromJsonAsync<List<GalleryDTO>>(requestGetURL).Result ?? new List<GalleryDTO>();

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                        "Cập nhật gallery thất bại", SweetAlertMessageType.error);
                    return View("Manage", galleries);
                }

                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Cập nhật gallery thành công", SweetAlertMessageType.success);
                return View("Manage", galleries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating gallery");
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Cập nhật gallery thất bại", SweetAlertMessageType.error);
                return RedirectToAction("Manage");
            }
        }

        public IActionResult DeleteGallery(int id)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }

            var apiUrl = _configuration["AppSettings:ApiUrl"];
            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery/{id}";

            try
            {
                var response = client.DeleteAsync(requestURL).Result;

                string requestGetURL = $"{apiUrl}/api/Gallery";
                var galleries = client.GetFromJsonAsync<List<GalleryDTO>>(requestGetURL).Result ?? new List<GalleryDTO>();

                if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                        "Xóa gallery thất bại", SweetAlertMessageType.error);
                    return View("Manage", galleries);
                }

                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Xóa gallery thành công", SweetAlertMessageType.success);
                return View("Manage", galleries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting gallery");
                ViewBag.SweetAlertShowMessage = SweetAlertHelper.ShowMessage("Thông báo",
                    "Xóa gallery thất bại", SweetAlertMessageType.error);
                return RedirectToAction("Manage");
            }
        }

        public IActionResult Manager(int? id)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }

            var apiUrl = _configuration["AppSettings:ApiUrl"];
            ViewBag.ApiUrl = apiUrl;

            if (id == null || id == 0)
            {
                // Create new gallery
                var newGallery = new GalleryDTO
                {
                    IdGallery = 0,
                    Title = "Gallery Mới",
                    Description = "",
                    CreatedDate = DateTime.Now,
                    BannerImagePath = "",
                    BannerImageName = "",
                    GalleryItems = new List<GalleryItemDTO>()
                };
                return View(newGallery);
            }

            // Edit existing gallery
            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery/{id}";

            try
            {
                var response = client.GetFromJsonAsync<GalleryDTO>(requestURL).Result;
                if (response == null)
                {
                    return NotFound();
                }

                return View(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gallery for management");
                return NotFound();
            }
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
            var HostPath = Path.Combine("wwwroot", "assets", "gallery", imgName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), HostPath);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            return Json(new { imgNew = imgName });
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

    }
}
