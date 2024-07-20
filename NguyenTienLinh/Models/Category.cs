using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenTienLinh.Models;

public partial class Category
{
    [Key]
    public int IdCategories { get; set; }

    public string CategoryName { get; set; } = null!;

    [InverseProperty("CategoriesIdCategoriesNavigation")]
    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
