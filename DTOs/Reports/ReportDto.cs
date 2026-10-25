namespace WalMart_ERP_System.DTOs.Reports;

public class ReportDto
{
    public DateTime FromUtc { get; set; }
    public DateTime ToUtc { get; set; }
    public decimal TotalSales { get; set; }
    public int NumberOfSales { get; set; }
    public string GroupBy { get; set; } = string.Empty;
    public List<LowStockProductReportDto> LowStockProducts { get; set; } = new();
    public List<BestSellingProductReportDto> BestSellingProducts { get; set; } = new();
    public List<SalesPeriodReportDto> SalesByPeriod { get; set; } = new();
}

public class LowStockProductReportDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public int LowStockThreshold { get; set; }
}

public class BestSellingProductReportDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int UnitsSold { get; set; }
    public decimal SalesTotal { get; set; }
}

public class SalesPeriodReportDto
{
    public DateTime PeriodStartUtc { get; set; }
    public int NumberOfSales { get; set; }
    public decimal TotalSales { get; set; }
}
