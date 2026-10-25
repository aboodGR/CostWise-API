using WalMart_ERP_System.Models;
using WalMart_ERP_System.Results;

namespace WalMart_ERP_System.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllProducts();
    Task<Product?> GetProductById(int id);
    Task<Result<Product>> AddProduct(Product product);
    Task<Result<Product>> UpdateProduct(int id, Product product);
    Task<Result<bool>> DeleteProduct(int id);
}
