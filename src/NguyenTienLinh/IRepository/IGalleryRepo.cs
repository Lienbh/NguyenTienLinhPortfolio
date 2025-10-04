using BuildingBlock.DTOS;

namespace AppApi.IRepository
{
    public interface IGalleryRepo
    {
        Task<List<GalleryDTO>> GetAllGalleriesAsync();
        Task<GalleryDTO?> GetGalleryByIdAsync(int id);
        Task<GalleryDTO> CreateGalleryAsync(GalleryDTO galleryDTO);
        Task<GalleryDTO?> UpdateGalleryAsync(int id, GalleryDTO galleryDTO);
        Task<bool> DeleteGalleryAsync(int id);
        Task<List<GalleryItemDTO>> GetGalleryItemsByGalleryIdAsync(int galleryId);
        Task<GalleryItemDTO> CreateGalleryItemAsync(GalleryItemDTO galleryItemDTO);
        Task<bool> DeleteGalleryItemAsync(int id);

        // Gallery Item Position Management
        Task<bool> UpdateGalleryItemPositionAsync(int itemId, int newPosition);
        Task<bool> ReorderGalleryItemsAsync(int galleryId, List<int> itemIds);
        Task<bool> ClearGalleryItemsAsync(int galleryId);
        Task<List<GalleryItemDTO>> CreateGalleryItemsBatchAsync(List<GalleryItemDTO> galleryItems);

        // New methods for granular operations
        Task<GalleryItemDTO?> GetGalleryItemByIdAsync(int itemId);
        Task<GalleryItemDTO> UpdateGalleryItemAsync(int itemId, GalleryItemDTO item);
    }
}
