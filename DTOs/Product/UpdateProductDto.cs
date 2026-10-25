using System.ComponentModel.DataAnnotations;

namespace WalMart_ERP_System.DTOs.Product;

public class UpdateProductDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    public decimal SellingPrice { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    public bool IsActive { get; set; }
}
