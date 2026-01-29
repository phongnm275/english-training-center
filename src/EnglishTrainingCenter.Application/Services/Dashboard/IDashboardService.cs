using EnglishTrainingCenter.Application.DTOs.Dashboard;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace EnglishTrainingCenter.Application.Services.Dashboard;

/// <summary>
/// Service interface for admin dashboard analytics and reporting
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Gets overall system overview statistics
    /// </summary>
    Task<SystemOverviewDto> GetSystemOverviewAsync();

    /// <summary>
    /// Gets student-related statistics and metrics
    /// </summary>
    Task<StudentStatisticsDto> GetStudentStatisticsAsync();

    /// <summary>
    /// Gets course-related statistics and metrics
    /// </summary>
    Task<CourseStatisticsDto> GetCourseStatisticsAsync();

    /// <summary>
    /// Gets instructor-related statistics and metrics
    /// </summary>
    Task<InstructorStatisticsDto> GetInstructorStatisticsAsync();

    /// <summary>
    /// Gets financial and payment analytics
    /// </summary>
    Task<FinancialMetricsDto> GetFinancialMetricsAsync();

    /// <summary>
    /// Gets academic performance analytics
    /// </summary>
    Task<AcademicMetricsDto> GetAcademicMetricsAsync();

    /// <summary>
    /// Gets revenue and payment report
    /// </summary>
    Task<RevenueReportDto> GetRevenueReportAsync(DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>
    /// Gets course performance report
    /// </summary>
    Task<IEnumerable<CoursePerformanceDto>> GetCoursePerformanceReportAsync();

    /// <summary>
    /// Gets enrollment trends over time
    /// </summary>
    Task<IEnumerable<EnrollmentTrendDto>> GetEnrollmentTrendsAsync(int months = 12);

    /// <summary>
    /// Gets payment trends over time
    /// </summary>
    Task<IEnumerable<PaymentTrendDto>> GetPaymentTrendsAsync(int months = 12);

    /// <summary>
    /// Gets top performing students by GPA
    /// </summary>
    Task<IEnumerable<TopPerformerDto>> GetTopStudentsAsync(int count = 10);

    /// <summary>
    /// Gets at-risk students (low grades/payment issues)
    /// </summary>
    Task<IEnumerable<AtRiskStudentDto>> GetAtRiskStudentsAsync();

    /// <summary>
    /// Gets system health status
    /// </summary>
    Task<SystemHealthDto> GetSystemHealthAsync();

    /// <summary>
    /// Gets user activity and login statistics
    /// </summary>
    Task<UserActivityDto> GetUserActivityAsync(int days = 30);

    /// <summary>
    /// Gets comprehensive dashboard summary
    /// </summary>
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();

    /// <summary>
    /// Gets detailed financial breakdown report
    /// </summary>
    Task<FinancialBreakdownDto> GetFinancialBreakdownAsync();
}
