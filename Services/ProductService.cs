using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WalMart_ERP_System.Data;
using WalMart_ERP_System.Interfaces;
using WalMart_ERP_System.Models;
using WalMart_ERP_System.Results;

namespace WalMart_ERP_System.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Result<Product>> AddProduct(Product product)
        {
            product.Name = product.Name.Trim();
            product.Sku = product.Sku.Trim();

            var categoryExists = await _context.Categories
                .AnyAsync(x => x.Id == product.CategoryId);
            if (!categoryExists)
            {
                return new Result<Product>
                {
                    Success = false,
                    ErrorCode = "CategoryNotFound",
                    ErrorMessage = "The selected category was not found."
                };
            }

            var skuExists = await _context.Products
                .AnyAsync(x => x.Sku == product.Sku);
            if (skuExists)
            {
                return new Result<Product>
                {
                    Success = false,
                    ErrorCode = "DuplicateSku",
                    ErrorMessage = "A product with this SKU already exists."
                };
            }

            await _context.Products.AddAsync(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                if (exception.InnerException is SqlException sqlException
                    && (sqlException.Number == 2601 || sqlException.Number == 2627))
                {
                    return new Result<Product>
                    {
                        Success = false,
                        ErrorCode = "DuplicateSku",
                        ErrorMessage = "A product with this SKU already exists."
                    };
                }

                throw;
            }

            return new Result<Product>
            {
                Success = true,
                Data = product
            };
        }

        public async Task<Result<Product>> UpdateProduct(int id, Product product)
        {
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existingProduct == null)
            {
                return new Result<Product>
                {
                    Success = false,
                    ErrorCode = "ProductNotFound",
                    ErrorMessage = "Product was not found."
                };
            }

            product.Name = product.Name.Trim();
            product.Sku = product.Sku.Trim();

            var categoryExists = await _context.Categories
                .AnyAsync(x => x.Id == product.CategoryId);
            if (!categoryExists)
            {
                return new Result<Product>
                {
                    Success = false,
                    ErrorCode = "CategoryNotFound",
                    ErrorMessage = "The selected category was not found."
                };
            }

            var skuExists = await _context.Products
                .AnyAsync(x => x.Sku == product.Sku && x.Id != id);
            if (skuExists)
            {
                return new Result<Product>
                {
                    Success = false,
                    ErrorCode = "DuplicateSku",
                    ErrorMessage = "A product with this SKU already exists."
                };
            }

            existingProduct.Name = product.Name;
            existingProduct.Sku = product.Sku;
            existingProduct.Description = product.Description;
            existingProduct.SellingPrice = product.SellingPrice;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.IsActive = product.IsActive;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                if (exception.InnerException is SqlException sqlException
                    && (sqlException.Number == 2601 || sqlException.Number == 2627))
                {
                    return new Result<Product>
                    {
                        Success = false,
                        ErrorCode = "DuplicateSku",
                        ErrorMessage = "A product with this SKU already exists."
                    };
                }

                throw;
            }

            return new Result<Product>
            {
                Success = true,
                Data = existingProduct
            };
        }

        public async Task<Result<bool>> DeleteProduct(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    ErrorCode = "ProductNotFound",
                    ErrorMessage = "Product was not found."
                };
            }

            var hasInventoryHistory = await _context.InventoryTransactions
                .AnyAsync(x => x.ProductId == id);
            var hasSalesHistory = await _context.SaleItems
                .AnyAsync(x => x.ProductId == id);
            if (hasInventoryHistory || hasSalesHistory)
            {
                return new Result<bool>
                {
                    Success = false,
                    ErrorCode = "ProductHasHistory",
                    ErrorMessage = "The product cannot be deleted because it has inventory or sales history."
                };
            }

            _context.Products.Remove(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                if (exception.InnerException is SqlException sqlException
                    && sqlException.Number == 547)
                {
                    return new Result<bool>
                    {
                        Success = false,
                        ErrorCode = "ProductHasHistory",
                        ErrorMessage = "The product cannot be deleted because it has inventory or sales history."
                    };
                }

                throw;
            }

            return new Result<bool>
            {
                Success = true,
                Data = true
            };
        }
    }
}
