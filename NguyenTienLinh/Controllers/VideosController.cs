using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NguyenTienLinh.Context;
using NguyenTienLinh.Controllers;
using NguyenTienLinh.Models;
using System.IO;

namespace nguyentienlink_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly ILogger<VideosController> _logger;
        private AppDbContext _context;

        public VideosController(ILogger<VideosController> logger)
        {
            logger = _logger;

            _context = new AppDbContext();
        }
        [HttpGet(Name = "VideosController")]

        public IEnumerable<Videos> Get()
        {
            return _context.Videos.ToList();
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var video = _context.Videos.Find(id);
            if (video == null)
            {
                return NotFound();
            }
            return Ok(video);
        }
       
        [HttpPost]
        public IActionResult Post([FromBody] Videos video, string path,string tilte,int stt,string categoryName)
        {
            Videos videos = new Videos();
            xulytilte xulytilte = new xulytilte();
            Xulyvideo xulyvideo = new Xulyvideo();
            videos.Title = xulytilte.XulyTitle(tilte);
            videos.VideoLinks = xulyvideo.XulyLinksYoutube(path);
            if (ModelState.IsValid)
            {
                _context.Videos.Add(videos);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();

        }



        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Videos video)
        {
            var entity = _context.Videos.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.STT = video.STT;
            entity.VideoLinks = video.VideoLinks;
            entity.Title = video.Title;
            entity.IdCategories = video.IdCategories;
           
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var video = _context.Videos.Find(id);
            if (video == null)
            {
                return NotFound();
            }
            _context.Videos.Remove(video);
            _context.SaveChanges();
            return Ok();
        }
       

    }
}
