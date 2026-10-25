using Microsoft.EntityFrameworkCore;
using WalMart_ERP_System.Data;
using WalMart_ERP_System.DTOs.Reports;
using WalMart_ERP_System.Interfaces;

namespace WalMart_ERP_System.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportDto> GetReport(DateTime fromUtc, DateTime toUtc, string groupBy)
        {
            var sales = _context.Sales
                .AsNoTracking()
                .Where(sale => sale.DateUtc >= fromUtc && sale.DateUtc <= toUtc);

            var totalSales = await sales
                .Select(sale => (decimal?)sale.TotalAmount)
                .SumAsync() ?? 0;
            var numberOfSales = await sales.CountAsync();

            var lowStockProducts = await _context.Products
                .AsNoTracking()
                .Where(product => product.StockQuantity <= product.LowStockThreshold)
                .OrderBy(product => product.StockQuantity)
                .Select(product => new LowStockProductReportDto
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Sku = product.Sku,
                    StockQuantity = product.StockQuantity,
                    LowStockThreshold = product.LowStockThreshold
                })
                .ToListAsync();

            var bestSellingProducts = await _context.SaleItems
                .AsNoTracking()
                .Where(item => item.Sale.DateUtc >= fromUtc && item.Sale.DateUtc <= toUtc)
                .GroupBy(item => new
                {
                    item.ProductId,
                    item.Product.Name,
                    item.Product.Sku
                })
                .Select(group => new
                {
                    group.Key.ProductId,
                    group.Key.Name,
                    group.Key.Sku,
                    UnitsSold = group.Sum(item => item.Quantity),
                    SalesTotal = group.Sum(item => item.LineTotal)
                })
                .OrderByDescending(product => product.UnitsSold)
                .ThenBy(product => product.Name)
                .Take(10)
                .ToListAsync();

            var report = new ReportDto
            {
                FromUtc = fromUtc,
                ToUtc = toUtc,
                TotalSales = totalSales,
                NumberOfSales = numberOfSales,
                GroupBy = groupBy,
                LowStockProducts = lowStockProducts,
                BestSellingProducts = bestSellingProducts.Select(product => new BestSellingProductReportDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Sku = product.Sku,
                    UnitsSold = product.UnitsSold,
                    SalesTotal = product.SalesTotal
                }).ToList()
            };

            if (groupBy == "month")
            {
                var monthlySales = await sales
                    .GroupBy(sale => new { sale.DateUtc.Year, sale.DateUtc.Month })
                    .Select(group => new
                    {
                        group.Key.Year,
                        group.Key.Month,
                        NumberOfSales = group.Count(),
                        TotalSales = group.Sum(sale => sale.TotalAmount)
                    })
                    .OrderBy(period => period.Year)
                    .ThenBy(period => period.Month)
                    .ToListAsync();

                report.SalesByPeriod = monthlySales.Select(period => new SalesPeriodReportDto
                {
                    PeriodStartUtc = new DateTime(period.Year, period.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                    NumberOfSales = period.NumberOfSales,
                    TotalSales = period.TotalSales
                }).ToList();
            }
            else
            {
                var dailySales = await sales
                    .GroupBy(sale => new { sale.DateUtc.Year, sale.DateUtc.Month, sale.DateUtc.Day })
                    .Select(group => new
                    {
                        group.Key.Year,
                        group.Key.Month,
                        group.Key.Day,
                        NumberOfSales = group.Count(),
                        TotalSales = group.Sum(sale => sale.TotalAmount)
                    })
                    .OrderBy(period => period.Year)
                    .ThenBy(period => period.Month)
                    .ThenBy(period => period.Day)
                    .ToListAsync();

                report.SalesByPeriod = dailySales.Select(period => new SalesPeriodReportDto
                {
                    PeriodStartUtc = new DateTime(period.Year, period.Month, period.Day, 0, 0, 0, DateTimeKind.Utc),
                    NumberOfSales = period.NumberOfSales,
                    TotalSales = period.TotalSales
                }).ToList();
            }

            return report;
        }
    }
}
