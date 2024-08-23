using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NguyenTienLinh.Models;

[Table("BackGround")]
public partial class BackGround
{
    [Key]
    public int IdBackGround { get; set; }
    [Required]
    public string Image { get; set; }
    [Required]
    public int TimeInterval { get; set; } = 3000;

}
