using WalMart_ERP_System.Models;

namespace WalMart_ERP_System.DTOs.Inventory;

public class InventoryTransactionDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public InventoryTransactionType Type { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime DateUtc { get; set; }
}
