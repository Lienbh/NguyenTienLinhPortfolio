using Microsoft.AspNetCore.Http;
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
        private AppDbContext _context;

        public CategoriesController(ILogger<CategoriesController> logger)
        {
            logger = _logger;
            _context = new AppDbContext();
        }
        [HttpGet(Name = "CategoriesController")]
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
       
        [HttpPost]
        public IActionResult Post([FromBody] Categories category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Categories  category)
        {
            var entity = _context.Categories.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.CategoryName = category.CategoryName;
            
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
