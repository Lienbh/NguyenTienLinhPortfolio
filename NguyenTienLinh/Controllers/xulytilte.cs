using System.Text;

namespace NguyenTienLinh.Controllers
{
    public class xulytilte
    {
        public xulytilte()
        {
                
        }
        //cho phép tiêu đề được nhập tiếng việt

        public string XulyTitle(string title)
        {
            //chuyển tiêu đề sang utf8
            byte[] bytes = Encoding.Default.GetBytes(title);
            title = Encoding.UTF8.GetString(bytes);
            return title;
        }

    }
}
