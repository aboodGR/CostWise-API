using CostWise_API.Data;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.EntityFrameworkCore;

namespace CostWise_API.Services
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
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategory(int Id)
        {
            var delete = await _context.Category.FirstOrDefaultAsync(c => c.Id == Id);
            if (delete == null) {
                return false;
            }
            _context.Category.Remove(delete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Category>> GetAllCategories()    
        {
            return await _context.Category.AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetCategoryById(int Id)
        {
            var getId = await _context.Category.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id);
            return getId;
        }

        public async Task<Category?> UpdateCategory(int Id, Category category)
        {
            var idChecker = await _context.Category.FirstOrDefaultAsync(c => c.Id == Id);
            if (idChecker == null) {
                return null;
            }
            idChecker.Name = category.Name;
            
            await _context.SaveChangesAsync();
            return idChecker;
        }
    }
}
