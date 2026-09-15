using CostWise_API.Models;

namespace CostWise_API.Interfaces
{
    public interface IReportService
    {
        ReportSummary GetSummary();
    }
}
