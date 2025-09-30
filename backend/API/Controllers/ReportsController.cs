using System;
using System.Threading;
using System.Threading.Tasks;
using defectTracker.DTOs;
using defectTracker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace defectTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("export")]
        public async Task<IActionResult> ExportCsv([FromBody] DefectFilterDto filter, CancellationToken cancellationToken)
        {
            var fileBytes = await _reportService.ExportDefectsCsvAsync(filter, cancellationToken);
            return File(fileBytes, "text/csv", $"defects_{DateTime.UtcNow:yyyyMMdd}.csv");
        }

        [HttpGet("analytics")]
        public async Task<IActionResult> GetAnalytics([FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken cancellationToken)
        {
            var analytics = await _reportService.GetAnalyticsAsync(from, to, cancellationToken);
            return Ok(analytics);
        }
    }
}
