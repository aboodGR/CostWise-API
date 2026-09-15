using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CostWise_API.Services
{
    public class CategoryService : ICategoryService
    {
        List<Category> listcategory = new List<Category>();
        int idcounter = 0;
        public Category? AddCategory(Category category)
        {
            
            idcounter++;
            category.Id = idcounter;
            listcategory.Add(category);
            return category;
        }

        public bool DeleteCategory(int Id)
        {
            var delete = listcategory.FirstOrDefault(c => c.Id == Id);
            if (delete == null) {
                return false;
            }
            listcategory.Remove(delete);
            return true;
        }

        public List<Category> GetAllCategories()    
        {
            return listcategory;
        }

        public Category? GetCategoryById(int Id)
        {
            var getId = listcategory.FirstOrDefault(c => c.Id == Id);
            return getId;
        }

        public Category? UpdateCategory(int Id, Category category)
        {
            var idChecker = listcategory.FirstOrDefault(c => c.Id == Id);
            if (idChecker == null) {
                return null;
            }
            category.Id = idChecker.Id;
            listcategory.Remove(idChecker);
            listcategory.Add(category);
            return category;
        }
    }
}
