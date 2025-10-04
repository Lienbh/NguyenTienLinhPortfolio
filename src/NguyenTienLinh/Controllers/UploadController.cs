using Microsoft.AspNetCore.Mvc;

namespace NguyenTienLinh.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        // POST: api/Upload/upload
        [HttpPost("upload")]
        public async Task<ActionResult> UploadImage(IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                {
                    return BadRequest("No image file provided");
                }

                // Validate file type
                var allowedExtensions = new[]
                {
                    ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".tiff", ".tif",
                    ".svg", ".ico", ".heic", ".heif", ".avif", ".raw", ".cr2", ".nef",
                    ".arw", ".dng", ".orf", ".rw2", ".pef", ".srw", ".x3f", ".raf"
                };
                var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(
                        "Invalid file type. Only image formats are allowed (JPG, PNG, GIF, WebP, BMP, TIFF, SVG, ICO, HEIC, AVIF, RAW formats).");
                }

                // Validate file size (max 10MB)
                if (image.Length > 10 * 1024 * 1024)
                {
                    return BadRequest("File size too large. Maximum size is 10MB.");
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var webAppPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebApp", "wwwroot", "assets",
                    "gallery");
                var filePath = Path.Combine(webAppPath, fileName);

                // Ensure directory exists
                Directory.CreateDirectory(webAppPath);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return Ok(new { success = true, fileName = fileName, originalName = image.FileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Upload/banner
        [HttpPost("banner")]
        public async Task<ActionResult> UploadBanner(IFormFile image, int galleryId)
        {
            try
            {
                if (image == null || image.Length == 0)
                {
                    return BadRequest("No image file provided");
                }

                // Validate file type
                var allowedExtensions = new[]
                {
                    ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".tiff", ".tif",
                    ".svg", ".ico", ".heic", ".heif", ".avif", ".raw", ".cr2", ".nef",
                    ".arw", ".dng", ".orf", ".rw2", ".pef", ".srw", ".x3f", ".raf"
                };
                var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(
                        "Invalid file type. Only image formats are allowed (JPG, PNG, GIF, WebP, BMP, TIFF, SVG, ICO, HEIC, AVIF, RAW formats).");
                }

                // Validate file size (max 10MB)
                if (image.Length > 10 * 1024 * 1024)
                {
                    return BadRequest("File size too large. Maximum size is 10MB.");
                }

                // Generate unique filename
                var fileName = $"banner_{Guid.NewGuid()}{fileExtension}";
                var webAppPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebApp", "wwwroot", "assets",
                    "gallery", galleryId.ToString());
                var filePath = Path.Combine(webAppPath, fileName);

                // Ensure directory exists
                Directory.CreateDirectory(webAppPath);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return Ok(new { success = true, fileName = fileName, originalName = image.FileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Upload/gallery
        [HttpPost("gallery")]
        public async Task<ActionResult> UploadGalleryPhoto(IFormFile image, int galleryId)
        {
            try
            {
                if (image == null || image.Length == 0)
                {
                    return BadRequest("No image file provided");
                }

                // Validate file type
                var allowedExtensions = new[]
                {
                    ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".tiff", ".tif",
                    ".svg", ".ico", ".heic", ".heif", ".avif", ".raw", ".cr2", ".nef",
                    ".arw", ".dng", ".orf", ".rw2", ".pef", ".srw", ".x3f", ".raf"
                };
                var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(
                        "Invalid file type. Only image formats are allowed (JPG, PNG, GIF, WebP, BMP, TIFF, SVG, ICO, HEIC, AVIF, RAW formats).");
                }

                // Validate file size (max 10MB)
                if (image.Length > 10 * 1024 * 1024)
                {
                    return BadRequest("File size too large. Maximum size is 10MB.");
                }

                // Generate unique filename
                var fileName = $"photo_{Guid.NewGuid()}{fileExtension}";
                var webAppPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebApp", "wwwroot", "assets",
                    "gallery", galleryId.ToString());
                var filePath = Path.Combine(webAppPath, fileName);

                // Ensure directory exists
                Directory.CreateDirectory(webAppPath);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                return Ok(new { success = true, fileName = fileName, originalName = image.FileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Upload/move-banner
        [HttpPost("move-banner")]
        public async Task<ActionResult> MoveBannerToGalleryFolder([FromBody] MoveBannerRequest request)
        {
            try
            {
                var generalPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebApp", "wwwroot", "assets",
                    "gallery", request.BannerPath);

                var galleryPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebApp", "wwwroot", "assets",
                    "gallery", request.GalleryId.ToString());

                // Ensure gallery directory exists
                Directory.CreateDirectory(galleryPath);

                var galleryFilePath = Path.Combine(galleryPath, request.BannerPath);

                // Move file from general folder to gallery folder
                if (System.IO.File.Exists(generalPath))
                {
                    System.IO.File.Move(generalPath, galleryFilePath);
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Upload/move-photos
        [HttpPost("move-photos")]
        public async Task<ActionResult> MovePhotosToGalleryFolder([FromBody] MovePhotosRequest request)
        {
            try
            {
                var generalPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebApp", "wwwroot", "assets",
                    "gallery");

                var galleryPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebApp", "wwwroot", "assets",
                    "gallery", request.GalleryId.ToString());

                // Ensure gallery directory exists
                Directory.CreateDirectory(galleryPath);

                foreach (var photoPath in request.Photos)
                {
                    var sourcePath = Path.Combine(generalPath, photoPath);
                    var destPath = Path.Combine(galleryPath, photoPath);

                    if (System.IO.File.Exists(sourcePath))
                    {
                        System.IO.File.Move(sourcePath, destPath);
                    }
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Upload/rollback
        [HttpPost("rollback")]
        public async Task<ActionResult> RollbackUploadedFiles([FromBody] RollbackRequest request)
        {
            try
            {
                var generalPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebApp", "wwwroot", "assets",
                    "gallery");

                foreach (var fileName in request.Files)
                {
                    var filePath = Path.Combine(generalPath, fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class MoveBannerRequest
    {
        public string BannerPath { get; set; } = string.Empty;
        public int GalleryId { get; set; }
    }

    public class MovePhotosRequest
    {
        public List<string> Photos { get; set; } = new List<string>();
        public int GalleryId { get; set; }
    }

    public class RollbackRequest
    {
        public List<string> Files { get; set; } = new List<string>();
    }
}
