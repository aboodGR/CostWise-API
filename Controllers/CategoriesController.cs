using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalMart_ERP_System.DTOs.Category;
using WalMart_ERP_System.Interfaces;
using WalMart_ERP_System.Models;

namespace WalMart_ERP_System.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAc()
        {
            var categories = await _categoryService.GetAllCategories();

            var response = categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAc(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryAc(CreateCategoryDto createCategoryDto)
        {
            var newCategory = new Category
            {
                Name = createCategoryDto.Name
            };

            var category = await _categoryService.AddCategory(newCategory);
            if (category == null)
            {
                return BadRequest("A category with this name already exists.");
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAc(int id, UpdateCategoryDto updateCategoryDto)
        {
            var category = new Category
            {
                Name = updateCategoryDto.Name
            };

            var updated = await _categoryService.UpdateCategory(id, category);
            if (!updated.Success)
            {
                if (updated.ErrorCode == "CategoryNotFound")
                {
                    return NotFound(updated.ErrorMessage);
                }

                if (updated.ErrorCode == "DuplicateCategory")
                {
                    return BadRequest(updated.ErrorMessage);
                }

                return Problem("Unexpected category update result.");
            }

            if (updated.Data == null)
            {
                return Problem("Category update returned no data.");
            }

            var response = new CategoryDto
            {
                Id = updated.Data.Id,
                Name = updated.Data.Name
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAc(int id)
        {
            var deleted = await _categoryService.DeleteCategory(id);
            if (!deleted.Success)
            {
                if (deleted.ErrorCode == "CategoryNotFound")
                {
                    return NotFound(deleted.ErrorMessage);
                }

                if (deleted.ErrorCode == "CategoryHasProducts")
                {
                    return Conflict(deleted.ErrorMessage);
                }

                return Problem("Unexpected category delete result.");
            }

            return NoContent();
        }
    }
}
