using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenTienLinh.Models;

[Table("BackGround")]
public partial class BackGround
{
    [Key]
    public int IdBackGround { get; set; }
    [Required]
    public string Image { get; set; }

}
 