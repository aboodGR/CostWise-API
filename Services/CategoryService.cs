using CostWise_API.Data;
using CostWise_API.Interfaces;
using CostWise_API.Models;

namespace CostWise_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Category? AddCategory(Category category)
        {
            _context.Category.Add(category);
            _context.SaveChanges();
            return category;
        }

        public bool DeleteCategory(int Id)
        {
            var delete = _context.Category.FirstOrDefault(c => c.Id == Id);
            if (delete == null) {
                return false;
            }
            _context.Category.Remove(delete);
            _context.SaveChanges();

            return true;
        }

        public List<Category> GetAllCategories()    
        {
            return _context.Category.ToList();
        }

        public Category? GetCategoryById(int Id)
        {
            var getId = _context.Category.FirstOrDefault(c => c.Id == Id);
            return getId;
        }

        public Category? UpdateCategory(int Id, Category category)
        {
            var idChecker = _context.Category.FirstOrDefault(c => c.Id == Id);
            if (idChecker == null) {
                return null;
            }
            idChecker.Name = category.Name;
            
            _context.SaveChanges();
            return idChecker;
        }
    }
}
