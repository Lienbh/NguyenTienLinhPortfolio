
using AppApi.IRepository;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;


namespace AppApi.Repository
{
    public class AboutRepo : IAboutsRepos
    {
        private readonly AppDbContext _context;

        public AboutRepo(AppDbContext dBContext)
        {
            _context = dBContext;
        }
        public About AddAbout(About about)
        {
            try
            {
                _context.About.Add(about);
                _context.SaveChanges();
                return about;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public About DeleteAbout(int id)
        {
            var about = _context.About.Find(id);
            try
            {
                if (about != null)
                {
                    _context.About.Remove(about);
                    _context.SaveChanges();
                }
                return about;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public About GetAboutById(int id)
        {
            return _context.About.Find(id);
        }

        public IEnumerable<About> GetAllAbouts()
        {
            return _context.About.ToList();
        }

        public About UpdateAbout(About about)
        {
            try
            {

                var _about = _context.About.Find(about.IdAbout);
                if (_about != null)
                {
                    _about.IdAbout = about.IdAbout;
                    _about.AboutImage = about.AboutImage;
                    _context.About.Update(_about);
                    _context.SaveChanges();
                    
                }
                return about;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
