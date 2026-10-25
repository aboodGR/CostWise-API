using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WalMart_ERP_System.Data;
using WalMart_ERP_System.Interfaces;
using WalMart_ERP_System.Models;
using WalMart_ERP_System.Results;

namespace WalMart_ERP_System.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> AddCategory(Category category)
        {
            category.Name = category.Name.Trim();

            var categoryExists = await _context.Categories
                .AnyAsync(x => x.Name == category.Name);
            if (categoryExists)
            {
                return null;
            }

            await _context.Categories.AddAsync(category);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                if (exception.InnerException is SqlException sqlException
                    && (sqlException.Number == 2601 || sqlException.Number == 2627))
                {
                    return null;
                }

                throw;
            }

            return category;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Result<Category>> UpdateCategory(int id, Category category)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existingCategory == null)
            {
                return new Result<Category>
                {
                    Success = false,
                    ErrorCode = "CategoryNotFound",
                    ErrorMessage = "Category was not found."
                };
            }

            category.Name = category.Name.Trim();

            var categoryExists = await _context.Categories
                .AnyAsync(x => x.Name == category.Name && x.Id != id);
            if (categoryExists)
            {
                return new Result<Category>
                {
                    Success = false,
                    ErrorCode = "DuplicateCategory",
                    ErrorMessage = "A category with this name already exists."
                };
            }

            existingCategory.Name = category.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                if (exception.InnerException is SqlException sqlException
                    && (sqlException.Number == 2601 || sqlException.Number == 2627))
                {
                    return new Result<Category>
                    {
                        Success = false,
                        ErrorCode = "DuplicateCategory",
                        ErrorMessage = "A category with this name already exists."
                    };
                }

                throw;
            }

            return new Result<Category>
            {
                Success = true,
                Data = existingCategory
            };
        }

        public async Task<Result<bool>> DeleteCategory(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    ErrorCode = "CategoryNotFound",
                    ErrorMessage = "Category was not found."
                };
            }

            var hasProducts = await _context.Products
                .AnyAsync(x => x.CategoryId == id);
            if (hasProducts)
            {
                return new Result<bool>
                {
                    Success = false,
                    ErrorCode = "CategoryHasProducts",
                    ErrorMessage = "The category cannot be deleted while products use it."
                };
            }

            _context.Categories.Remove(category);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                if (exception.InnerException is SqlException sqlException
                    && sqlException.Number == 547)
                {
                    return new Result<bool>
                    {
                        Success = false,
                        ErrorCode = "CategoryHasProducts",
                        ErrorMessage = "The category cannot be deleted while products use it."
                    };
                }

                throw;
            }

            return new Result<bool>
            {
                Success = true,
                Data = true
            };
        }
    }
}
