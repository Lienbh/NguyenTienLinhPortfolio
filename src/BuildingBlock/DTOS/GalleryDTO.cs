namespace BuildingBlock.DTOS
{
    public class GalleryDTO
    {
        public int IdGallery { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Url { get; set; }
        public DateTime CreatedDate { get; set; }
        // Banner fields
        public string? BannerImagePath { get; set; }
        public string? BannerImageName { get; set; }

        public List<GalleryItemDTO> GalleryItems { get; set; } = new List<GalleryItemDTO>();
    }
}
