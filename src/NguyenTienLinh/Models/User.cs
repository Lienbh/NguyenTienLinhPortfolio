using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenTienLinh.Models;

[Table("User")]
public partial class User
{
    [Key]
    public int IdUser { get; set; }

    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}
