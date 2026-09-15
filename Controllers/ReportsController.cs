using CostWise_API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CostWise_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService reportService;
        public ReportsController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet("summary")]
        public IActionResult GetSummary() {
            return Ok(reportService.GetSummary());
        }

    }
}
