using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NguyenTienLinh.Models;

[Index("CategoriesIdCategories", Name = "IX_Videos_CategoriesIdCategories")]
public partial class Video
{
    [Key]
    public int IdVideo { get; set; }

    [Column("STT")]
    public int Stt { get; set; }

    public string Title { get; set; } = null!;

    public string VideoLinks { get; set; } = null!;

    public int IdCategories { get; set; }

    public int CategoriesIdCategories { get; set; }

    [ForeignKey("CategoriesIdCategories")]
    [InverseProperty("Videos")]
    public virtual Category CategoriesIdCategoriesNavigation { get; set; } = null!;
}
