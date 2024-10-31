using System.ComponentModel.DataAnnotations;

namespace NguyenTienLinh.Models
{
    public class Categories
    {
        [Key]
        public int IdCategories { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string BrandingImage { get; set; }

        //tôi muốn thể hiện mối quan hệ n-n giữa bảng Videos và bảng Categories
        public virtual List<Videos> Videos { get; set; }


    }
}
