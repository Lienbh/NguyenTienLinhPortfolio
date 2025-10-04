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
                .ToListAsync();

            return galleries.Select(g => new GalleryDTO
            {
                IdGallery = g.IdGallery,
                Title = g.Title,
                Description = g.Description,
                CreatedDate = g.CreatedDate,
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
                Description = gallery.Description,
                CreatedDate = gallery.CreatedDate,
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
            var gallery = new Gallery
            {
                Title = galleryDTO.Title,
                Description = galleryDTO.Description,
                BannerImagePath = galleryDTO.BannerImagePath,
                BannerImageName = galleryDTO.BannerImageName
            };

            _context.Galleries.Add(gallery);
            await _context.SaveChangesAsync();

            return new GalleryDTO
            {
                IdGallery = gallery.IdGallery,
                Title = gallery.Title,
                Description = gallery.Description,
                CreatedDate = gallery.CreatedDate,
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
            gallery.Description = galleryDTO.Description;
            gallery.BannerImagePath = galleryDTO.BannerImagePath;
            gallery.BannerImageName = galleryDTO.BannerImageName;

            await _context.SaveChangesAsync();

            return new GalleryDTO
            {
                IdGallery = gallery.IdGallery,
                Title = gallery.Title,
                Description = gallery.Description,
                CreatedDate = gallery.CreatedDate,
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
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null) return false;

            _context.Galleries.Remove(gallery);
            await _context.SaveChangesAsync();
            return true;
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

            _context.GalleryItems.Remove(galleryItem);
            await _context.SaveChangesAsync();
            return true;
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


    }
}