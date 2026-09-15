using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CostWise_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService category;
        public CategoriesController(ICategoryService category)
        {
            this.category = category;
        }

        [HttpGet]
        public IActionResult GetAllCategoriesAc() {
            return Ok(category.GetAllCategories());
        }
        [HttpGet("{id}")]
        public IActionResult GetCategoryByIdAc(int id) {
            return Ok(category.GetCategoryById(id));
        }
        [HttpPost]
        public IActionResult AddCategoryAc(Category category) {
            return Ok(this.category.AddCategory(category));
        }
        [HttpPut]
        public IActionResult UpdateCategoryAc(int id , Category category) {
            var updated = this.category.UpdateCategory(id,category);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }
        [HttpDelete]
        public IActionResult DeleteCategoryAc(int id) {
            var deleted = this.category.DeleteCategory(id);
            if (deleted == false)
                return NotFound();
            return Ok(deleted);
        }

    }
}
