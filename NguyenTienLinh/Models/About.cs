using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenTienLinh.Models;

[Table("About")]
public partial class About
{
    [Key]
    public int IdAbout { get; set; }

    public string AboutImage { get; set; } = null!;
}
