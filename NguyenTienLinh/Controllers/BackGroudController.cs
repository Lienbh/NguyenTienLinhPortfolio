using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;
using nguyentienlink_api.Controllers;

namespace NguyenTienLinh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackGroudController : ControllerBase
    {
        private readonly ILogger<BackGroudController> _logger;
        private AppDbContext _context;
        public BackGroudController(ILogger<BackGroudController> logger)
        {
            logger = _logger;
            _context = new AppDbContext();
        }
        [HttpGet(Name = "BackgroudController")]
        public IEnumerable<BackGround> Get()
        {
            return _context.BackGround.ToList();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var backgroud = _context.BackGround.Find(id);
            if (backgroud == null)
            {
                return NotFound();
            }
            return Ok(backgroud);
        }
        [HttpPost]

        public IActionResult Post([FromBody] BackGround backgroud, string path)
        {

            backgroud = new BackGround();

            Xulyanh xl = new Xulyanh();



            backgroud.Image = xl.Xuly(path);







            // Post byte[] to database
            if (ModelState.IsValid)
            {
                _context.BackGround.Add(backgroud);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] BackGround backgroud)
        {
            var entity = _context.BackGround.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Image = backgroud.Image;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var backgroud = _context.BackGround.Find(id);
            if (backgroud == null)
            {
                return NotFound();
            }
            _context.BackGround.Remove(backgroud);
            _context.SaveChanges();
            return Ok();
        }

    }
}
