using System.ComponentModel.DataAnnotations;

namespace BuildingBlock.DTOS
{
    public class UserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
