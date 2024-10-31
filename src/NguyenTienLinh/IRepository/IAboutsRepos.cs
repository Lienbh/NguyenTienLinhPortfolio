



using NguyenTienLinh.Models;

namespace AppApi.IRepository
{
    public interface IAboutsRepos
    {
        IEnumerable<About> GetAllAbouts();

        About GetAboutById(int id);

        About AddAbout(About about);

        About UpdateAbout(About about);

        About DeleteAbout(int id);
    }
}
