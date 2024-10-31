using System.ComponentModel.DataAnnotations;

namespace BuildingBlock.DTOS
{
    public class CategoriesDTO
    {

        public int IdCategories { get; set; }
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string BrandingImage { get; set; }
    }
}
