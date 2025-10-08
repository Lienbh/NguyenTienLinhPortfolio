using AppApi.IRepository;
using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Linq;

using System.Text.RegularExpressions;

namespace NguyenTienLinh.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryRepo _galleryRepo;

        public GalleryController(IGalleryRepo galleryRepo)
        {
            _galleryRepo = galleryRepo;
        }

        // Helper method to generate SEO-friendly URL from title
        private async Task<string> GenerateUrlSlugAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
                return $"gallery-{DateTime.Now:yyyyMMddHHmmss}";

            // Convert Vietnamese to Latin characters
            string latinTitle = ConvertVietnameseToLatin(title);

            // Convert to lowercase and replace spaces with hyphens
            string slug = Regex.Replace(latinTitle.ToLower(), @"[^a-z0-9\s-]", "")
                              .Replace(" ", "-")
                              .Replace("--", "-")
                              .Trim('-');

            // Ensure slug is not empty
            if (string.IsNullOrEmpty(slug))
                slug = $"gallery-{DateTime.Now:yyyyMMddHHmmss}";

            // Check if URL already exists and make it unique
            string originalSlug = slug;
            int counter = 1;

            while (await _galleryRepo.GetGalleryByUrlAsync($"/{slug}") != null)
            {
                slug = $"{originalSlug}-{counter}";
                counter++;
            }

            return $"/{slug}";
        }

        // Helper method to convert Vietnamese characters to Latin
        private string ConvertVietnameseToLatin(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Vietnamese to Latin mapping
            var vietnameseMap = new Dictionary<string, string>
            {
                // Lowercase vowels with diacritics
                {"à", "a"}, {"á", "a"}, {"ạ", "a"}, {"ả", "a"}, {"ã", "a"},
                {"â", "a"}, {"ầ", "a"}, {"ấ", "a"}, {"ậ", "a"}, {"ẩ", "a"}, {"ẫ", "a"},
                {"ă", "a"}, {"ằ", "a"}, {"ắ", "a"}, {"ặ", "a"}, {"ẳ", "a"}, {"ẵ", "a"},
                {"è", "e"}, {"é", "e"}, {"ẹ", "e"}, {"ẻ", "e"}, {"ẽ", "e"},
                {"ê", "e"}, {"ề", "e"}, {"ế", "e"}, {"ệ", "e"}, {"ể", "e"}, {"ễ", "e"},
                {"ì", "i"}, {"í", "i"}, {"ị", "i"}, {"ỉ", "i"}, {"ĩ", "i"},
                {"ò", "o"}, {"ó", "o"}, {"ọ", "o"}, {"ỏ", "o"}, {"õ", "o"},
                {"ô", "o"}, {"ồ", "o"}, {"ố", "o"}, {"ộ", "o"}, {"ổ", "o"}, {"ỗ", "o"},
                {"ơ", "o"}, {"ờ", "o"}, {"ớ", "o"}, {"ợ", "o"}, {"ở", "o"}, {"ỡ", "o"},
                {"ù", "u"}, {"ú", "u"}, {"ụ", "u"}, {"ủ", "u"}, {"ũ", "u"},
                {"ư", "u"}, {"ừ", "u"}, {"ứ", "u"}, {"ự", "u"}, {"ử", "u"}, {"ữ", "u"},
                {"ỳ", "y"}, {"ý", "y"}, {"ỵ", "y"}, {"ỷ", "y"}, {"ỹ", "y"},
                {"đ", "d"},
                
                // Uppercase vowels with diacritics
                {"À", "A"}, {"Á", "A"}, {"Ạ", "A"}, {"Ả", "A"}, {"Ã", "A"},
                {"Â", "A"}, {"Ầ", "A"}, {"Ấ", "A"}, {"Ậ", "A"}, {"Ẩ", "A"}, {"Ẫ", "A"},
                {"Ă", "A"}, {"Ằ", "A"}, {"Ắ", "A"}, {"Ặ", "A"}, {"Ẳ", "A"}, {"Ẵ", "A"},
                {"È", "E"}, {"É", "E"}, {"Ẹ", "E"}, {"Ẻ", "E"}, {"Ẽ", "E"},
                {"Ê", "E"}, {"Ề", "E"}, {"Ế", "E"}, {"Ệ", "E"}, {"Ể", "E"}, {"Ễ", "E"},
                {"Ì", "I"}, {"Í", "I"}, {"Ị", "I"}, {"Ỉ", "I"}, {"Ĩ", "I"},
                {"Ò", "O"}, {"Ó", "O"}, {"Ọ", "O"}, {"Ỏ", "O"}, {"Õ", "O"},
                {"Ô", "O"}, {"Ồ", "O"}, {"Ố", "O"}, {"Ộ", "O"}, {"Ổ", "O"}, {"Ỗ", "O"},
                {"Ơ", "O"}, {"Ờ", "O"}, {"Ớ", "O"}, {"Ợ", "O"}, {"Ở", "O"}, {"Ỡ", "O"},
                {"Ù", "U"}, {"Ú", "U"}, {"Ụ", "U"}, {"Ủ", "U"}, {"Ũ", "U"},
                {"Ư", "U"}, {"Ừ", "U"}, {"Ứ", "U"}, {"Ự", "U"}, {"Ử", "U"}, {"Ữ", "U"},
                {"Ỳ", "Y"}, {"Ý", "Y"}, {"Ỵ", "Y"}, {"Ỷ", "Y"}, {"Ỹ", "Y"},
                {"Đ", "D"},
                
                // Special characters and punctuation
                {"?", ""}, {"!", ""}, {".", ""}, {",", ""}, {":", ""}, {";", ""},
                {"'", ""}, {"\"", ""}, {"(", ""}, {")", ""}, {"[", ""}, {"]", ""},
                {"{", ""}, {"}", ""}, {"<", ""}, {">", ""}, {"|", ""}, {"\\", ""},
                {"/", ""}, {"@", ""}, {"#", ""}, {"$", ""}, {"%", ""}, {"^", ""},
                {"&", ""}, {"*", ""}, {"+", ""}, {"=", ""}, {"~", ""}, {"`", ""},
                {" ", "-"}, {"_", "-"}, {"—", "-"}, {"–", "-"}, {"-", "-"}
            };

            foreach (var kvp in vietnameseMap)
            {
                text = text.Replace(kvp.Key, kvp.Value);
            }

            // Additional cleanup for multiple consecutive hyphens and special cases
            text = Regex.Replace(text, @"-+", "-"); // Replace multiple hyphens with single hyphen
            text = text.Trim('-'); // Remove leading/trailing hyphens

            return text;
        }

        // GET: api/Gallery
        [HttpGet]
        public async Task<ActionResult<PagedResult<GalleryDTO>>> GetAllGalleries(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            try
            {
                var result = await _galleryRepo.GetGalleriesPagedAsync(page, pageSize, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Gallery/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GalleryDTO>> GetGallery(int id)
        {
            try
            {
                var gallery = await _galleryRepo.GetGalleryByIdAsync(id);
                if (gallery == null)
                {
                    return NotFound($"Gallery with ID {id} not found.");
                }

                return Ok(gallery);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Gallery/by-url/dam-cuoi-bach-hong-lien
        [HttpGet("by-url/{url}")]
        public async Task<ActionResult<GalleryDTO>> GetGalleryByUrl(string url)
        {
            try
            {
                var gallery = await _galleryRepo.GetGalleryByUrlAsync($"/{url}");
                if (gallery == null)
                {
                    return NotFound($"Gallery with URL /{url} not found.");
                }

                return Ok(gallery);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Gallery
        [HttpPost]
        public async Task<ActionResult<GalleryDTO>> CreateGallery([FromBody] GalleryDTO galleryDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdGallery = await _galleryRepo.CreateGalleryAsync(galleryDTO);
                return CreatedAtAction(nameof(GetGallery), new { id = createdGallery.IdGallery }, createdGallery);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Gallery/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GalleryDTO>> UpdateGallery(int id, [FromBody] GalleryDTO galleryDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedGallery = await _galleryRepo.UpdateGalleryAsync(id, galleryDTO);
                if (updatedGallery == null)
                {
                    return NotFound($"Gallery with ID {id} not found.");
                }

                return Ok(updatedGallery);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Gallery/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGallery(int id)
        {
            try
            {
                Console.WriteLine($"Attempting to delete gallery {id}");

                var result = await _galleryRepo.DeleteGalleryAsync(id);
                if (!result)
                {
                    Console.WriteLine($"Gallery with ID {id} not found or deletion failed");
                    return NotFound($"Gallery with ID {id} not found.");
                }

                Console.WriteLine($"Successfully deleted gallery {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting gallery {id}: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Gallery/5/items
        [HttpGet("{id}/items")]
        public async Task<ActionResult<List<GalleryItemDTO>>> GetGalleryItems(int id)
        {
            try
            {
                var galleryItems = await _galleryRepo.GetGalleryItemsByGalleryIdAsync(id);
                return Ok(galleryItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Gallery/items
        [HttpPost("items")]
        public async Task<ActionResult<GalleryItemDTO>> CreateGalleryItem([FromBody] GalleryItemDTO galleryItemDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdItem = await _galleryRepo.CreateGalleryItemAsync(galleryItemDTO);
                return CreatedAtAction(nameof(GetGalleryItems), new { id = createdItem.IdGallery }, createdItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Gallery/items/5
        [HttpPut("items/{id}")]
        public async Task<ActionResult<GalleryItemDTO>> UpdateGalleryItem(int id,
            [FromBody] GalleryItemDTO galleryItemDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedItem = await _galleryRepo.UpdateGalleryItemAsync(id, galleryItemDTO);
                return Ok(updatedItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Gallery/items/5
        [HttpDelete("items/{id}")]
        public async Task<ActionResult> DeleteGalleryItem(int id)
        {
            try
            {
                var result = await _galleryRepo.DeleteGalleryItemAsync(id);
                if (!result)
                {
                    return NotFound($"Gallery item with ID {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Gallery/items/5/position
        [HttpPut("items/{id}/position")]
        public async Task<ActionResult> UpdateGalleryItemPosition(int id, [FromBody] int newPosition)
        {
            try
            {
                var result = await _galleryRepo.UpdateGalleryItemPositionAsync(id, newPosition);
                if (!result)
                {
                    return NotFound($"Gallery item with ID {id} not found.");
                }

                return Ok(new { message = "Position updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Gallery/{id}/items
        [HttpDelete("{id}/items")]
        public async Task<ActionResult> ClearGalleryItems(int id)
        {
            try
            {
                var success = await _galleryRepo.ClearGalleryItemsAsync(id);
                if (success)
                {
                    return Ok();
                }
                return BadRequest("Failed to clear gallery items");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Gallery/items/batch
        [HttpPost("items/batch")]
        public async Task<ActionResult> CreateGalleryItemsBatch([FromBody] List<GalleryItemDTO> galleryItems)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdItems = await _galleryRepo.CreateGalleryItemsBatchAsync(galleryItems);
                return Ok(createdItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Gallery/create-with-files
        [HttpPost("create-with-files")]
        public async Task<ActionResult> CreateGalleryWithFiles([FromForm] CreateGalleryWithFilesRequest request)
        {
            try
            {
                // Check if title already exists
                if (await _galleryRepo.IsTitleExistsAsync(request.Title.Trim()))
                {
                    return BadRequest(new { success = false, message = "Tiêu đề photo đã tồn tại. Vui lòng chọn tiêu đề khác." });
                }

                // Step 1: Create gallery first
                var galleryData = new GalleryDTO
                {
                    Title = request.Title,
                    Url = await GenerateUrlSlugAsync(request.Title.Trim()),
                    CreatedDate = DateTime.Now
                };

                var createdGallery = await _galleryRepo.CreateGalleryAsync(galleryData);
                var galleryId = createdGallery.IdGallery;

                var uploadedFiles = new List<string>();

                try
                {
                    // Step 2: Upload banner if provided
                    if (request.BannerImage != null && request.BannerImage.Length > 0)
                    {
                        var bannerPath = await UploadImageFile(request.BannerImage, galleryId, "banner");
                        uploadedFiles.Add(bannerPath);

                        // Update gallery with banner path
                        createdGallery.BannerImagePath = bannerPath;
                        createdGallery.BannerImageName = request.BannerImage.FileName;
                        await _galleryRepo.UpdateGalleryAsync(createdGallery.IdGallery, createdGallery);
                    }

                    // Step 3: Upload gallery photos if provided
                    var galleryItems = new List<GalleryItemDTO>();
                    if (request.GalleryImages != null && request.GalleryImages.Count > 0)
                    {
                        for (int i = 0; i < request.GalleryImages.Count; i++)
                        {
                            var photoPath = await UploadImageFile(request.GalleryImages[i], galleryId, "photo");
                            uploadedFiles.Add(photoPath);

                            galleryItems.Add(new GalleryItemDTO
                            {
                                IdGallery = galleryId,
                                ImagePath = photoPath,
                                ImageName = request.GalleryImages[i].FileName,
                                Description = "",
                                DisplayOrder = i + 1
                            });
                        }

                        // Save gallery items
                        await _galleryRepo.CreateGalleryItemsBatchAsync(galleryItems);
                    }

                    return Ok(new
                    {
                        success = true,
                        galleryId = galleryId,
                        message = "Gallery created successfully with files"
                    });
                }
                catch (Exception ex)
                {
                    // Rollback: Delete uploaded files
                    await RollbackFiles(uploadedFiles, galleryId);

                    // Delete gallery if it was created
                    await _galleryRepo.DeleteGalleryAsync(galleryId);

                    return StatusCode(500, $"Error uploading files: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Gallery/update-with-files/{id}
        [HttpPut("update-with-files/{id}")]
        public async Task<ActionResult> UpdateGalleryWithFiles(int id, [FromForm] UpdateGalleryWithFilesRequest request)
        {
            try
            {
                var existingGallery = await _galleryRepo.GetGalleryByIdAsync(id);
                if (existingGallery == null)
                {
                    return NotFound("Gallery not found");
                }

                // Check if title already exists (excluding current gallery)
                if (await _galleryRepo.IsTitleExistsAsync(request.Title.Trim(), id))
                {
                    return BadRequest(new { success = false, message = "Tiêu đề photo đã tồn tại. Vui lòng chọn tiêu đề khác." });
                }

                var uploadedFiles = new List<string>();

                try
                {
                    // Step 1: Update gallery basic info
                    existingGallery.Title = request.Title;
                    existingGallery.Url = await GenerateUrlSlugAsync(request.Title.Trim());

                    // Step 2: Upload new banner if provided
                    if (request.BannerImage != null && request.BannerImage.Length > 0)
                    {
                        var bannerPath = await UploadImageFile(request.BannerImage, id, "banner");
                        uploadedFiles.Add(bannerPath);

                        existingGallery.BannerImagePath = bannerPath;
                        existingGallery.BannerImageName = request.BannerImage.FileName;
                    }

                    // Step 3: Upload new gallery photos if provided
                    if (request.GalleryImages != null && request.GalleryImages.Count > 0)
                    {
                        // Clear existing items
                        await _galleryRepo.ClearGalleryItemsAsync(id);

                        var galleryItems = new List<GalleryItemDTO>();
                        for (int i = 0; i < request.GalleryImages.Count; i++)
                        {
                            var photoPath = await UploadImageFile(request.GalleryImages[i], id, "photo");
                            uploadedFiles.Add(photoPath);

                            galleryItems.Add(new GalleryItemDTO
                            {
                                IdGallery = id,
                                ImagePath = photoPath,
                                ImageName = request.GalleryImages[i].FileName,
                                Description = "",
                                DisplayOrder = i + 1
                            });
                        }

                        // Save new gallery items
                        await _galleryRepo.CreateGalleryItemsBatchAsync(galleryItems);
                    }

                    // Update gallery
                    await _galleryRepo.UpdateGalleryAsync(id, existingGallery);

                    return Ok(new
                    {
                        success = true,
                        message = "Gallery updated successfully with files"
                    });
                }
                catch (Exception ex)
                {
                    // Rollback: Delete uploaded files
                    await RollbackFiles(uploadedFiles, id);

                    return StatusCode(500, $"Error uploading files: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<string> UploadImageFile(IFormFile image, int galleryId, string prefix)
        {
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
                throw new ArgumentException("Invalid file type. Only image formats are allowed.");
            }

            // Validate file size (max 100MB)
            if (image.Length > 100 * 1024 * 1024)
            {
                throw new ArgumentException("File size too large. Maximum size is 100MB.");
            }

            // Generate unique filename
            var fileName = $"{prefix}_{Guid.NewGuid()}{fileExtension}";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "gallery", galleryId.ToString());
            var filePath = Path.Combine(uploadPath, fileName);

            // Ensure directory exists
            Directory.CreateDirectory(uploadPath);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return fileName;
        }

        private async Task RollbackFiles(List<string> files, int galleryId)
        {
            try
            {
                var galleryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "gallery", galleryId.ToString());

                foreach (var fileName in files)
                {
                    var filePath = Path.Combine(galleryPath, fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log rollback error but don't throw
                Console.WriteLine($"Rollback error: {ex.Message}");
            }
        }

        // PUT: api/Gallery/update-with-changes/{id}
        [HttpPut("update-with-changes/{id}")]
        public async Task<ActionResult> UpdateGalleryWithChanges(int id, [FromForm] GalleryUpdateRequest request)
        {
            try
            {
                var existingGallery = await _galleryRepo.GetGalleryByIdAsync(id);
                if (existingGallery == null)
                {
                    return NotFound("Gallery not found");
                }

                var uploadedFiles = new List<string>();

                try
                {
                    // 1. Update basic info
                    existingGallery.Title = request.Title;
                    existingGallery.Url = await GenerateUrlSlugAsync(request.Title);

                    // 2. Handle banner update
                    if (request.BannerImage != null && request.BannerImage.Length > 0)
                    {
                        // Delete old banner file
                        if (!string.IsNullOrEmpty(existingGallery.BannerImagePath))
                        {
                            await DeleteFile(existingGallery.BannerImagePath, id);
                        }

                        // Upload new banner
                        var bannerPath = await UploadImageFile(request.BannerImage, id, "banner");
                        uploadedFiles.Add(bannerPath);
                        existingGallery.BannerImagePath = bannerPath;
                        existingGallery.BannerImageName = request.BannerImage.FileName;
                    }

                    // 3. Handle photo deletions
                    foreach (var photoId in request.PhotosToDelete)
                    {
                        var photo = await _galleryRepo.GetGalleryItemByIdAsync(photoId);
                        if (photo != null)
                        {
                            // Delete file
                            await DeleteFile(photo.ImagePath, id);
                            // Delete from database
                            await _galleryRepo.DeleteGalleryItemAsync(photoId);
                        }
                    }

                    // 4. Handle photo updates (replace)
                    for (int i = 0; i < request.PhotosToUpdate.Count; i++)
                    {
                        var photoId = request.PhotosToUpdatePhotoId[i];
                        var photo = await _galleryRepo.GetGalleryItemByIdAsync(photoId);
                        if (photo != null)
                        {
                            // Delete old file
                            await DeleteFile(photo.ImagePath, id);

                            // Upload new file
                            var newPath = await UploadImageFile(request.PhotosToUpdate[i], id, "photo");
                            uploadedFiles.Add(newPath);

                            // Update database using existing method
                            var updatedItem = new GalleryItemDTO
                            {
                                IdGalleryItem = photoId,
                                IdGallery = photo.IdGallery,
                                ImagePath = newPath,
                                ImageName = request.PhotosToUpdate[i].FileName,
                                Description = photo.Description,
                                DisplayOrder = request.PhotosToUpdateDisplayOrder[i]
                            };
                            await _galleryRepo.UpdateGalleryItemAsync(photoId, updatedItem);
                        }
                    }

                    // 5. Handle photo additions
                    for (int i = 0; i < request.PhotosToAdd.Count; i++)
                    {
                        var photoPath = await UploadImageFile(request.PhotosToAdd[i], id, "photo");
                        uploadedFiles.Add(photoPath);

                        var newItem = new GalleryItemDTO
                        {
                            IdGallery = id,
                            ImagePath = photoPath,
                            ImageName = request.PhotosToAdd[i].FileName,
                            Description = "",
                            DisplayOrder = request.PhotosToAddDisplayOrder[i]
                        };

                        await _galleryRepo.CreateGalleryItemAsync(newItem);
                    }

                    // 6. Handle photo reordering - Simple swap approach
                    if (request.PhotosToReorderPhotoId.Count > 0)
                    {
                        Console.WriteLine($"Processing {request.PhotosToReorderPhotoId.Count} reorder operations");

                        // Get all items in the gallery
                        var allItems = await _galleryRepo.GetGalleryItemsByGalleryIdAsync(id);
                        Console.WriteLine($"Found {allItems.Count} items in gallery {id}");

                        // Process each reorder operation
                        for (int i = 0; i < request.PhotosToReorderPhotoId.Count; i++)
                        {
                            var photoId = request.PhotosToReorderPhotoId[i];
                            var newPosition = request.PhotosToReorderNewOrder[i];

                            Console.WriteLine($"Moving photo {photoId} to position {newPosition}");

                            // Find the item to move
                            var itemToMove = allItems.FirstOrDefault(x => x.IdGalleryItem == photoId);
                            if (itemToMove == null) continue;

                            // Find the item currently at the target position
                            var itemAtTarget = allItems.FirstOrDefault(x => x.DisplayOrder == newPosition);

                            if (itemAtTarget != null)
                            {
                                // Simple swap: exchange DisplayOrder values
                                var tempOrder = itemToMove.DisplayOrder;
                                itemToMove.DisplayOrder = itemAtTarget.DisplayOrder;
                                itemAtTarget.DisplayOrder = tempOrder;

                                Console.WriteLine($"Swapped: Item {photoId} ({tempOrder} -> {newPosition}) and Item {itemAtTarget.IdGalleryItem} ({newPosition} -> {tempOrder})");

                                // Update both items
                                await _galleryRepo.UpdateGalleryItemAsync(itemToMove.IdGalleryItem, itemToMove);
                                await _galleryRepo.UpdateGalleryItemAsync(itemAtTarget.IdGalleryItem, itemAtTarget);
                            }
                            else
                            {
                                // No item at target position, just move the item
                                itemToMove.DisplayOrder = newPosition;
                                await _galleryRepo.UpdateGalleryItemAsync(itemToMove.IdGalleryItem, itemToMove);
                                Console.WriteLine($"Moved item {photoId} to position {newPosition}");
                            }
                        }

                        Console.WriteLine($"Successfully processed reorder operations");
                    }

                    // 7. Update gallery
                    await _galleryRepo.UpdateGalleryAsync(id, existingGallery);

                    return Ok(new { success = true, message = "Gallery updated successfully" });
                }
                catch (Exception ex)
                {
                    // Rollback only uploaded files
                    await RollbackFiles(uploadedFiles, id);
                    return StatusCode(500, $"Error updating gallery: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task DeleteFile(string fileName, int galleryId)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "gallery", galleryId.ToString(), fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file {fileName}: {ex.Message}");
            }
        }

        // PUT: api/Gallery/{id}/position
        [HttpPut("{id}/position")]
        public async Task<ActionResult> UpdateGalleryPosition(int id, [FromBody] int newPosition)
        {
            try
            {
                var result = await _galleryRepo.UpdateGalleryPositionAsync(id, newPosition);
                if (!result)
                {
                    return NotFound($"Gallery with ID {id} not found.");
                }

                return Ok(new { message = "Gallery position updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Gallery/reorder
        [HttpPost("reorder")]
        public async Task<ActionResult> ReorderGalleries([FromBody] List<int> galleryIds)
        {
            try
            {
                var result = await _galleryRepo.ReorderGalleriesAsync(galleryIds);
                if (!result)
                {
                    return BadRequest("Failed to reorder galleries");
                }

                return Ok(new { message = "Galleries reordered successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }

    public class CreateGalleryWithFilesRequest
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile? BannerImage { get; set; }
        public List<IFormFile>? GalleryImages { get; set; }
    }

    public class UpdateGalleryWithFilesRequest
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile? BannerImage { get; set; }
        public List<IFormFile>? GalleryImages { get; set; }
    }

    public class GalleryUpdateRequest
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile? BannerImage { get; set; }

        // Simple arrays for FormData binding
        public List<IFormFile> PhotosToAdd { get; set; } = new();
        public List<int> PhotosToAddDisplayOrder { get; set; } = new();

        public List<IFormFile> PhotosToUpdate { get; set; } = new();
        public List<int> PhotosToUpdatePhotoId { get; set; } = new();
        public List<int> PhotosToUpdateDisplayOrder { get; set; } = new();

        public List<int> PhotosToDelete { get; set; } = new();

        public List<int> PhotosToReorderPhotoId { get; set; } = new();
        public List<int> PhotosToReorderNewOrder { get; set; } = new();
    }
}