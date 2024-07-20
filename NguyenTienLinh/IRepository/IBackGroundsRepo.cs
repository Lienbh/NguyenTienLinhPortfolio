



using NguyenTienLinh.Models;

namespace AppApi.IRepository
{
    public interface IBackGroundsRepo
    {
        IEnumerable<BackGround> GetAllBackGrounds();

        BackGround GetBackGroundById(int id);

        BackGround AddBackGround(BackGround backGround);

        BackGround UpdateBackGround(BackGround backGround);

        BackGround DeleteBackGround(int id);
    }
}
