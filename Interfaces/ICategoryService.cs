using CostWise_API.Models;

using CostWise_API.Results;

namespace CostWise_API.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories(int userId);
        Task<Category?> GetCategoryById(int id, int userId);
        Task<Category?> AddCategory(Category category);
        Task<Result<Category>> UpdateCategory(int id, Category category, int userId);
        Task<bool> DeleteCategory(int id, int userId);
    }
}
