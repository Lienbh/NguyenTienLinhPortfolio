using AppApi.IRepository;
using BuildingBlock.DTOS;
using Microsoft.EntityFrameworkCore;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;
using System.Linq;

namespace NguyenTienLinh.Repository
{
    public class GalleryRepo : IGalleryRepo
    {
        private readonly AppDbContext _context;

        public GalleryRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GalleryDTO>> GetAllGalleriesAsync()
        {
            var galleries = await _context.Galleries
                .Include(g => g.GalleryItems)
                .OrderBy(g => g.DisplayOrder)
                .ThenByDescending(g => g.CreatedDate)
                .ToListAsync();

            return galleries.Select(g => new GalleryDTO
            {
                IdGallery = g.IdGallery,
                Title = g.Title,
                Url = g.Url,
                CreatedDate = g.CreatedDate,
                DisplayOrder = g.DisplayOrder,
                GalleryItems = g.GalleryItems.Select(gi => new GalleryItemDTO
                {
                    IdGalleryItem = gi.IdGalleryItem,
                    IdGallery = gi.IdGallery,
                    ImagePath = gi.ImagePath,
                    ImageName = gi.ImageName,
                    Description = gi.Description,
                    DisplayOrder = gi.DisplayOrder
                }).OrderBy(gi => gi.DisplayOrder).ToList(),
                BannerImagePath = g.BannerImagePath,
                BannerImageName = g.BannerImageName
            }).ToList();
        }

