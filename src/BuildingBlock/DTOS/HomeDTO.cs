namespace BuildingBlock.DTOS
{
    public class HomeDTO
    {
        public List<CategoriesDTO> CategoriesDTOs { get; set; } = new List<CategoriesDTO>();
        public List<BackGroudDTO> BackGroudDTOs { get; set; } = new List<BackGroudDTO>();
    }
}
