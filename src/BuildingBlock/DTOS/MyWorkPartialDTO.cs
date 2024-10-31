namespace BuildingBlock.DTOS
{
    public class MyWorkPartialDTO
    {
        public string Heading { get; set; }
        public List<VideoDTO> videoDTOs { get; set; }
    }

    public class VideoDTO
    {
        public int? IdVideo { get; set; }
        public int? STT { get; set; }

        public string Title { get; set; }

        public string VideoLinks { get; set; }

        public int? IdCategories { get; set; }
    }
}
