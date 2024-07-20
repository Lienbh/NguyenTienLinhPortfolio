

using NguyenTienLinh.Models;

namespace AppApi.IRepository
{
    public interface IVideosRepo
    {
        IEnumerable<Videos> GetAllVideos();

        Videos GetVideoById(int id);

        Videos AddVideo(Videos video);

        Videos UpdateVideo(Videos video);

        Videos DeleteVideo(int id);
    }
}
