using EnglishTrainingCenter.Application.DTOs.Dashboard;
using EnglishTrainingCenter.Application.Services.Dashboard;
using EnglishTrainingCenter.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// API controller for admin dashboard and analytics
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/dashboard")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Gets system overview summary with key metrics
    /// </summary>
    /// <returns>System overview data</returns>
    [HttpGet("overview")]
    [ProduceResponseType(typeof(ApiResponse<SystemOverviewDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSystemOverview()
    {
        try
        {
            var result = await _dashboardService.GetSystemOverviewAsync();

            return Ok(new ApiResponse<SystemOverviewDto>
            {
                Success = true,
                Message = "System overview retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting system overview: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving overview",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets comprehensive dashboard summary
    /// </summary>
    /// <returns>Dashboard summary</returns>
    [HttpGet("summary")]
    [ProduceResponseType(typeof(ApiResponse<DashboardSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboardSummary()
    {
        try
        {
            var result = await _dashboardService.GetDashboardSummaryAsync();

            return Ok(new ApiResponse<DashboardSummaryDto>
            {
                Success = true,
                Message = "Dashboard summary retrieved",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting dashboard summary: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving summary",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets student statistics and metrics
    /// </summary>
    /// <returns>Student statistics</returns>
    [HttpGet("students")]
    [ProduceResponseType(typeof(ApiResponse<StudentStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentStatistics()
    {
        try
        {
            var result = await _dashboardService.GetStudentStatisticsAsync();

            return Ok(new ApiResponse<StudentStatisticsDto>
            {
                Success = true,
                Message = "Student statistics retrieved",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting student statistics: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving statistics",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets course statistics and metrics
    /// </summary>
    /// <returns>Course statistics</returns>
    [HttpGet("courses")]
    [ProduceResponseType(typeof(ApiResponse<CourseStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseStatistics()
    {
        try
        {
            var result = await _dashboardService.GetCourseStatisticsAsync();

            return Ok(new ApiResponse<CourseStatisticsDto>
            {
                Success = true,
                Message = "Course statistics retrieved",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting course statistics: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving statistics",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets instructor statistics and metrics
    /// </summary>
    /// <returns>Instructor statistics</returns>
    [HttpGet("instructors")]
    [ProduceResponseType(typeof(ApiResponse<InstructorStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInstructorStatistics()
    {
        try
        {
            var result = await _dashboardService.GetInstructorStatisticsAsync();

            return Ok(new ApiResponse<InstructorStatisticsDto>
            {
                Success = true,
                Message = "Instructor statistics retrieved",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting instructor statistics: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving statistics",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets financial metrics and analytics
    /// </summary>
    /// <returns>Financial metrics</returns>
    [HttpGet("financial")]
    [ProduceResponseType(typeof(ApiResponse<FinancialMetricsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFinancialMetrics()
    {
        try
        {
            var result = await _dashboardService.GetFinancialMetricsAsync();

            return Ok(new ApiResponse<FinancialMetricsDto>
            {
                Success = true,
                Message = "Financial metrics retrieved",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting financial metrics: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving metrics",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets academic metrics and performance data
    /// </summary>
    /// <returns>Academic metrics</returns>
    [HttpGet("academic")]
    [ProduceResponseType(typeof(ApiResponse<AcademicMetricsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAcademicMetrics()
    {
        try
        {
            var result = await _dashboardService.GetAcademicMetricsAsync();

            return Ok(new ApiResponse<AcademicMetricsDto>
            {
                Success = true,
                Message = "Academic metrics retrieved",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting academic metrics: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving metrics",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets revenue report for specified period
    /// </summary>
    /// <param name="startDate">Report start date (optional)</param>
    /// <param name="endDate">Report end date (optional)</param>
    /// <returns>Revenue report</returns>
    [HttpGet("revenue")]
    [ProduceResponseType(typeof(ApiResponse<RevenueReportDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var result = await _dashboardService.GetRevenueReportAsync(startDate, endDate);

            return Ok(new ApiResponse<RevenueReportDto>
            {
                Success = true,
                Message = "Revenue report retrieved",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting revenue report: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving report",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets financial breakdown report
    /// </summary>
    /// <returns>Financial breakdown</returns>
    [HttpGet("financial-breakdown")]
    [ProduceResponseType(typeof(ApiResponse<FinancialBreakdownDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFinancialBreakdown()
    {
        try
        {
            var result = await _dashboardService.GetFinancialBreakdownAsync();

            return Ok(new ApiResponse<FinancialBreakdownDto>
            {
                Success = true,
                Message = "Financial breakdown retrieved",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting financial breakdown: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving breakdown",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets course performance report
    /// </summary>
    /// <returns>Course performance data</returns>
    [HttpGet("course-performance")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<CoursePerformanceDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCoursePerformance()
    {
        try
        {
            var result = await _dashboardService.GetCoursePerformanceReportAsync();

            return Ok(new ApiResponse<IEnumerable<CoursePerformanceDto>>
            {
                Success = true,
                Message = $"Course performance report for {result.Count()} courses",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting course performance: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving report",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets enrollment trends
    /// </summary>
    /// <param name="months">Number of months to include (default: 12)</param>
    /// <returns>Enrollment trend data</returns>
    [HttpGet("enrollment-trends")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<EnrollmentTrendDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEnrollmentTrends([FromQuery] int months = 12)
    {
        try
        {
            if (months < 1 || months > 60)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Months must be between 1 and 60"
                });

            var result = await _dashboardService.GetEnrollmentTrendsAsync(months);

            return Ok(new ApiResponse<IEnumerable<EnrollmentTrendDto>>
            {
                Success = true,
                Message = $"Enrollment trends for {months} months",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting enrollment trends: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving trends",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets payment trends
    /// </summary>
    /// <param name="months">Number of months to include (default: 12)</param>
    /// <returns>Payment trend data</returns>
    [HttpGet("payment-trends")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<PaymentTrendDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentTrends([FromQuery] int months = 12)
    {
        try
        {
            if (months < 1 || months > 60)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Months must be between 1 and 60"
                });

            var result = await _dashboardService.GetPaymentTrendsAsync(months);

            return Ok(new ApiResponse<IEnumerable<PaymentTrendDto>>
            {
                Success = true,
                Message = $"Payment trends for {months} months",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting payment trends: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving trends",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets top performing students
    /// </summary>
    /// <param name="count">Number of top students (default: 10)</param>
    /// <returns>Top student list</returns>
    [HttpGet("top-students")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<TopPerformerDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopStudents([FromQuery] int count = 10)
    {
        try
        {
            if (count < 1 || count > 100)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Count must be between 1 and 100"
                });

            var result = await _dashboardService.GetTopStudentsAsync(count);

            return Ok(new ApiResponse<IEnumerable<TopPerformerDto>>
            {
                Success = true,
                Message = $"Top {result.Count()} students retrieved",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting top students: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving students",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets at-risk students
    /// </summary>
    /// <returns>At-risk student list</returns>
    [HttpGet("at-risk-students")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<AtRiskStudentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAtRiskStudents()
    {
        try
        {
            var result = await _dashboardService.GetAtRiskStudentsAsync();

            return Ok(new ApiResponse<IEnumerable<AtRiskStudentDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} at-risk students",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting at-risk students: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving students",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets system health status
    /// </summary>
    /// <returns>System health data</returns>
    [HttpGet("health")]
    [ProduceResponseType(typeof(ApiResponse<SystemHealthDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSystemHealth()
    {
        try
        {
            var result = await _dashboardService.GetSystemHealthAsync();

            return Ok(new ApiResponse<SystemHealthDto>
            {
                Success = true,
                Message = $"System health: {result.SystemStatus}",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking system health: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error checking health",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets user activity metrics
    /// </summary>
    /// <param name="days">Number of days to include (default: 30)</param>
    /// <returns>User activity data</returns>
    [HttpGet("user-activity")]
    [ProduceResponseType(typeof(ApiResponse<UserActivityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserActivity([FromQuery] int days = 30)
    {
        try
        {
            if (days < 1 || days > 365)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Days must be between 1 and 365"
                });

            var result = await _dashboardService.GetUserActivityAsync(days);

            return Ok(new ApiResponse<UserActivityDto>
            {
                Success = true,
                Message = $"User activity for {days} days",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting user activity: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving activity",
                Data = ex.Message
            });
        }
    }
}
