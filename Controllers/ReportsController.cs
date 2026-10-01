using CostWise_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CostWise_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService reportService;
        public ReportsController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary() {
            var userIdClaim = User.FindFirst("UserId")!.Value;
            var userId = int.Parse(userIdClaim);
            return Ok(await reportService.GetSummary(userId));
        }

    }
}
