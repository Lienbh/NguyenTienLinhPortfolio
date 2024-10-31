
using AppApi.IRepository;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;

namespace AppApi.Repository
{
    public class CategoriesRepos : ICategoriesRepo
    {
        private readonly AppDbContext _context;

        public CategoriesRepos(AppDbContext context)
        {
            _context = context;
        }
        public Categories AddCategory(Categories category)
        {
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Categories DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            try
            {
                if(category != null)
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                    
                }
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Categories> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public Categories GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public Categories UpdateCategory(Categories category)
        {
            try
            {
                var findCategory = _context.Categories.Find(category.IdCategories);
                if(findCategory != null)
                {
                    findCategory.IdCategories = category.IdCategories;
                    findCategory.CategoryName = category.CategoryName;
                    findCategory.Videos = category.Videos;
                    _context.Categories.Update(findCategory);
                    _context.SaveChanges();
                }
                return findCategory;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
