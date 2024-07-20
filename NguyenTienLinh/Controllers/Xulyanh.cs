using System.Collections;

namespace nguyentienlink_api.Controllers
{
    public class Xulyanh
    {
        
        public Xulyanh()
        {

            
            
        }
        public string  Xuly(string path) {
            // Đường dẫn của tệp hình ảnh
           

            // Đọc tệp hình ảnh thành một mảng byte
            byte[] imageBytes = File.ReadAllBytes(path);
            string base64String = Convert.ToBase64String(imageBytes);
            

            // Sử dụng mảng byte ở đây
            // Ví dụ: bạn có thể gửi nó qua mạng, lưu vào cơ sở dữ liệu, hoặc thực hiện các xử lý khác

            // In ra kích thước của mảng byte để kiểm tra
            Console.WriteLine("Size of image byte array: " + imageBytes.Length);

            return base64String;
        }
       
    }
}
