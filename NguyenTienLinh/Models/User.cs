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

    //Lien
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
