using System.ComponentModel.DataAnnotations;

namespace WalMart_ERP_System.DTOs.Sales;

public class CreateSaleDto
{
    [Required]
    [MinLength(1)]
    public List<CreateSaleItemDto> Items { get; set; } = new();
}

public class CreateSaleItemDto
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
