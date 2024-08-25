using BuildingBlock.DTOS;
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
        private readonly AppDbContext _context;

        public UserController(ILogger<UserController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
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
        public IActionResult CreateUser([FromBody] UserDTO user)
        {
            if (ModelState.IsValid)
            {
                User userCreate = new User();
                userCreate.UserName = user.UserName;
                userCreate.Password = user.Password;
                _context.User.Add(userCreate);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO user)
        {

            if (ModelState.IsValid)
            {
                var checkUser = _context.User.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
                if (checkUser == null)
                {
                    return NotFound("Thông tin tài khoản hoặc mật khẩu không chính xác");
                }
                else
                {
                    return Ok("Đăng nhập thành công");
                }


            }
            return Content("Đăng nhập thất bại");
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDTO user)
        {
            var entity = _context.User.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.UserName = user.UserName;

            entity.Password = user.Password;
            _context.User.Update(entity);
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
