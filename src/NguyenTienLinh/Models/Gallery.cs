using System.ComponentModel.DataAnnotations;

namespace NguyenTienLinh.Models
{
    public class Gallery
    {
        [Key]
        public int IdGallery { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Url { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Banner fields
        [StringLength(200)]
        public string? BannerImagePath { get; set; }

        [StringLength(200)]
        public string? BannerImageName { get; set; }

        // Display order for sorting
        public int DisplayOrder { get; set; } = 0;

        // Navigation properties
        public virtual ICollection<GalleryItem> GalleryItems { get; set; } = new List<GalleryItem>();
    }
}
