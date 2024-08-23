namespace BuildingBlock.DTOS
{
    public class ManageCategoriesDTO
    {
        public int IdCategories { get; set; }
        public string CategoryName { get; set; }
        public string BrandingImage { get; set; }
        public List<CategoriesDTO> Categories { get; set; }
    }
}
