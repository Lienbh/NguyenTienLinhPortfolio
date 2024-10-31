



using NguyenTienLinh.Models;

namespace AppApi.IRepository
{
    public interface ICategoriesRepo
    {
        IEnumerable<Categories> GetAllCategories();

        Categories GetCategoryById(int id);

        Categories AddCategory(Categories category);

        Categories UpdateCategory(Categories category);

        Categories DeleteCategory(int id);
    }
}
