using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;


namespace nguyentienlink_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly AppDbContext _context;

        public CategoriesController(ILogger<CategoriesController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet()]
        public IEnumerable<Categories> Get()
        {
            return _context.Categories.ToList();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet("get-detail/{id}")]
        public MyWorkPartialDTO GetDetail(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return new MyWorkPartialDTO();
            }
            MyWorkPartialDTO myWorkPartialDTO = new MyWorkPartialDTO();
            myWorkPartialDTO.Heading = category.CategoryName;
            var listVideo = _context.Videos.Where(c => c.IdCategories == id).ToList().OrderBy(c => c.STT).ThenBy(c => c.IdVideo);
            myWorkPartialDTO.videoDTOs = new List<VideoDTO>();
            if (listVideo.Count() > 0)
            {

                foreach (var video in listVideo)
                {
                    VideoDTO videoDTO = new VideoDTO();
                    videoDTO.Title = video.Title;
                    videoDTO.VideoLinks = video.VideoLinks;
                    videoDTO.IdVideo = video.IdVideo;
                    myWorkPartialDTO.videoDTOs.Add(videoDTO);
                }
            }
            return myWorkPartialDTO;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CategoriesDTO category)
        {
            if (ModelState.IsValid)
            {
                Categories categoryCreate = new Categories();
                categoryCreate.CategoryName = category.CategoryName;
                categoryCreate.BrandingImage = category.BrandingImage;
                _context.Categories.Add(categoryCreate);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CategoriesDTO category)
        {
            var entity = _context.Categories.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.CategoryName = category.CategoryName;
            entity.BrandingImage = category.BrandingImage;
            _context.Categories.Update(entity);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok();
        }
    }
}
