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
            var checkedCategory = await _context.Category.AnyAsync(x=>x.Name==category.Name && x.UserId == category.UserId);
            if (!checkedCategory)
            {
                await _context.Category.AddAsync(category);
                await _context.SaveChangesAsync();
                return category;
            }
            else {
                return null;
            }
        }

        public async Task<bool> DeleteCategory(int Id , int userId)
        {
            var delete = await _context.Category.FirstOrDefaultAsync(c => c.Id == Id && c.UserId == userId);
            if (delete == null) {
                return false;
            }
            _context.Category.Remove(delete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Category>> GetAllCategories(int userId)    
        {
            return await _context.Category.Where(c=>c.UserId==userId).AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetCategoryById(int Id, int userId)
        {
            var getId = await _context.Category.AsNoTracking().FirstOrDefaultAsync(c => c.Id == Id && c.UserId == userId);
            return getId;
        }

        public async Task<Category?> UpdateCategory(int Id, Category category, int  userId)
        {
            var idChecker = await _context.Category.FirstOrDefaultAsync(c => c.Id == Id && c.UserId == userId);
            if (idChecker == null) {
                return null;
            }
            var checkedCategory = await _context.Category.AnyAsync(x => x.Name == category.Name && x.UserId == userId && x.Id != category.Id);
            if (checkedCategory) {
                return null;
            }
            idChecker.Name = category.Name;
            
            await _context.SaveChangesAsync();
            return idChecker;
        }
    }
}
