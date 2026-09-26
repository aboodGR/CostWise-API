using CostWise_API.DTOs.Category;
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
            var result = await category.GetAllCategories();
            var response = result.Select( x => new CategroyResponseDto {
                Id = x.Id,
                Name = x.Name
            });
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAc(int id) {
            var result = await category.GetCategoryById(id);
            if (result == null) {
                return NotFound();
            }
            var response = new CategroyResponseDto {
                Id = result.Id,
                Name = result.Name
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategoryAc(UpdateCategoryDtos updateCategoryDtos) {
            var newCategory = new Category {
                Name = updateCategoryDtos.Name
            };
            var categroies = await category.AddCategory(newCategory);
            if (categroies == null) {
                return BadRequest();
            }
            return Ok(categroies);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategoryAc(int id , CreateCategoryDto createCategoryDto) {
            var newCategory = new Category
            {
                Name = createCategoryDto.Name
            };
            var updated = await this.category.UpdateCategory(id, newCategory);
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
