using Microsoft.EntityFrameworkCore;
using WalMart_ERP_System.Data;
using WalMart_ERP_System.Interfaces;
using WalMart_ERP_System.Models;
using WalMart_ERP_System.Results;

namespace WalMart_ERP_System.Services
{
    public class SalesService : ISalesService
    {
        private readonly ApplicationDbContext _context;

        public SalesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Sale>> CompleteSale(Sale sale)
        {
            var saleItems = sale.Items.ToList();
            if (saleItems.Count == 0)
            {
                return new Result<Sale>
                {
                    Success = false,
                    ErrorCode = "SaleHasNoItems",
                    ErrorMessage = "A sale must contain at least one product."
                };
            }

            if (saleItems.Any(item => item.ProductId <= 0 || item.Quantity <= 0))
            {
                return new Result<Sale>
                {
                    Success = false,
                    ErrorCode = "InvalidSaleItem",
                    ErrorMessage = "Each sale item must have a valid product and positive quantity."
                };
            }

            if (saleItems.Select(item => item.ProductId).Distinct().Count() != saleItems.Count)
            {
                return new Result<Sale>
                {
                    Success = false,
                    ErrorCode = "DuplicateProductLines",
                    ErrorMessage = "A product can only appear once in a sale."
                };
            }

            await using var databaseTransaction = await _context.Database.BeginTransactionAsync();

            var productIds = saleItems.Select(item => item.ProductId).ToList();
            var products = await _context.Products
                .Where(product => productIds.Contains(product.Id))
                .ToDictionaryAsync(product => product.Id);

            if (products.Count != productIds.Count)
            {
                return new Result<Sale>
                {
                    Success = false,
                    ErrorCode = "ProductNotFound",
                    ErrorMessage = "One or more products were not found."
                };
            }

            foreach (var saleItem in saleItems)
            {
                var product = products[saleItem.ProductId];
                if (!product.IsActive)
                {
                    return new Result<Sale>
                    {
                        Success = false,
                        ErrorCode = "ProductInactive",
                        ErrorMessage = $"Product {product.Sku} is inactive and cannot be sold."
                    };
                }

                saleItem.Product = product;
                saleItem.UnitPrice = product.SellingPrice;
                saleItem.LineTotal = product.SellingPrice * saleItem.Quantity;
            }

            foreach (var saleItem in saleItems.OrderBy(item => item.ProductId))
            {
                var updatedRows = await _context.Products
                    .Where(product => product.Id == saleItem.ProductId
                        && product.IsActive
                        && product.StockQuantity >= saleItem.Quantity)
                    .ExecuteUpdateAsync(update => update.SetProperty(
                        product => product.StockQuantity,
                        product => product.StockQuantity - saleItem.Quantity));

                if (updatedRows == 0)
                {
                    await databaseTransaction.RollbackAsync();

                    var currentProduct = await _context.Products
                        .AsNoTracking()
                        .FirstOrDefaultAsync(product => product.Id == saleItem.ProductId);
                    if (currentProduct == null)
                    {
                        return new Result<Sale>
                        {
                            Success = false,
                            ErrorCode = "ProductNotFound",
                            ErrorMessage = "A product was removed before the sale could be completed."
                        };
                    }

                    if (!currentProduct.IsActive)
                    {
                        return new Result<Sale>
                        {
                            Success = false,
                            ErrorCode = "ProductInactive",
                            ErrorMessage = $"Product {currentProduct.Sku} is inactive and cannot be sold."
                        };
                    }

                    return new Result<Sale>
                    {
                        Success = false,
                        ErrorCode = "InsufficientStock",
                        ErrorMessage = $"There is not enough stock for product {currentProduct.Sku}."
                    };
                }
            }

            sale.DateUtc = DateTime.UtcNow;
            sale.TotalAmount = saleItems.Sum(item => item.LineTotal);
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            foreach (var saleItem in saleItems)
            {
                await _context.InventoryTransactions.AddAsync(new InventoryTransaction
                {
                    ProductId = saleItem.ProductId,
                    Quantity = saleItem.Quantity,
                    Type = InventoryTransactionType.StockOut,
                    Reason = $"Sale #{sale.Id} checkout",
                    DateUtc = sale.DateUtc
                });
            }

            await _context.SaveChangesAsync();
            await databaseTransaction.CommitAsync();

            return new Result<Sale>
            {
                Success = true,
                Data = sale
            };
        }

        public async Task<Sale?> GetSaleById(int id)
        {
            return await _context.Sales
                .AsNoTracking()
                .Include(sale => sale.Items)
                .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(sale => sale.Id == id);
        }

        public async Task<List<Sale>> GetSalesHistory(DateTime? fromUtc, DateTime? toUtc)
        {
            var sales = _context.Sales
                .AsNoTracking()
                .Include(sale => sale.Items)
                .ThenInclude(item => item.Product)
                .AsQueryable();

            if (fromUtc.HasValue)
            {
                sales = sales.Where(sale => sale.DateUtc >= fromUtc.Value);
            }

            if (toUtc.HasValue)
            {
                sales = sales.Where(sale => sale.DateUtc <= toUtc.Value);
            }

            return await sales
                .OrderByDescending(sale => sale.DateUtc)
                .ToListAsync();
        }
    }
}
