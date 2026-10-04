using CostWise_API.DTOs.Category;
using CostWise_API.Interfaces;
using CostWise_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostWise_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService category;

        public CategoriesController(ICategoryService category)
        {
            this.category = category;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAc()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var userId = int.Parse(userIdClaim);

            var result = await category.GetAllCategories(userId);

            var response = result.Select(x => new CategoryResponseDto
            {
                Id = x.Id,
                Name = x.Name
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAc(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var userId = int.Parse(userIdClaim);

            var result = await category.GetCategoryById(id, userId);

            if (result == null)
            {
                return NotFound();
            }

            var response = new CategoryResponseDto
            {
                Id = result.Id,
                Name = result.Name
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryAc(CreateCategoryDto createCategoryDto)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var userId = int.Parse(userIdClaim);

            var newCategory = new Category
            {
                Name = createCategoryDto.Name,
                UserId = userId
            };

            var categories = await category.AddCategory(newCategory);

            if (categories == null)
            {
                return BadRequest();
            }

            return Ok(new CategoryResponseDto { Id = categories.Id, Name = categories.Name });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategoryAc(int id, UpdateCategoryDto updateCategoryDto)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var userId = int.Parse(userIdClaim);

            var newCategory = new Category
            {
                Name = updateCategoryDto.Name
            };

            var updated = await category.UpdateCategory(id, newCategory, userId);

            if (!updated.Success)
            {
                if (updated.ErrorCode == "CategoryNotFound")
                    return NotFound(updated.ErrorMessage);
                if (updated.ErrorCode == "DuplicateCategory")
                    return BadRequest(updated.ErrorMessage);
                return Problem("Unexpected category update result.");
            }
            if (updated.Data == null)
                return Problem("Category update returned no data.");

            return Ok(new CategoryResponseDto { Id = updated.Data.Id, Name = updated.Data.Name });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategoryAc(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            var userId = int.Parse(userIdClaim);

            var deleted = await category.DeleteCategory(id, userId);

            if (deleted == false)
                return NotFound();

            return Ok(deleted);
        }
    }
}
