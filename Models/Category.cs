using System.ComponentModel.DataAnnotations;

namespace WalMart_ERP_System.Models;

public class Category
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public required string Name { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
