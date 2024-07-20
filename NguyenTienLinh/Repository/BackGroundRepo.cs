
using AppApi.IRepository;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;

namespace AppApi.Repository
{
    public class BackGroundRepo : IBackGroundsRepo
    {
        private readonly AppDbContext _context;

        public BackGroundRepo(AppDbContext context)
        {
            _context = context;
        }
        public BackGround AddBackGround(BackGround backGround)
        {
            try
            {

                _context.BackGround.Add(backGround);
                _context.SaveChanges();
                return backGround;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public BackGround DeleteBackGround(int id)
        {
            var backGround = _context.BackGround.Find(id);
            try
            {
                if (backGround != null)
                {
                    _context.BackGround.Remove(backGround);
                    _context.SaveChanges();

                }
                return backGround;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<BackGround> GetAllBackGrounds()
        {
            return _context.BackGround.ToList();
        }

        public BackGround GetBackGroundById(int id)
        {
            return _context.BackGround.Find(id);
        }

        public BackGround UpdateBackGround(BackGround backGround)
        {
            try
            {
                var bg = _context.BackGround.Find(backGround.IdBackGround);
                if (bg != null)
                {
                    bg.IdBackGround = backGround.IdBackGround;
                    bg.Image = backGround.Image;
                    _context.Update(bg);
                    _context.SaveChanges();
                }
                return bg;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
