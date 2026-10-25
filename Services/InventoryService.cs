using Microsoft.EntityFrameworkCore;
using WalMart_ERP_System.Data;
using WalMart_ERP_System.Interfaces;
using WalMart_ERP_System.Models;
using WalMart_ERP_System.Results;

namespace WalMart_ERP_System.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<InventoryTransaction>> AdjustStock(InventoryTransaction inventoryTransaction)
        {
            if (inventoryTransaction.ProductId <= 0 || inventoryTransaction.Quantity <= 0
                || !Enum.IsDefined(inventoryTransaction.Type))
            {
                return new Result<InventoryTransaction>
                {
                    Success = false,
                    ErrorCode = "InvalidAdjustment",
                    ErrorMessage = "Product, quantity, and transaction type must be valid."
                };
            }

            inventoryTransaction.Reason = inventoryTransaction.Reason.Trim();
            if (string.IsNullOrWhiteSpace(inventoryTransaction.Reason))
            {
                return new Result<InventoryTransaction>
                {
                    Success = false,
                    ErrorCode = "InvalidAdjustment",
                    ErrorMessage = "A reason is required for the stock adjustment."
                };
            }

            await using var databaseTransaction = await _context.Database.BeginTransactionAsync();

            int updatedRows;
            if (inventoryTransaction.Type == InventoryTransactionType.StockIn)
            {
                updatedRows = await _context.Products
                    .Where(product => product.Id == inventoryTransaction.ProductId)
                    .ExecuteUpdateAsync(update => update.SetProperty(
                        product => product.StockQuantity,
                        product => product.StockQuantity + inventoryTransaction.Quantity));
            }
            else
            {
                updatedRows = await _context.Products
                    .Where(product => product.Id == inventoryTransaction.ProductId
                        && product.StockQuantity >= inventoryTransaction.Quantity)
                    .ExecuteUpdateAsync(update => update.SetProperty(
                        product => product.StockQuantity,
                        product => product.StockQuantity - inventoryTransaction.Quantity));
            }

            if (updatedRows == 0)
            {
                var productExists = await _context.Products
                    .AnyAsync(product => product.Id == inventoryTransaction.ProductId);

                if (!productExists)
                {
                    return new Result<InventoryTransaction>
                    {
                        Success = false,
                        ErrorCode = "ProductNotFound",
                        ErrorMessage = "Product was not found."
                    };
                }

                return new Result<InventoryTransaction>
                {
                    Success = false,
                    ErrorCode = "InsufficientStock",
                    ErrorMessage = "The stock adjustment would make the product quantity negative."
                };
            }

            inventoryTransaction.DateUtc = DateTime.UtcNow;
            await _context.InventoryTransactions.AddAsync(inventoryTransaction);
            await _context.SaveChangesAsync();
            await databaseTransaction.CommitAsync();

            return new Result<InventoryTransaction>
            {
                Success = true,
                Data = inventoryTransaction
            };
        }

        public async Task<Product?> GetProductStock(int productId)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(product => product.Id == productId);
        }

        public async Task<List<Product>> GetLowStockProducts()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(product => product.StockQuantity <= product.LowStockThreshold)
                .OrderBy(product => product.StockQuantity)
                .ToListAsync();
        }
    }
}
