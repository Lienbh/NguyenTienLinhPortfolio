using System.Text;
using YoutubeExplode;

namespace nguyentienlink_api.Controllers
{
    public class Xulyvideo
    {
        public Xulyvideo()
        {
            
        }
        public string XulyLinksYoutube(string path)
        {
            var youtube = new YoutubeClient();

            var video = youtube.Videos.GetAsync(path).Result;

            byte[] bytes = Encoding.Default.GetBytes(video.Title);

            string title = Encoding.UTF8.GetString(bytes);

            return title;



        }

    }
}