        public async Task<PagedResult<GalleryDTO>> GetGalleriesPagedAsync(int page, int pageSize, string? search = null)
        {
            var query = _context.Galleries.Include(g => g.GalleryItems).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                var searchTerm = search.Trim().ToLower();
                query = query.Where(g => g.Title.ToLower().Contains(searchTerm) || g.Url.ToLower().Contains(searchTerm));
            }

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply pagination and ordering
            var galleries = await query
                .OrderBy(g => g.DisplayOrder)
                .ThenByDescending(g => g.CreatedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var data = galleries.Select(g => new GalleryDTO
            {
                IdGallery = g.IdGallery,
                Title = g.Title,
                Url = g.Url,
                CreatedDate = g.CreatedDate,
                DisplayOrder = g.DisplayOrder,
                GalleryItems = g.GalleryItems.Select(gi => new GalleryItemDTO
                {
                    IdGalleryItem = gi.IdGalleryItem,
                    IdGallery = gi.IdGallery,
                    ImagePath = gi.ImagePath,
                    ImageName = gi.ImageName,
                    Description = gi.Description,
                    DisplayOrder = gi.DisplayOrder
                }).OrderBy(gi => gi.DisplayOrder).ToList(),
                BannerImagePath = g.BannerImagePath,
                BannerImageName = g.BannerImageName
            }).ToList();

            return new PagedResult<GalleryDTO>
            {
                Data = data,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        public async Task<GalleryDTO?> GetGalleryByIdAsync(int id)
        {
            var gallery = await _context.Galleries
                .Include(g => g.GalleryItems)
                .FirstOrDefaultAsync(g => g.IdGallery == id);

            if (gallery == null) return null;

            return new GalleryDTO
            {
                IdGallery = gallery.IdGallery,
                Title = gallery.Title,
                Url = gallery.Url,
                CreatedDate = gallery.CreatedDate,
                DisplayOrder = gallery.DisplayOrder,
                GalleryItems = gallery.GalleryItems.Select(gi => new GalleryItemDTO
                {
                    IdGalleryItem = gi.IdGalleryItem,
                    IdGallery = gi.IdGallery,
                    ImagePath = gi.ImagePath,
                    ImageName = gi.ImageName,
                    Description = gi.Description,
                    DisplayOrder = gi.DisplayOrder
                }).OrderBy(gi => gi.DisplayOrder).ToList(),
                BannerImagePath = gallery.BannerImagePath,
                BannerImageName = gallery.BannerImageName
            };
        }

        public async Task<GalleryDTO> CreateGalleryAsync(GalleryDTO galleryDTO)
        {
            // Auto-assign DisplayOrder if not provided
            if (galleryDTO.DisplayOrder == 0)
            {
                var maxOrder = await _context.Galleries.MaxAsync(g => (int?)g.DisplayOrder) ?? 0;
                galleryDTO.DisplayOrder = maxOrder + 1;
            }

            var gallery = new Gallery
            {
                Title = galleryDTO.Title,
                Url = galleryDTO.Url,
                BannerImagePath = galleryDTO.BannerImagePath,
                BannerImageName = galleryDTO.BannerImageName,
                DisplayOrder = galleryDTO.DisplayOrder
            };

            _context.Galleries.Add(gallery);
            await _context.SaveChangesAsync();

            return new GalleryDTO
            {
                IdGallery = gallery.IdGallery,
                Title = gallery.Title,
                Url = gallery.Url,
                CreatedDate = gallery.CreatedDate,
                DisplayOrder = gallery.DisplayOrder,
                BannerImagePath = gallery.BannerImagePath,
                BannerImageName = gallery.BannerImageName,
                GalleryItems = new List<GalleryItemDTO>()
            };
        }

        public async Task<GalleryDTO?> UpdateGalleryAsync(int id, GalleryDTO galleryDTO)
        {
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null) return null;

            gallery.Title = galleryDTO.Title;
            gallery.Url = galleryDTO.Url;
            gallery.BannerImagePath = galleryDTO.BannerImagePath;
            gallery.BannerImageName = galleryDTO.BannerImageName;
            gallery.DisplayOrder = galleryDTO.DisplayOrder;

            await _context.SaveChangesAsync();

            return new GalleryDTO
            {
                IdGallery = gallery.IdGallery,
                Title = gallery.Title,
                Url = gallery.Url,
                CreatedDate = gallery.CreatedDate,
                DisplayOrder = gallery.DisplayOrder,
                BannerImagePath = gallery.BannerImagePath,
                BannerImageName = gallery.BannerImageName,
                GalleryItems = gallery.GalleryItems.Select(gi => new GalleryItemDTO
                {
                    IdGalleryItem = gi.IdGalleryItem,
                    IdGallery = gi.IdGallery,
                    ImagePath = gi.ImagePath,
                    ImageName = gi.ImageName,
                    Description = gi.Description,
                    DisplayOrder = gi.DisplayOrder
                }).OrderBy(gi => gi.DisplayOrder).ToList()
            };
        }

        public async Task<bool> DeleteGalleryAsync(int id)
        {
            try
            {
                var gallery = await _context.Galleries
                    .Include(g => g.GalleryItems)
                    .FirstOrDefaultAsync(g => g.IdGallery == id);

                if (gallery == null)
                {
                    Console.WriteLine($"Gallery with ID {id} not found");
                    return false;
                }

                // Delete all files from server before deleting database records
                await DeleteGalleryFilesAsync(id, gallery);

                // Delete from database
                _context.Galleries.Remove(gallery);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Successfully deleted gallery {id}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting gallery {id}: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        private async Task DeleteGalleryFilesAsync(int galleryId, Gallery gallery)
        {
            try
            {
                var galleryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "gallery", galleryId.ToString());

                // Delete banner file if exists
                if (!string.IsNullOrEmpty(gallery.BannerImagePath))
                {
                    var bannerPath = Path.Combine(galleryPath, gallery.BannerImagePath);
                    if (File.Exists(bannerPath))
                    {
                        File.Delete(bannerPath);
                        Console.WriteLine($"Deleted banner file: {bannerPath}");
                    }
                }

                // Delete all gallery item files
                foreach (var item in gallery.GalleryItems)
                {
                    if (!string.IsNullOrEmpty(item.ImagePath))
                    {
                        var itemPath = Path.Combine(galleryPath, item.ImagePath);
                        if (File.Exists(itemPath))
                        {
                            File.Delete(itemPath);
                            Console.WriteLine($"Deleted gallery item file: {itemPath}");
                        }
                    }
                }

                // Delete the entire gallery folder if it exists and is empty
                if (Directory.Exists(galleryPath))
                {
                    try
                    {
                        Directory.Delete(galleryPath, true);
                        Console.WriteLine($"Deleted gallery folder: {galleryPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Could not delete gallery folder {galleryPath}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting gallery files for gallery {galleryId}: {ex.Message}");
                throw; // Re-throw to ensure transaction rollback
            }
        }

        // Gallery Item methods
        public async Task<List<GalleryItemDTO>> GetGalleryItemsByGalleryIdAsync(int galleryId)
        {
            var items = await _context.GalleryItems
                .Where(gi => gi.IdGallery == galleryId)
                .OrderBy(gi => gi.DisplayOrder)
                .ToListAsync();

            return items.Select(gi => new GalleryItemDTO
            {
                IdGalleryItem = gi.IdGalleryItem,
                IdGallery = gi.IdGallery,
                ImagePath = gi.ImagePath,
                ImageName = gi.ImageName,
                Description = gi.Description,
                DisplayOrder = gi.DisplayOrder
            }).ToList();
        }

        public async Task<GalleryItemDTO> CreateGalleryItemAsync(GalleryItemDTO galleryItemDTO)
        {
            var galleryItem = new GalleryItem
            {
                IdGallery = galleryItemDTO.IdGallery,
                ImagePath = galleryItemDTO.ImagePath,
                ImageName = galleryItemDTO.ImageName,
                Description = galleryItemDTO.Description,
                DisplayOrder = galleryItemDTO.DisplayOrder
            };

            _context.GalleryItems.Add(galleryItem);
            await _context.SaveChangesAsync();

            return new GalleryItemDTO
            {
                IdGalleryItem = galleryItem.IdGalleryItem,
                IdGallery = galleryItem.IdGallery,
                ImagePath = galleryItem.ImagePath,
                ImageName = galleryItem.ImageName,
                Description = galleryItem.Description,
                DisplayOrder = galleryItem.DisplayOrder
            };
        }

        public async Task<GalleryItemDTO> UpdateGalleryItemAsync(int id, GalleryItemDTO galleryItemDTO)
        {
            var galleryItem = await _context.GalleryItems.FindAsync(id);
            if (galleryItem == null) throw new Exception("Gallery item not found");

            galleryItem.ImagePath = galleryItemDTO.ImagePath;
            galleryItem.ImageName = galleryItemDTO.ImageName;
            galleryItem.Description = galleryItemDTO.Description;
            galleryItem.DisplayOrder = galleryItemDTO.DisplayOrder;

            await _context.SaveChangesAsync();

            return new GalleryItemDTO
            {
                IdGalleryItem = galleryItem.IdGalleryItem,
                IdGallery = galleryItem.IdGallery,
                ImagePath = galleryItem.ImagePath,
                ImageName = galleryItem.ImageName,
                Description = galleryItem.Description,
                DisplayOrder = galleryItem.DisplayOrder
            };
        }

        public async Task<bool> DeleteGalleryItemAsync(int id)
        {
            var galleryItem = await _context.GalleryItems.FindAsync(id);
            if (galleryItem == null) return false;

            try
            {
                // Delete file from server before deleting database record
                await DeleteGalleryItemFileAsync(galleryItem);

                // Delete from database
                _context.GalleryItems.Remove(galleryItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting gallery item {id}: {ex.Message}");
                return false;
            }
        }

        private async Task DeleteGalleryItemFileAsync(GalleryItem galleryItem)
        {
            try
            {
                if (!string.IsNullOrEmpty(galleryItem.ImagePath))
                {
                    var galleryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "gallery", galleryItem.IdGallery.ToString());
                    var filePath = Path.Combine(galleryPath, galleryItem.ImagePath);

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Console.WriteLine($"Deleted gallery item file: {filePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting gallery item file for item {galleryItem.IdGalleryItem}: {ex.Message}");
                throw; // Re-throw to ensure transaction rollback
            }
        }

        // Gallery Item Position Management
        public async Task<bool> UpdateGalleryItemPositionAsync(int itemId, int newPosition)
        {
            try
            {
                var item = await _context.GalleryItems.FindAsync(itemId);
                if (item == null) return false;

                var galleryId = item.IdGallery;

                // Get all items in the same gallery, ordered by current DisplayOrder
                var allItems = await _context.GalleryItems
                    .Where(gi => gi.IdGallery == galleryId)
                    .OrderBy(gi => gi.DisplayOrder)
                    .ToListAsync();

                // Find the item to move
                var itemToMove = allItems.FirstOrDefault(i => i.IdGalleryItem == itemId);
                if (itemToMove == null) return false;

                // Get current position (0-based)
                var currentPosition = allItems.IndexOf(itemToMove);
                var targetPosition = newPosition - 1; // Convert to 0-based

                Console.WriteLine($"Moving item {itemId} from position {currentPosition + 1} to position {newPosition}");

                // Remove the item from its current position
                allItems.Remove(itemToMove);

                // Insert it at the new position
                var insertIndex = Math.Max(0, Math.Min(targetPosition, allItems.Count));
                allItems.Insert(insertIndex, itemToMove);

                // Update DisplayOrder for ALL items to reflect new positions
                for (int i = 0; i < allItems.Count; i++)
                {
                    var oldOrder = allItems[i].DisplayOrder;
                    allItems[i].DisplayOrder = i + 1;
                    Console.WriteLine($"Item {allItems[i].IdGalleryItem}: {oldOrder} → {i + 1}");
                }

                await _context.SaveChangesAsync();
                Console.WriteLine($"Successfully updated {allItems.Count} items in gallery {galleryId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating gallery item position: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ReorderGalleryItemsAsync(int galleryId, List<int> itemIds)
        {
            try
            {
                var galleryItems = await _context.GalleryItems
                    .Where(gi => gi.IdGallery == galleryId)
                    .ToListAsync();

                for (int i = 0; i < itemIds.Count; i++)
                {
                    var item = galleryItems.FirstOrDefault(gi => gi.IdGalleryItem == itemIds[i]);
                    if (item != null)
                    {
                        item.DisplayOrder = i + 1;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ClearGalleryItemsAsync(int galleryId)
        {
            try
            {
                var galleryItems = await _context.GalleryItems
                    .Where(gi => gi.IdGallery == galleryId)
                    .ToListAsync();

                _context.GalleryItems.RemoveRange(galleryItems);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<GalleryItemDTO>> CreateGalleryItemsBatchAsync(List<GalleryItemDTO> galleryItems)
        {
            try
            {
                var entities = galleryItems.Select(dto => new GalleryItem
                {
                    IdGallery = dto.IdGallery,
                    ImagePath = dto.ImagePath,
                    ImageName = dto.ImageName,
                    Description = dto.Description,
                    DisplayOrder = dto.DisplayOrder
                }).ToList();

                _context.GalleryItems.AddRange(entities);
                await _context.SaveChangesAsync();

                return entities.Select(entity => new GalleryItemDTO
                {
                    IdGalleryItem = entity.IdGalleryItem,
                    IdGallery = entity.IdGallery,
                    ImagePath = entity.ImagePath,
                    ImageName = entity.ImageName,
                    Description = entity.Description,
                    DisplayOrder = entity.DisplayOrder
                }).ToList();
            }
            catch
            {
                return new List<GalleryItemDTO>();
            }
        }

        public async Task<GalleryItemDTO?> GetGalleryItemByIdAsync(int itemId)
        {
            try
            {
                var item = await _context.GalleryItems
                    .FirstOrDefaultAsync(gi => gi.IdGalleryItem == itemId);

                if (item == null) return null;

                return new GalleryItemDTO
                {
                    IdGalleryItem = item.IdGalleryItem,
                    IdGallery = item.IdGallery,
                    ImagePath = item.ImagePath,
                    ImageName = item.ImageName,
                    Description = item.Description,
                    DisplayOrder = item.DisplayOrder
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<GalleryDTO?> GetGalleryByUrlAsync(string url)
        {
            var gallery = await _context.Galleries
                .Include(g => g.GalleryItems)
                .FirstOrDefaultAsync(g => g.Url == url);

            if (gallery == null) return null;

            return new GalleryDTO
            {
                IdGallery = gallery.IdGallery,
                Title = gallery.Title,
                Url = gallery.Url,
                CreatedDate = gallery.CreatedDate,
                DisplayOrder = gallery.DisplayOrder,
                BannerImagePath = gallery.BannerImagePath,
                BannerImageName = gallery.BannerImageName,
                GalleryItems = gallery.GalleryItems
                    .OrderBy(gi => gi.DisplayOrder)
                    .Select(gi => new GalleryItemDTO
                    {
                        IdGalleryItem = gi.IdGalleryItem,
                        IdGallery = gi.IdGallery,
                        ImagePath = gi.ImagePath,
                        ImageName = gi.ImageName,
                        Description = gi.Description,
                        DisplayOrder = gi.DisplayOrder
                    })
                    .ToList()
            };
        }

        public async Task<bool> IsTitleExistsAsync(string title, int? excludeId = null)
        {
            var query = _context.Galleries.Where(g => g.Title.ToLower() == title.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(g => g.IdGallery != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        // Gallery Position Management
        public async Task<bool> UpdateGalleryPositionAsync(int galleryId, int newPosition)
        {
            try
            {
                var gallery = await _context.Galleries.FindAsync(galleryId);
                if (gallery == null) return false;

                // Get all galleries, ordered by current DisplayOrder
                var allGalleries = await _context.Galleries
                    .OrderBy(g => g.DisplayOrder)
                    .ToListAsync();

                // Find the gallery to move
                var galleryToMove = allGalleries.FirstOrDefault(g => g.IdGallery == galleryId);
                if (galleryToMove == null) return false;

                // Get current position (0-based)
                var currentPosition = allGalleries.IndexOf(galleryToMove);
                var targetPosition = newPosition - 1; // Convert to 0-based

                Console.WriteLine($"Moving gallery {galleryId} from position {currentPosition + 1} to position {newPosition}");

                // Remove the gallery from its current position
                allGalleries.Remove(galleryToMove);

                // Insert it at the new position
                var insertIndex = Math.Max(0, Math.Min(targetPosition, allGalleries.Count));
                allGalleries.Insert(insertIndex, galleryToMove);

                // Update DisplayOrder for ALL galleries to reflect new positions
                for (int i = 0; i < allGalleries.Count; i++)
                {
                    var oldOrder = allGalleries[i].DisplayOrder;
                    allGalleries[i].DisplayOrder = i + 1;
                    Console.WriteLine($"Gallery {allGalleries[i].IdGallery}: {oldOrder} → {i + 1}");
                }

                await _context.SaveChangesAsync();
                Console.WriteLine($"Successfully updated {allGalleries.Count} galleries");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating gallery position: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ReorderGalleriesAsync(List<int> galleryIds)
        {
            try
            {
                var galleries = await _context.Galleries.ToListAsync();

                for (int i = 0; i < galleryIds.Count; i++)
                {
                    var gallery = galleries.FirstOrDefault(g => g.IdGallery == galleryIds[i]);
                    if (gallery != null)
                    {
                        gallery.DisplayOrder = i + 1;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}