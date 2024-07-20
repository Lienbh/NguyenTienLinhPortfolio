using System.ComponentModel.DataAnnotations;

namespace NguyenTienLinh.Models
{
    public class Categories
    {
        [Key]
        public int IdCategories { get; set; }
        public string CategoryName { get; set; }

        //tôi muốn thể hiện mối quan hệ n-n giữa bảng Videos và bảng Categories
     public virtual ICollection<Videos> Videos { get; set; } = new List<Videos>();


    }
}
