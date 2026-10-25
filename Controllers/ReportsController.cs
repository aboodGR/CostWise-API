using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalMart_ERP_System.Interfaces;

namespace WalMart_ERP_System.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReport(
            DateTimeOffset? fromUtc,
            DateTimeOffset? toUtc,
            string groupBy = "day")
        {
            if (!fromUtc.HasValue || !toUtc.HasValue)
            {
                return BadRequest("Both fromUtc and toUtc are required.");
            }

            if (fromUtc.Value > toUtc.Value)
            {
                return BadRequest("fromUtc must be earlier than or equal to toUtc.");
            }

            if (!string.Equals(groupBy, "day", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(groupBy, "month", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("groupBy must be either 'day' or 'month'.");
            }

            var report = await _reportService.GetReport(
                fromUtc.Value.UtcDateTime,
                toUtc.Value.UtcDateTime,
                groupBy.ToLowerInvariant());

            return Ok(report);
        }
    }
}
