namespace BuildingBlock.DTOS
{
    public class ManageVideoDTO
    {
        public string CategoriesName { get; set; }
        public int? IdCategories { get; set; }
        public List<VideoDTO> Videos { get; set; }
    }
}
