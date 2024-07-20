using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;


namespace nguyentienlink_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private AppDbContext _context;

        public UserController(ILogger<UserController> logger)
        {
            logger = _logger;
            _context = new AppDbContext();
        }
        [HttpGet(Name = "UserController")]
        public IEnumerable<User> Get()
        {
            return _context.User.ToList();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public IActionResult Post([FromBody] User user,string Username,string password)
        {
            if (ModelState.IsValid)
            {
                _context.User.Add(user);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }                   


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            var entity = _context.User.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.UserName = user.UserName;
            
            entity.Password = user.Password;
            _context.SaveChanges();
            return Ok();
        }
       
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.User.Remove(user);
            _context.SaveChanges();
            return Ok();
        }
       
    }
}
