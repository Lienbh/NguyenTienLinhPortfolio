using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NguyenTienLinh.Models
{
    public class GalleryItem
    {
        [Key]
        public int IdGalleryItem { get; set; }

        [Required]
        public int IdGallery { get; set; }

        [Required]
        [StringLength(200)]
        public string ImagePath { get; set; } = string.Empty;

        [StringLength(200)]
        public string? ImageName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int DisplayOrder { get; set; } = 0;

        // Navigation property
        [ForeignKey("IdGallery")]
        public virtual Gallery Gallery { get; set; } = null!;
    }
}
