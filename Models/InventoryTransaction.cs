using System.ComponentModel.DataAnnotations;

namespace WalMart_ERP_System.Models;

public class InventoryTransaction
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    public InventoryTransactionType Type { get; set; }

    [Required, MaxLength(500)]
    public required string Reason { get; set; }

    public DateTime DateUtc { get; set; } = DateTime.UtcNow;
}

public enum InventoryTransactionType
{
    StockIn = 1,
    StockOut = 2
}
