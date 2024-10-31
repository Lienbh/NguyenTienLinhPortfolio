
using NguyenTienLinh.Models;

namespace AppApi.IRepository
{
    public interface IUserRepo
    {
        IEnumerable<User> GetAllUsers();

        User GetUserById(int id);

        User AddUser(User user);

        User UpdateUser(User user);

        User DeleteUser(int id);
    }
}
