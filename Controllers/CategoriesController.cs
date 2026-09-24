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
        public async Task<IActionResult> GetAllCategoriesAc() {
            return Ok(await category.GetAllCategories());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAc(int id) {
            var getId = await category.GetCategoryById(id);
            if (getId == null) {
                return NotFound();
            }
            return Ok(getId);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategoryAc(Category category) {
            return Ok(await this.category.AddCategory(category));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategoryAc(int id , Category category) {
            var updated = await this.category.UpdateCategory(id,category);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryAc(int id) {
            var deleted = await this.category.DeleteCategory(id);
            if (deleted == false)
                return NotFound();
            return Ok(deleted);
        }

    }
}
