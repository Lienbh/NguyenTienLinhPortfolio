
using AppApi.IRepository;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;

namespace AppApi.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }
        public User AddUser(User user)
        {
            try
            {

                _context.User.Add(user);
                _context.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public User DeleteUser(int id)
        {
            var user = _context.User.Find(id);
            try
            {
                if (user != null)
                {
                    _context.User.Remove(user);
                    _context.SaveChanges();

                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.User.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.User.Find(id);
        }

        public User UpdateUser(User user)
        {
            try
            {
                var userExist = _context.User.Find(user.IdUser);
                if (userExist != null)
                {
                    _context.User.Update(user);
                    _context.SaveChanges();
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
