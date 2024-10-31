
using AppApi.IRepository;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;

namespace AppApi.Repository
{
    public class VideoRepo : IVideosRepo
    {
        private readonly AppDbContext _context;

        public VideoRepo(AppDbContext dBContext)
        {
            _context = dBContext;
        }
        public Videos AddVideo(Videos video)
        {
            try
            {
                _context.Videos.Add(video);
                _context.SaveChanges();
                return video;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public Videos DeleteVideo(int id)
        {
            var video = _context.Videos.Find(id);
            try
            {
                if(video != null)
                {
                    _context.Videos.Remove(video);
                    _context.SaveChanges();
                }
                return video;
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        public IEnumerable<Videos> GetAllVideos()
        {
            return _context.Videos.ToList();
        }

        public Videos GetVideoById(int id)
        {
            return _context.Videos.Find(id);
        }

        public Videos UpdateVideo(Videos video)
        {
            try
            {
                var videoExist = _context.Videos.Find(video.IdVideo);
                if (videoExist != null)
                {
                   
                    videoExist.STT = video.STT;
                    videoExist.Title = video.Title;
                    videoExist.VideoLinks = video.VideoLinks;
                    videoExist.IdCategories = video.IdCategories;
                    videoExist.Categories = video.Categories;

                    _context.Videos.Update(videoExist);
                    _context.SaveChanges();
                }
                return videoExist;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
