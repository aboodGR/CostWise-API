using System.ComponentModel.DataAnnotations;

namespace WalMart_ERP_System.DTOs.Category;

public class CreateCategoryDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
