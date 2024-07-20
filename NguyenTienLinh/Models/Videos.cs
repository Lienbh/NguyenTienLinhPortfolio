using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NguyenTienLinh.Models
{
    public class Videos
    {
        [Key]
        public int IdVideo { get; set; }
        public int STT { get; set; }

        public string Title { get; set; }

        public string VideoLinks { get; set; }

       

        [ForeignKey("IdCategories")]

        public int IdCategories { get; set; }

       
        public virtual Categories Categories { get; set; }
    }
}
