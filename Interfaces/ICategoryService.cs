using CostWise_API.Models;

namespace CostWise_API.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        Category? GetCategoryById(int Id);
        Category? AddCategory(Category category);
        Category? UpdateCategory(int Id , Category category);
        bool DeleteCategory(int Id);
    }
}