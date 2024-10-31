using System.ComponentModel.DataAnnotations;

namespace BuildingBlock.DTOS
{
	public class AboutDTO
	{


		public int? IdAbout { get; set; }
		[Required]
		public string AboutImage { get; set; }
	}
}
