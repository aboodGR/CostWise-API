using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalMart_ERP_System.DTOs.Product;
using WalMart_ERP_System.Interfaces;
using WalMart_ERP_System.Models;

namespace WalMart_ERP_System.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAc()
        {
            var products = await _productService.GetAllProducts();

            var response = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Description = product.Description,
                SellingPrice = product.SellingPrice,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAc(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            var response = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Description = product.Description,
                SellingPrice = product.SellingPrice,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAc(CreateProductDto createProductDto)
        {
            var newProduct = new Product
            {
                Name = createProductDto.Name,
                Sku = createProductDto.Sku,
                Description = createProductDto.Description,
                SellingPrice = createProductDto.SellingPrice,
                CategoryId = createProductDto.CategoryId,
                IsActive = createProductDto.IsActive
            };

            var result = await _productService.AddProduct(newProduct);
            if (!result.Success)
            {
                if (result.ErrorCode == "CategoryNotFound")
                {
                    return BadRequest(result.ErrorMessage);
                }

                if (result.ErrorCode == "DuplicateSku")
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Problem("Unexpected product creation result.");
            }

            if (result.Data == null)
            {
                return Problem("Product creation returned no data.");
            }

            var response = new ProductDto
            {
                Id = result.Data.Id,
                Name = result.Data.Name,
                Sku = result.Data.Sku,
                Description = result.Data.Description,
                SellingPrice = result.Data.SellingPrice,
                CategoryId = result.Data.CategoryId,
                IsActive = result.Data.IsActive
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAc(int id, UpdateProductDto updateProductDto)
        {
            var product = new Product
            {
                Name = updateProductDto.Name,
                Sku = updateProductDto.Sku,
                Description = updateProductDto.Description,
                SellingPrice = updateProductDto.SellingPrice,
                CategoryId = updateProductDto.CategoryId,
                IsActive = updateProductDto.IsActive
            };

            var result = await _productService.UpdateProduct(id, product);
            if (!result.Success)
            {
                if (result.ErrorCode == "ProductNotFound")
                {
                    return NotFound(result.ErrorMessage);
                }

                if (result.ErrorCode == "CategoryNotFound" || result.ErrorCode == "DuplicateSku")
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Problem("Unexpected product update result.");
            }

            if (result.Data == null)
            {
                return Problem("Product update returned no data.");
            }

            var response = new ProductDto
            {
                Id = result.Data.Id,
                Name = result.Data.Name,
                Sku = result.Data.Sku,
                Description = result.Data.Description,
                SellingPrice = result.Data.SellingPrice,
                CategoryId = result.Data.CategoryId,
                IsActive = result.Data.IsActive
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAc(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result.Success)
            {
                if (result.ErrorCode == "ProductNotFound")
                {
                    return NotFound(result.ErrorMessage);
                }

                if (result.ErrorCode == "ProductHasHistory")
                {
                    return Conflict(result.ErrorMessage);
                }

                return Problem("Unexpected product delete result.");
            }

            return NoContent();
        }
    }
}
