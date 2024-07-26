using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NguyenTienLinh.Context;
using NguyenTienLinh.DTOS;
using NguyenTienLinh.Models;
using nguyentienlink_api.Controllers;

namespace NguyenTienLinh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly ILogger<AboutController> _logger;
        private AppDbContext _context;
        public AboutController(ILogger<AboutController> logger)
        {
            logger = _logger;
            _context = new AppDbContext();
        }

        [HttpGet(Name = "AboutController")]
        public IEnumerable<About> Get()
        {
            return _context.About.ToList();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            
            var about = _context.About.Find(id);
            if (about == null)
            {
                return NotFound();
            }
            return Ok(about);
        }

        [HttpGet("get-by-aboutImage/{aboutImage}")]

        public IActionResult Get(string aboutImage)
        {

            var about = _context.About.Where(c => c.AboutImage.StartsWith(aboutImage)).ToList();
            var aboutF = _context.About.FirstOrDefault(c => c.AboutImage.Contains(aboutImage));
            if (about == null)
            {
                return NotFound();
            }
            return Ok(about);
        }

        //Tôi muốn tạo mới 1 đối tượng About
        [HttpPost()]
        public IActionResult Post([FromBody] AboutDTO about)
        {
          
            //Xulyanh xl = new Xulyanh();
            //about.AboutImage = xl.Xuly(about.Path);
            if (ModelState.IsValid)
            {
                About aboutCreate = new About();
                aboutCreate.AboutImage= about.AboutImage;
                _context.About.Add(aboutCreate);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();

        }
       
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AboutDTO about)
        {
            var entity = _context.About.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.AboutImage = about.AboutImage;
            _context.About.Update(entity);
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var about = _context.About.Find(id);
            if (about == null)
            {
                return NotFound();
            }
            _context.About.Remove(about);
            _context.SaveChanges();
            return Ok();
        }
        


    }
}
