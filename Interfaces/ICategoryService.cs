using WalMart_ERP_System.DTOs.Category;
using WalMart_ERP_System.Models;
using WalMart_ERP_System.Results;

namespace WalMart_ERP_System.Interfaces;

public interface ICategoryService
{
    Task<Category?> AddCategory(Category category);
    Task<List<Category>> GetAllCategories();
    Task<Category?> GetCategoryById(int id);
    Task<Result<Category>> UpdateCategory(int id, Category category);
    Task<Result<bool>> DeleteCategory(int id);
}
