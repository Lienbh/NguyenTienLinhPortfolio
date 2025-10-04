namespace BuildingBlock.DTOS
{
    public class GalleryItemDTO
    {
        public int IdGalleryItem { get; set; }
        public int IdGallery { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string? ImageName { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
