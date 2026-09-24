using CostWise_API.Models;

namespace CostWise_API.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<Category?> GetCategoryById(int id);
        Task<Category?> AddCategory(Category category);
        Task<Category?> UpdateCategory(int id, Category category);
        Task<bool> DeleteCategory(int id);
    }
}