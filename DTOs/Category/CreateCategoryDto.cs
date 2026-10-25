using System.ComponentModel.DataAnnotations;

namespace CostWise_API.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

    }
}
