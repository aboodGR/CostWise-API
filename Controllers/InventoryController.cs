using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalMart_ERP_System.DTOs.Inventory;
using WalMart_ERP_System.Interfaces;
using WalMart_ERP_System.Models;

namespace WalMart_ERP_System.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("adjustments")]
        public async Task<IActionResult> AdjustStock(InventoryAdjustmentDto adjustmentDto)
        {
            var inventoryTransaction = new InventoryTransaction
            {
                ProductId = adjustmentDto.ProductId,
                Quantity = adjustmentDto.Quantity,
                Type = adjustmentDto.Type,
                Reason = adjustmentDto.Reason
            };

            var result = await _inventoryService.AdjustStock(inventoryTransaction);
            if (!result.Success)
            {
                if (result.ErrorCode == "ProductNotFound")
                {
                    return NotFound(result.ErrorMessage);
                }

                if (result.ErrorCode == "InsufficientStock")
                {
                    return Conflict(result.ErrorMessage);
                }

                if (result.ErrorCode == "InvalidAdjustment")
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Problem("Unexpected inventory adjustment result.");
            }

            if (result.Data == null)
            {
                return Problem("Inventory adjustment returned no transaction.");
            }

            var response = new InventoryTransactionDto
            {
                Id = result.Data.Id,
                ProductId = result.Data.ProductId,
                Quantity = result.Data.Quantity,
                Type = result.Data.Type,
                Reason = result.Data.Reason,
                DateUtc = result.Data.DateUtc
            };

            return Ok(response);
        }

        [HttpGet("stock/{productId:int}")]
        public async Task<IActionResult> GetProductStock(int productId)
        {
            var product = await _inventoryService.GetProductStock(productId);
            if (product == null)
            {
                return NotFound();
            }

            var response = new InventoryStockDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Sku = product.Sku,
                StockQuantity = product.StockQuantity,
                LowStockThreshold = product.LowStockThreshold
            };

            return Ok(response);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var products = await _inventoryService.GetLowStockProducts();

            var response = products.Select(product => new InventoryStockDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Sku = product.Sku,
                StockQuantity = product.StockQuantity,
                LowStockThreshold = product.LowStockThreshold
            }).ToList();

            return Ok(response);
        }
    }
}
