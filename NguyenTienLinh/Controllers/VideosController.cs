using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;

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
        [HttpGet]

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


        [HttpGet("get-video-by-categoryid/{id}")]
        public IActionResult GetVideoByCategoryId(int id)
        {
            ManageVideoDTO manageVideoDTO = new ManageVideoDTO();
            manageVideoDTO.IdCategories = id;
            var category = _context.Categories.FirstOrDefault(c => c.IdCategories == id);
            if (category == null)
            {
                return NotFound();
            }
            manageVideoDTO.CategoriesName = category.CategoryName;
            manageVideoDTO.Videos = new List<VideoDTO>();
            var videos = _context.Videos.Where(c => c.IdCategories == id).ToList().OrderBy(c => c.STT).ThenBy(c => c.IdVideo);
            foreach (var item in videos)
            {
                VideoDTO videoDTO = new VideoDTO();
                videoDTO.IdVideo = item.IdVideo;
                videoDTO.STT = item.STT;
                videoDTO.Title = item.Title;
                videoDTO.VideoLinks = item.VideoLinks;
                videoDTO.IdCategories = item.IdCategories;
                manageVideoDTO.Videos.Add(videoDTO);
            }
            return Ok(manageVideoDTO);
        }
        [HttpPost]
        public IActionResult CreateVideo(VideoDTO video)
        {
            if (ModelState.IsValid)
            {
                var lstVideoAfterSTT = _context.Videos.AsNoTracking().Where(c => c.IdCategories == video.IdCategories
               && c.STT >= video.STT && c.IdVideo != video.IdVideo).ToList();
                foreach (var x in lstVideoAfterSTT)
                {
                    _context.ChangeTracker.Clear();
                    x.STT += 1;
                    _context.Videos.Update(x);
                    _context.SaveChanges();
                }

                Videos videoCreate = new Videos();
                videoCreate.STT = (int)video.STT;
                videoCreate.VideoLinks = video.VideoLinks;
                videoCreate.IdCategories = (int)video.IdCategories;
                videoCreate.Title = video.Title;
                _context.Videos.Add(videoCreate);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] VideoDTO video)
        {
            var entity = _context.Videos.Find(id);
            if (entity == null)
            {
                return NotFound();
            }

            if (entity.STT != video.STT)
            {
                var lstVideoAfterSTT = _context.Videos.AsNoTracking().Where(c => c.IdCategories == video.IdCategories
                && c.STT >= video.STT && c.IdVideo != video.IdVideo).ToList();
                foreach (var x in lstVideoAfterSTT)
                {
                    _context.ChangeTracker.Clear();
                    x.STT += 1;
                    _context.Videos.Update(x);
                    _context.SaveChanges();
                }
            }
            entity.STT = (int)video.STT;
            entity.VideoLinks = video.VideoLinks;
            entity.Title = video.Title;
            entity.IdCategories = (int)video.IdCategories;
            _context.Update(entity);
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
