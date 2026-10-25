using WalMart_ERP_System.Models;
using WalMart_ERP_System.Results;

namespace WalMart_ERP_System.Interfaces;

public interface IInventoryService
{
    Task<Result<InventoryTransaction>> AdjustStock(InventoryTransaction inventoryTransaction);
    Task<Product?> GetProductStock(int productId);
    Task<List<Product>> GetLowStockProducts();
}
