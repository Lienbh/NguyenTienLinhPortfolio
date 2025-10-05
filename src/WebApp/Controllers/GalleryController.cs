using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{

    public class GalleryController : Controller
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly IConfiguration _configuration;
        private readonly System.Text.Json.JsonSerializerOptions _jsonOptions;

        public GalleryController(ILogger<GalleryController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _jsonOptions = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IActionResult> Index()
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }

            var apiUrl = _configuration["AppSettings:ApiUrl"];
            var cdnUrl = _configuration["AppSettings:CdnUrl"];
            ViewBag.ApiUrl = apiUrl;
            ViewBag.CdnUrl = cdnUrl;

            try
            {
                // Load initial galleries from backend
                HttpClient client = new HttpClient();
                string requestURL = $"{apiUrl}/api/Gallery?page=1&pageSize=10";

                var response = await client.GetAsync(requestURL);
                var jsonContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var pagedResult = System.Text.Json.JsonSerializer.Deserialize<PagedResult<GalleryDTO>>(jsonContent, _jsonOptions);
                    return View(pagedResult?.Data ?? new List<GalleryDTO>());
                }
                else
                {
                    _logger.LogError("Failed to load galleries: {StatusCode} - {Content}", response.StatusCode, jsonContent);
                    return View(new List<GalleryDTO>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading galleries in Index action");
                return View(new List<GalleryDTO>());
            }
        }

        // Endpoint for loading galleries via AJAX
        [HttpGet("LoadGalleries")]
        public async Task<IActionResult> LoadGalleries(int page = 1, int pageSize = 10, string? search = null)
        {
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery?page={page}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(search))
            {
                requestURL += $"&search={Uri.EscapeDataString(search.Trim())}";
            }

            try
            {
                var response = await client.GetFromJsonAsync<PagedResult<GalleryDTO>>(requestURL);
                if (response != null)
                {
                    return Json(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading galleries");
                return Json(new { error = "Failed to load galleries" });
            }

            return Json(new { error = "No data found" });
        }

        // Endpoint for deleting galleries via AJAX
        [HttpPost("DeleteGallery")]
        public async Task<IActionResult> DeleteGallery(int id)
        {
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery/{id}";

            try
            {
                var response = await client.DeleteAsync(requestURL);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
                else
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, error = errorText });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting gallery {Id}", id);
                return Json(new { success = false, error = "Failed to delete gallery" });
            }
        }

        // Endpoint for creating gallery with files via AJAX
        [HttpPost("CreateGalleryWithFiles")]
        public async Task<IActionResult> CreateGalleryWithFiles()
        {
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery/create-with-files";

            try
            {
                var formData = new MultipartFormDataContent();

                // Get form data from request
                foreach (var key in Request.Form.Keys)
                {
                    formData.Add(new StringContent(Request.Form[key]), key);
                }

                // Get files from request
                foreach (var file in Request.Form.Files)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    formData.Add(fileContent, file.Name, file.FileName);
                }

                var response = await client.PostAsync(requestURL, formData);
                var resultString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse JSON string to object
                    var resultObject = System.Text.Json.JsonSerializer.Deserialize<object>(resultString);
                    return Json(resultObject);
                }
                else
                {
                    // Parse error JSON string to object
                    var errorObject = System.Text.Json.JsonSerializer.Deserialize<object>(resultString);
                    return Json(errorObject);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating gallery with files");
                return Json(new { success = false, error = "Failed to create gallery" });
            }
        }

        // Endpoint for updating gallery with changes via AJAX
        [HttpPut("UpdateGalleryWithChanges/{id}")]
        public async Task<IActionResult> UpdateGalleryWithChanges(int id)
        {
            var apiUrl = _configuration["AppSettings:ApiUrl"];
            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery/update-with-changes/{id}";

            try
            {
                var formData = new MultipartFormDataContent();

                // Get form data from request
                foreach (var key in Request.Form.Keys)
                {
                    formData.Add(new StringContent(Request.Form[key]), key);
                }

                // Get files from request
                foreach (var file in Request.Form.Files)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    formData.Add(fileContent, file.Name, file.FileName);
                }

                var response = await client.PutAsync(requestURL, formData);
                var resultString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Parse JSON string to object
                    var resultObject = System.Text.Json.JsonSerializer.Deserialize<object>(resultString);
                    return Json(resultObject);
                }
                else
                {
                    // Parse error JSON string to object
                    var errorObject = System.Text.Json.JsonSerializer.Deserialize<object>(resultString);
                    return Json(errorObject);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating gallery with changes {Id}", id);
                return Json(new { success = false, error = "Failed to update gallery" });
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


        public IActionResult Manager(int? id)
        {
            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }

            var apiUrl = _configuration["AppSettings:ApiUrl"];
            var cdnUrl = _configuration["AppSettings:CdnUrl"];
            ViewBag.ApiUrl = apiUrl;
            ViewBag.CdnUrl = cdnUrl;

            if (id == null || id == 0)
            {
                // Create new gallery
                var newGallery = new GalleryDTO
                {
                    IdGallery = 0,
                    Title = "Gallery Mới",
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

        public async Task<IActionResult> View(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var getCurrentUser = HttpContext.Session.GetString("currentUser");
            if (!string.IsNullOrEmpty(getCurrentUser))
            {
                TempData["currentUser"] = getCurrentUser;
            }

            var apiUrl = _configuration["AppSettings:ApiUrl"];
            var cdnUrl = _configuration["AppSettings:CdnUrl"];
            ViewBag.ApiUrl = apiUrl;
            ViewBag.CdnUrl = cdnUrl;

            HttpClient client = new HttpClient();
            string requestURL = $"{apiUrl}/api/Gallery/by-url/{Uri.EscapeDataString(slug)}";

            try
            {
                var response = await client.GetAsync(requestURL);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                var gallery = System.Text.Json.JsonSerializer.Deserialize<GalleryDTO>(jsonContent, _jsonOptions);

                if (gallery == null)
                {
                    return NotFound();
                }

                return View("Manager", gallery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching gallery by slug: {Slug}", slug);
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
