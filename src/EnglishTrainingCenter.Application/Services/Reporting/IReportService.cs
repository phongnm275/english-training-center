using EnglishTrainingCenter.Application.DTOs.Reporting;

namespace EnglishTrainingCenter.Application.Services.Reporting;

/// <summary>
/// Service interface for advanced analytics and reporting
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Generates a student enrollment report
    /// </summary>
    /// <param name="startDate">Report start date</param>
    /// <param name="endDate">Report end date</param>
    /// <param name="format">Export format (PDF, Excel, CSV)</param>
    /// <returns>Report data as byte array</returns>
    Task<byte[]> GenerateStudentEnrollmentReportAsync(DateTime startDate, DateTime endDate, string format);

    /// <summary>
    /// Generates a financial report with payment details
    /// </summary>
    /// <param name="startDate">Report start date</param>
    /// <param name="endDate">Report end date</param>
    /// <param name="format">Export format (PDF, Excel, CSV)</param>
    /// <returns>Report data as byte array</returns>
    Task<byte[]> GenerateFinancialReportAsync(DateTime startDate, DateTime endDate, string format);

    /// <summary>
    /// Generates an academic performance report
    /// </summary>
    /// <param name="courseId">Optional course ID filter</param>
    /// <param name="format">Export format (PDF, Excel, CSV)</param>
    /// <returns>Report data as byte array</returns>
    Task<byte[]> GenerateAcademicReportAsync(int? courseId, string format);

    /// <summary>
    /// Generates a course performance and utilization report
    /// </summary>
    /// <param name="startDate">Report start date</param>
    /// <param name="endDate">Report end date</param>
    /// <param name="format">Export format (PDF, Excel, CSV)</param>
    /// <returns>Report data as byte array</returns>
    Task<byte[]> GenerateCoursePerformanceReportAsync(DateTime startDate, DateTime endDate, string format);

    /// <summary>
    /// Generates an instructor performance and workload report
    /// </summary>
    /// <param name="startDate">Report start date</param>
    /// <param name="endDate">Report end date</param>
    /// <param name="format">Export format (PDF, Excel, CSV)</param>
    /// <returns>Report data as byte array</returns>
    Task<byte[]> GenerateInstructorReportAsync(DateTime startDate, DateTime endDate, string format);

    /// <summary>
    /// Generates a custom report based on specified filters
    /// </summary>
    /// <param name="filter">Report filter criteria</param>
    /// <param name="format">Export format (PDF, Excel, CSV)</param>
    /// <returns>Report data as byte array</returns>
    Task<byte[]> GenerateCustomReportAsync(ReportFilterDto filter, string format);

    /// <summary>
    /// Generates an at-risk student intervention report
    /// </summary>
    /// <param name="format">Export format (PDF, Excel, CSV)</param>
    /// <returns>Report data as byte array</returns>
    Task<byte[]> GenerateAtRiskStudentReportAsync(string format);

    /// <summary>
    /// Generates a system compliance and audit report
    /// </summary>
    /// <param name="startDate">Report start date</param>
    /// <param name="endDate">Report end date</param>
    /// <param name="format">Export format (PDF, Excel, CSV)</param>
    /// <returns>Report data as byte array</returns>
    Task<byte[]> GenerateComplianceReportAsync(DateTime startDate, DateTime endDate, string format);

    /// <summary>
    /// Generates forecasting data for student enrollment trends
    /// </summary>
    /// <param name="months">Number of months to forecast</param>
    /// <returns>Forecast data</returns>
    Task<EnrollmentForecastDto> ForecastEnrollmentTrendsAsync(int months);

    /// <summary>
    /// Generates forecasting data for revenue trends
    /// </summary>
    /// <param name="months">Number of months to forecast</param>
    /// <returns>Forecast data</returns>
    Task<RevenueForecastDto> ForecastRevenueAsync(int months);

    /// <summary>
    /// Generates forecasting data for student performance
    /// </summary>
    /// <param name="months">Number of months to forecast</param>
    /// <returns>Forecast data</returns>
    Task<PerformanceForecastDto> ForecastStudentPerformanceAsync(int months);

    /// <summary>
    /// Gets available report types
    /// </summary>
    /// <returns>List of report types</returns>
    Task<IEnumerable<ReportTypeDto>> GetAvailableReportTypesAsync();

    /// <summary>
    /// Schedules a report to be generated at specified time
    /// </summary>
    /// <param name="request">Schedule request</param>
    /// <returns>Scheduled report ID</returns>
    Task<int> ScheduleReportAsync(ScheduleReportRequestDto request);

    /// <summary>
    /// Gets scheduled reports
    /// </summary>
    /// <returns>List of scheduled reports</returns>
    Task<IEnumerable<ScheduledReportDto>> GetScheduledReportsAsync();

    /// <summary>
    /// Cancels a scheduled report
    /// </summary>
    /// <param name="scheduleId">Schedule ID</param>
    /// <returns>Success indicator</returns>
    Task<bool> CancelScheduledReportAsync(int scheduleId);

    /// <summary>
    /// Gets report generation history
    /// </summary>
    /// <param name="limit">Maximum records to return</param>
    /// <returns>Report history</returns>
    Task<IEnumerable<ReportHistoryDto>> GetReportHistoryAsync(int limit = 50);

    /// <summary>
    /// Exports dashboard snapshot as report
    /// </summary>
    /// <param name="format">Export format (PDF, Excel, CSV)</param>
    /// <returns>Report data as byte array</returns>
    Task<byte[]> ExportDashboardSnapshotAsync(string format);
}
