using System.ComponentModel.DataAnnotations;
using WalMart_ERP_System.Models;

namespace WalMart_ERP_System.DTOs.Inventory;

public class InventoryAdjustmentDto
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [EnumDataType(typeof(InventoryTransactionType))]
    public InventoryTransactionType Type { get; set; }

    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;
}
