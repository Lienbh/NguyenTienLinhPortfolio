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
        public string? Description { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Banner fields
        [StringLength(200)]
        public string? BannerImagePath { get; set; }

        [StringLength(200)]
        public string? BannerImageName { get; set; }

        // Navigation properties
        public virtual ICollection<GalleryItem> GalleryItems { get; set; } = new List<GalleryItem>();
    }
}
