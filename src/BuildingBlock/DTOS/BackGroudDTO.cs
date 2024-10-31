using System.ComponentModel.DataAnnotations;

namespace BuildingBlock.DTOS
{
    public class BackGroudDTO
    {
        public int? IdBackGround { get; set; }
        [Required]
        public string Image { get; set; }

        [Required]
        public int TimeInterval { get; set; }
    }
}
