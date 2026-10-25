using System.ComponentModel.DataAnnotations.Schema;

namespace WalMart_ERP_System.Models;

public class Sale
{
    public int Id { get; set; }

    public DateTime DateUtc { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public ICollection<SaleItem> Items { get; set; } = new List<SaleItem>();
}
