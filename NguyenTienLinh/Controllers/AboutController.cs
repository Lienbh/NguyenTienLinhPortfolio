using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NguyenTienLinh.Context;
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
        //Tôi muốn tạo mới 1 đối tượng About
        [HttpPost()]
        public IActionResult Post([FromBody] About about,string path)
        {
            about = new About();
            Xulyanh xl = new Xulyanh();
            about.AboutImage = xl.Xuly(path);
            if (ModelState.IsValid)
            {
                _context.About.Add(about);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();

        }
       
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] About about)
        {
            var entity = _context.About.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.IdAbout = about.IdAbout;
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
