using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalMart_ERP_System.Models;

public class Product
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public required string Name { get; set; }

    [Required, MaxLength(100)]
    public required string Sku { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SellingPrice { get; set; }

    public bool IsActive { get; set; } = true;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int StockQuantity { get; set; }
    public int LowStockThreshold { get; set; }

    public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
}
