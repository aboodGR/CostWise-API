using WalMart_ERP_System.DTOs.Reports;

namespace WalMart_ERP_System.Interfaces;

public interface IReportService
{
    Task<ReportDto> GetReport(DateTime fromUtc, DateTime toUtc, string groupBy);
}
