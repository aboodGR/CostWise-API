using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalMart_ERP_System.DTOs.Sales;
using WalMart_ERP_System.Interfaces;
using WalMart_ERP_System.Models;

namespace WalMart_ERP_System.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [HttpPost]
        public async Task<IActionResult> CompleteSale(CreateSaleDto createSaleDto)
        {
            var sale = new Sale();
            foreach (var item in createSaleDto.Items)
            {
                sale.Items.Add(new SaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            var result = await _salesService.CompleteSale(sale);
            if (!result.Success)
            {
                if (result.ErrorCode == "InsufficientStock")
                {
                    return Conflict(result.ErrorMessage);
                }

                if (result.ErrorCode == "ProductNotFound" || result.ErrorCode == "ProductInactive"
                    || result.ErrorCode == "InvalidSaleItem" || result.ErrorCode == "SaleHasNoItems"
                    || result.ErrorCode == "DuplicateProductLines")
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Problem("Unexpected sale completion result.");
            }

            if (result.Data == null)
            {
                return Problem("Sale completion returned no sale.");
            }

            return Ok(MapSale(result.Data));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSaleById(int id)
        {
            var sale = await _salesService.GetSaleById(id);
            if (sale == null)
            {
                return NotFound();
            }

            return Ok(MapSale(sale));
        }

        [HttpGet]
        public async Task<IActionResult> GetSalesHistory(DateTimeOffset? fromUtc, DateTimeOffset? toUtc)
        {
            if (fromUtc.HasValue && toUtc.HasValue && fromUtc.Value > toUtc.Value)
            {
                return BadRequest("fromUtc must be earlier than or equal to toUtc.");
            }

            var sales = await _salesService.GetSalesHistory(
                fromUtc?.UtcDateTime,
                toUtc?.UtcDateTime);

            var response = sales.Select(MapSale).ToList();
            return Ok(response);
        }

        private static SaleDto MapSale(Sale sale)
        {
            var response = new SaleDto
            {
                Id = sale.Id,
                DateUtc = sale.DateUtc,
                TotalAmount = sale.TotalAmount
            };

            foreach (var item in sale.Items)
            {
                response.Items.Add(new SaleItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Sku = item.Product.Sku,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    LineTotal = item.LineTotal
                });
            }

            return response;
        }
    }
}
