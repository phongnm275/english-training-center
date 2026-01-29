using EnglishTrainingCenter.Application.DTOs.Reporting;
using EnglishTrainingCenter.Application.Services.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// Controller for advanced analytics and reporting functionality
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    /// <summary>
    /// Generate student enrollment report
    /// </summary>
    /// <remarks>
    /// Export student enrollment data for a specific period
    /// </remarks>
    [HttpPost("enrollment")]
    [Produces("application/pdf", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv")]
    public async Task<IActionResult> GenerateEnrollmentReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string format = "PDF")
    {
        try
        {
            _logger.LogInformation($"Generating enrollment report from {startDate} to {endDate} in {format} format");

            var reportData = await _reportService.GenerateStudentEnrollmentReportAsync(startDate, endDate, format);

            var mimeType = GetMimeType(format);
            var fileName = $"Enrollment_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{GetFileExtension(format)}";

            return File(reportData, mimeType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating enrollment report: {ex.Message}");
            return BadRequest(new { message = "Error generating report", error = ex.Message });
        }
    }

    /// <summary>
    /// Generate financial report
    /// </summary>
    /// <remarks>
    /// Export financial/revenue data for a specific period
    /// </remarks>
    [HttpPost("financial")]
    [Produces("application/pdf", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv")]
    public async Task<IActionResult> GenerateFinancialReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string format = "PDF")
    {
        try
        {
            _logger.LogInformation($"Generating financial report from {startDate} to {endDate} in {format} format");

            var reportData = await _reportService.GenerateFinancialReportAsync(startDate, endDate, format);

            var mimeType = GetMimeType(format);
            var fileName = $"Financial_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{GetFileExtension(format)}";

            return File(reportData, mimeType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating financial report: {ex.Message}");
            return BadRequest(new { message = "Error generating report", error = ex.Message });
        }
    }

    /// <summary>
    /// Generate academic report
    /// </summary>
    /// <remarks>
    /// Export grade and academic performance data
    /// </remarks>
    [HttpPost("academic")]
    [Produces("application/pdf", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv")]
    public async Task<IActionResult> GenerateAcademicReport(
        [FromQuery] int? courseId,
        [FromQuery] string format = "PDF")
    {
        try
        {
            _logger.LogInformation($"Generating academic report for course {courseId ?? 0} in {format} format");

            var reportData = await _reportService.GenerateAcademicReportAsync(courseId, format);

            var mimeType = GetMimeType(format);
            var fileName = $"Academic_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{GetFileExtension(format)}";

            return File(reportData, mimeType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating academic report: {ex.Message}");
            return BadRequest(new { message = "Error generating report", error = ex.Message });
        }
    }

    /// <summary>
    /// Generate course performance report
    /// </summary>
    /// <remarks>
    /// Export course enrollment and utilization data
    /// </remarks>
    [HttpPost("course-performance")]
    [Produces("application/pdf", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv")]
    public async Task<IActionResult> GenerateCoursePerformanceReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string format = "PDF")
    {
        try
        {
            _logger.LogInformation($"Generating course performance report from {startDate} to {endDate}");

            var reportData = await _reportService.GenerateCoursePerformanceReportAsync(startDate, endDate, format);

            var mimeType = GetMimeType(format);
            var fileName = $"Course_Performance_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{GetFileExtension(format)}";

            return File(reportData, mimeType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating course performance report: {ex.Message}");
            return BadRequest(new { message = "Error generating report", error = ex.Message });
        }
    }

    /// <summary>
    /// Generate instructor report
    /// </summary>
    /// <remarks>
    /// Export instructor workload and performance data
    /// </remarks>
    [HttpPost("instructor")]
    [Produces("application/pdf", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv")]
    public async Task<IActionResult> GenerateInstructorReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string format = "PDF")
    {
        try
        {
            _logger.LogInformation($"Generating instructor report from {startDate} to {endDate}");

            var reportData = await _reportService.GenerateInstructorReportAsync(startDate, endDate, format);

            var mimeType = GetMimeType(format);
            var fileName = $"Instructor_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{GetFileExtension(format)}";

            return File(reportData, mimeType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating instructor report: {ex.Message}");
            return BadRequest(new { message = "Error generating report", error = ex.Message });
        }
    }

    /// <summary>
    /// Generate custom report
    /// </summary>
    /// <remarks>
    /// Generate report with custom filters
    /// </remarks>
    [HttpPost("custom")]
    [Produces("application/pdf", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv")]
    public async Task<IActionResult> GenerateCustomReport(
        [FromBody] ReportFilterDto filter,
        [FromQuery] string format = "PDF")
    {
        try
        {
            _logger.LogInformation($"Generating custom report with filters");

            var reportData = await _reportService.GenerateCustomReportAsync(filter, format);

            var mimeType = GetMimeType(format);
            var fileName = $"Custom_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{GetFileExtension(format)}";

            return File(reportData, mimeType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating custom report: {ex.Message}");
            return BadRequest(new { message = "Error generating report", error = ex.Message });
        }
    }

    /// <summary>
    /// Generate at-risk student report
    /// </summary>
    /// <remarks>
    /// Identify students requiring intervention
    /// </remarks>
    [HttpPost("at-risk-students")]
    [Produces("application/pdf", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "text/csv")]
    public async Task<IActionResult> GenerateAtRiskStudentReport([FromQuery] string format = "PDF")
    {
        try
        {
            _logger.LogInformation($"Generating at-risk student report");

            var reportData = await _reportService.GenerateAtRiskStudentReportAsync(format);

            var mimeType = GetMimeType(format);
            var fileName = $"AtRisk_Students_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{GetFileExtension(format)}";

            return File(reportData, mimeType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating at-risk student report: {ex.Message}");
            return BadRequest(new { message = "Error generating report", error = ex.Message });
        }
    }

    /// <summary>
    /// Generate compliance report
    /// </summary>
    /// <remarks>
    /// Export compliance and audit data
    /// </remarks>
    [HttpPost("compliance")]
    [Produces("application/pdf", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    public async Task<IActionResult> GenerateComplianceReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string format = "PDF")
    {
        try
        {
            _logger.LogInformation($"Generating compliance report from {startDate} to {endDate}");

            var reportData = await _reportService.GenerateComplianceReportAsync(startDate, endDate, format);

            var mimeType = GetMimeType(format);
            var fileName = $"Compliance_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{GetFileExtension(format)}";

            return File(reportData, mimeType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating compliance report: {ex.Message}");
            return BadRequest(new { message = "Error generating report", error = ex.Message });
        }
    }

    /// <summary>
    /// Forecast enrollment trends
    /// </summary>
    /// <remarks>
    /// Get enrollment predictions for specified number of months
    /// </remarks>
    [HttpGet("forecast/enrollment")]
    [ProduceResponseType(typeof(EnrollmentForecastDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForecastEnrollment([FromQuery] int months = 3)
    {
        try
        {
            _logger.LogInformation($"Forecasting enrollment trends for {months} months");

            var forecast = await _reportService.ForecastEnrollmentTrendsAsync(months);
            return Ok(forecast);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error forecasting enrollment: {ex.Message}");
            return BadRequest(new { message = "Error generating forecast", error = ex.Message });
        }
    }

    /// <summary>
    /// Forecast revenue trends
    /// </summary>
    /// <remarks>
    /// Get revenue predictions for specified number of months
    /// </remarks>
    [HttpGet("forecast/revenue")]
    [ProduceResponseType(typeof(RevenueForecastDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForecastRevenue([FromQuery] int months = 3)
    {
        try
        {
            _logger.LogInformation($"Forecasting revenue trends for {months} months");

            var forecast = await _reportService.ForecastRevenueAsync(months);
            return Ok(forecast);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error forecasting revenue: {ex.Message}");
            return BadRequest(new { message = "Error generating forecast", error = ex.Message });
        }
    }

    /// <summary>
    /// Forecast student performance
    /// </summary>
    /// <remarks>
    /// Get student performance predictions for specified number of months
    /// </remarks>
    [HttpGet("forecast/performance")]
    [ProduceResponseType(typeof(PerformanceForecastDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForecastPerformance([FromQuery] int months = 3)
    {
        try
        {
            _logger.LogInformation($"Forecasting student performance for {months} months");

            var forecast = await _reportService.ForecastStudentPerformanceAsync(months);
            return Ok(forecast);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error forecasting performance: {ex.Message}");
            return BadRequest(new { message = "Error generating forecast", error = ex.Message });
        }
    }

    /// <summary>
    /// Get available report types
    /// </summary>
    /// <remarks>
    /// List all available reports that can be generated
    /// </remarks>
    [HttpGet("types")]
    [ProduceResponseType(typeof(IEnumerable<ReportTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReportTypes()
    {
        try
        {
            _logger.LogInformation("Retrieving available report types");

            var types = await _reportService.GetAvailableReportTypesAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving report types: {ex.Message}");
            return BadRequest(new { message = "Error retrieving report types", error = ex.Message });
        }
    }

    /// <summary>
    /// Schedule recurring report
    /// </summary>
    /// <remarks>
    /// Schedule a report to be generated at specified intervals
    /// </remarks>
    [HttpPost("schedule")]
    [ProduceResponseType(typeof(int), StatusCodes.Status201Created)]
    public async Task<IActionResult> ScheduleReport([FromBody] ScheduleReportRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Scheduling report: {request.ReportType}");

            var scheduleId = await _reportService.ScheduleReportAsync(request);
            return CreatedAtAction(nameof(GetScheduledReports), new { id = scheduleId }, new { scheduleId });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error scheduling report: {ex.Message}");
            return BadRequest(new { message = "Error scheduling report", error = ex.Message });
        }
    }

    /// <summary>
    /// Get all scheduled reports
    /// </summary>
    /// <remarks>
    /// Retrieve list of all scheduled reports
    /// </remarks>
    [HttpGet("scheduled")]
    [ProduceResponseType(typeof(IEnumerable<ScheduledReportDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetScheduledReports()
    {
        try
        {
            _logger.LogInformation("Retrieving scheduled reports");

            var scheduled = await _reportService.GetScheduledReportsAsync();
            return Ok(scheduled);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving scheduled reports: {ex.Message}");
            return BadRequest(new { message = "Error retrieving scheduled reports", error = ex.Message });
        }
    }

    /// <summary>
    /// Cancel scheduled report
    /// </summary>
    /// <remarks>
    /// Cancel a previously scheduled report
    /// </remarks>
    [HttpDelete("scheduled/{scheduleId}")]
    [ProduceResponseType(StatusCodes.Status200OK)]
    [ProduceResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelScheduledReport([FromRoute] int scheduleId)
    {
        try
        {
            _logger.LogInformation($"Canceling scheduled report: {scheduleId}");

            var success = await _reportService.CancelScheduledReportAsync(scheduleId);
            if (!success)
                return NotFound(new { message = "Scheduled report not found" });

            return Ok(new { message = "Scheduled report canceled successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error canceling scheduled report: {ex.Message}");
            return BadRequest(new { message = "Error canceling report", error = ex.Message });
        }
    }

    /// <summary>
    /// Get report history
    /// </summary>
    /// <remarks>
    /// Retrieve history of generated reports
    /// </remarks>
    [HttpGet("history")]
    [ProduceResponseType(typeof(IEnumerable<ReportHistoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReportHistory([FromQuery] int limit = 50)
    {
        try
        {
            _logger.LogInformation($"Retrieving report history (limit: {limit})");

            var history = await _reportService.GetReportHistoryAsync(limit);
            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving report history: {ex.Message}");
            return BadRequest(new { message = "Error retrieving history", error = ex.Message });
        }
    }

    /// <summary>
    /// Export dashboard snapshot
    /// </summary>
    /// <remarks>
    /// Export current dashboard as a report
    /// </remarks>
    [HttpPost("dashboard-snapshot")]
    [Produces("application/pdf", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    public async Task<IActionResult> ExportDashboardSnapshot([FromQuery] string format = "PDF")
    {
        try
        {
            _logger.LogInformation($"Exporting dashboard snapshot in {format} format");

            var reportData = await _reportService.ExportDashboardSnapshotAsync(format);

            var mimeType = GetMimeType(format);
            var fileName = $"Dashboard_Snapshot_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{GetFileExtension(format)}";

            return File(reportData, mimeType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error exporting dashboard snapshot: {ex.Message}");
            return BadRequest(new { message = "Error exporting snapshot", error = ex.Message });
        }
    }

    // Helper methods
    private string GetMimeType(string format)
    {
        return format.ToUpper() switch
        {
            "PDF" => "application/pdf",
            "EXCEL" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "CSV" => "text/csv",
            _ => "application/octet-stream"
        };
    }

    private string GetFileExtension(string format)
    {
        return format.ToUpper() switch
        {
            "PDF" => "pdf",
            "EXCEL" => "xlsx",
            "CSV" => "csv",
            _ => "bin"
        };
    }
}
