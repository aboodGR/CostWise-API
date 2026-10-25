namespace WalMart_ERP_System.DTOs.Sales;

public class SaleDto
{
    public int Id { get; set; }
    public DateTime DateUtc { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SaleItemDto> Items { get; set; } = new();
}
