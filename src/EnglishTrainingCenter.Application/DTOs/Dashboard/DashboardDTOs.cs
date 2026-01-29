namespace EnglishTrainingCenter.Application.DTOs.Dashboard;

/// <summary>
/// DTO for system overview summary
/// </summary>
public class SystemOverviewDto
{
    /// <summary>Total number of students</summary>
    public int TotalStudents { get; set; }

    /// <summary>Number of active students</summary>
    public int ActiveStudents { get; set; }

    /// <summary>Number of inactive students</summary>
    public int InactiveStudents { get; set; }

    /// <summary>Total number of courses</summary>
    public int TotalCourses { get; set; }

    /// <summary>Number of active courses</summary>
    public int ActiveCourses { get; set; }

    /// <summary>Total number of instructors</summary>
    public int TotalInstructors { get; set; }

    /// <summary>Number of active instructors</summary>
    public int ActiveInstructors { get; set; }

    /// <summary>Total enrollments</summary>
    public int TotalEnrollments { get; set; }

    /// <summary>Total revenue amount</summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>Pending payment amount</summary>
    public decimal PendingPayments { get; set; }

    /// <summary>Completed payment amount</summary>
    public decimal CompletedPayments { get; set; }

    /// <summary>Overview generated date</summary>
    public DateTime OverviewDate { get; set; }
}

/// <summary>
/// DTO for student statistics
/// </summary>
public class StudentStatisticsDto
{
    /// <summary>Total student count</summary>
    public int TotalStudents { get; set; }

    /// <summary>Active student count</summary>
    public int ActiveStudents { get; set; }

    /// <summary>Inactive student count</summary>
    public int InactiveStudents { get; set; }

    /// <summary>New students this month</summary>
    public int NewStudentsThisMonth { get; set; }

    /// <summary>Enrollment rate percentage</summary>
    public double EnrollmentRate { get; set; }

    /// <summary>Average student GPA</summary>
    public decimal AverageGPA { get; set; }

    /// <summary>Course completion rate percentage</summary>
    public double CompletionRate { get; set; }

    /// <summary>Total course enrollments</summary>
    public int TotalEnrollments { get; set; }

    /// <summary>Average student age</summary>
    public double AverageStudentAge { get; set; }

    /// <summary>Statistics generated date</summary>
    public DateTime StatisticsDate { get; set; }
}

/// <summary>
/// DTO for course statistics
/// </summary>
public class CourseStatisticsDto
{
    /// <summary>Total course count</summary>
    public int TotalCourses { get; set; }

    /// <summary>Active course count</summary>
    public int ActiveCourses { get; set; }

    /// <summary>Inactive course count</summary>
    public int InactiveCourses { get; set; }

    /// <summary>Average course capacity</summary>
    public double AverageCapacity { get; set; }

    /// <summary>Average enrollment per course</summary>
    public double AverageEnrollment { get; set; }

    /// <summary>Capacity utilization percentage</summary>
    public double CapacityUtilization { get; set; }

    /// <summary>Total enrollments across all courses</summary>
    public int TotalEnrollments { get; set; }

    /// <summary>Breakdown of courses by level</summary>
    public Dictionary<string, int> CourseLevels { get; set; }

    /// <summary>Statistics generated date</summary>
    public DateTime StatisticsDate { get; set; }
}

/// <summary>
/// DTO for instructor statistics
/// </summary>
public class InstructorStatisticsDto
{
    /// <summary>Total instructor count</summary>
    public int TotalInstructors { get; set; }

    /// <summary>Active instructor count</summary>
    public int ActiveInstructors { get; set; }

    /// <summary>Inactive instructor count</summary>
    public int InactiveInstructors { get; set; }

    /// <summary>Average salary amount</summary>
    public decimal AverageSalary { get; set; }

    /// <summary>Average years of experience</summary>
    public double AverageExperience { get; set; }

    /// <summary>Breakdown of instructors by qualification</summary>
    public Dictionary<string, int> QualificationBreakdown { get; set; }

    /// <summary>Statistics generated date</summary>
    public DateTime StatisticsDate { get; set; }
}

/// <summary>
/// DTO for financial metrics
/// </summary>
public class FinancialMetricsDto
{
    /// <summary>Total revenue amount</summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>Completed payment amount</summary>
    public decimal CompletedPayments { get; set; }

    /// <summary>Pending payment amount</summary>
    public decimal PendingPayments { get; set; }

    /// <summary>Failed payment amount</summary>
    public decimal FailedPayments { get; set; }

    /// <summary>Average transaction value</summary>
    public decimal AverageTransactionValue { get; set; }

    /// <summary>Collection rate percentage</summary>
    public double CollectionRate { get; set; }

    /// <summary>This month revenue</summary>
    public decimal ThisMonthRevenue { get; set; }

    /// <summary>Last month revenue</summary>
    public decimal LastMonthRevenue { get; set; }

    /// <summary>Revenue growth percentage</summary>
    public double RevenueGrowth { get; set; }

    /// <summary>Outstanding balance amount</summary>
    public decimal OutstandingBalance { get; set; }

    /// <summary>Metrics generated date</summary>
    public DateTime MetricsDate { get; set; }
}

/// <summary>
/// DTO for academic metrics
/// </summary>
public class AcademicMetricsDto
{
    /// <summary>Total grades recorded</summary>
    public int TotalGrades { get; set; }

    /// <summary>Average GPA</summary>
    public decimal AverageGPA { get; set; }

    /// <summary>Passing rate percentage</summary>
    public double PassingRate { get; set; }

    /// <summary>Excellent (A grade) rate percentage</summary>
    public double ExcellentRate { get; set; }

    /// <summary>Grade distribution by letter grade</summary>
    public Dictionary<string, int> GradeDistribution { get; set; }

    /// <summary>Students with A grades</summary>
    public int StudentsWithAGrades { get; set; }

    /// <summary>Students with B grades</summary>
    public int StudentsWithBGrades { get; set; }

    /// <summary>Students with C grades</summary>
    public int StudentsWithCGrades { get; set; }

    /// <summary>Students with D grades</summary>
    public int StudentsWithDGrades { get; set; }

    /// <summary>Students with F grades</summary>
    public int StudentsWithFGrades { get; set; }

    /// <summary>Metrics generated date</summary>
    public DateTime MetricsDate { get; set; }
}

/// <summary>
/// DTO for revenue report
/// </summary>
public class RevenueReportDto
{
    /// <summary>Total revenue in period</summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>Completed revenue</summary>
    public decimal CompletedRevenue { get; set; }

    /// <summary>Pending revenue</summary>
    public decimal PendingRevenue { get; set; }

    /// <summary>Total transaction count</summary>
    public int TransactionCount { get; set; }

    /// <summary>Completed transaction count</summary>
    public int CompletedTransactions { get; set; }

    /// <summary>Pending transaction count</summary>
    public int PendingTransactions { get; set; }

    /// <summary>Failed transaction count</summary>
    public int FailedTransactions { get; set; }

    /// <summary>Average transaction value</summary>
    public decimal AverageTransaction { get; set; }

    /// <summary>Monthly revenue breakdown</summary>
    public Dictionary<string, decimal> MonthlyRevenue { get; set; }

    /// <summary>Report period start date</summary>
    public DateTime ReportPeriodStart { get; set; }

    /// <summary>Report period end date</summary>
    public DateTime ReportPeriodEnd { get; set; }

    /// <summary>Report generated date</summary>
    public DateTime GeneratedDate { get; set; }
}

/// <summary>
/// DTO for course performance report
/// </summary>
public class CoursePerformanceDto
{
    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Course code</summary>
    public string CourseCode { get; set; }

    /// <summary>Total enrollments</summary>
    public int TotalEnrollments { get; set; }

    /// <summary>Course capacity</summary>
    public int Capacity { get; set; }

    /// <summary>Enrollment rate percentage</summary>
    public double EnrollmentRate { get; set; }

    /// <summary>Average student GPA in course</summary>
    public decimal AverageStudentGPA { get; set; }

    /// <summary>Total grades recorded</summary>
    public int TotalGradesRecorded { get; set; }

    /// <summary>Number of passing students</summary>
    public int PassingStudents { get; set; }

    /// <summary>Number of instructors</summary>
    public int InstructorCount { get; set; }

    /// <summary>Report generated date</summary>
    public DateTime ReportDate { get; set; }
}

/// <summary>
/// DTO for enrollment trend
/// </summary>
public class EnrollmentTrendDto
{
    /// <summary>Month in YYYY-MM format</summary>
    public string Month { get; set; }

    /// <summary>New enrollments this month</summary>
    public int NewEnrollments { get; set; }

    /// <summary>Trend date</summary>
    public DateTime TrendDate { get; set; }
}

/// <summary>
/// DTO for payment trend
/// </summary>
public class PaymentTrendDto
{
    /// <summary>Month in YYYY-MM format</summary>
    public string Month { get; set; }

    /// <summary>Revenue for month</summary>
    public decimal Revenue { get; set; }

    /// <summary>Total transaction count</summary>
    public int TransactionCount { get; set; }

    /// <summary>Completed transaction count</summary>
    public int CompletedTransactions { get; set; }

    /// <summary>Trend date</summary>
    public DateTime TrendDate { get; set; }
}

/// <summary>
/// DTO for top performing student
/// </summary>
public class TopPerformerDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student full name</summary>
    public string StudentName { get; set; }

    /// <summary>Student email</summary>
    public string Email { get; set; }

    /// <summary>Current GPA</summary>
    public decimal GPA { get; set; }

    /// <summary>Number of courses enrolled</summary>
    public int CoursesEnrolled { get; set; }

    /// <summary>Average numeric score</summary>
    public double AverageScore { get; set; }
}

/// <summary>
/// DTO for at-risk student
/// </summary>
public class AtRiskStudentDto
{
    /// <summary>Student ID</summary>
    public int StudentId { get; set; }

    /// <summary>Student full name</summary>
    public string StudentName { get; set; }

    /// <summary>Student email</summary>
    public string Email { get; set; }

    /// <summary>Current GPA</summary>
    public decimal CurrentGPA { get; set; }

    /// <summary>List of risk factors</summary>
    public List<string> RiskFactors { get; set; }

    /// <summary>Number of enrolled courses</summary>
    public int EnrollmentCount { get; set; }
}

/// <summary>
/// DTO for system health check
/// </summary>
public class SystemHealthDto
{
    /// <summary>System status (Healthy/Warning/Critical)</summary>
    public string SystemStatus { get; set; }

    /// <summary>Data quality score 0-100</summary>
    public double DataQualityScore { get; set; }

    /// <summary>System load percentage</summary>
    public double SystemLoadPercentage { get; set; }

    /// <summary>Total active records</summary>
    public int ActiveRecordCount { get; set; }

    /// <summary>Total inactive records</summary>
    public int InactiveRecordCount { get; set; }

    /// <summary>Last health check time</summary>
    public DateTime LastHealthCheckTime { get; set; }

    /// <summary>Maintenance required flag</summary>
    public bool UpcomingMaintenanceRequired { get; set; }
}

/// <summary>
/// DTO for user activity metrics
/// </summary>
public class UserActivityDto
{
    /// <summary>Period in days</summary>
    public int PeriodDays { get; set; }

    /// <summary>Active users in period</summary>
    public int ActiveUsers { get; set; }

    /// <summary>New users in period</summary>
    public int NewUsers { get; set; }

    /// <summary>Total users in system</summary>
    public int TotalUsers { get; set; }

    /// <summary>User engagement percentage</summary>
    public double UserEngagementPercentage { get; set; }

    /// <summary>Last recorded activity date</summary>
    public DateTime LastActivityDate { get; set; }

    /// <summary>Report generated date</summary>
    public DateTime ReportDate { get; set; }
}

/// <summary>
/// DTO for dashboard summary
/// </summary>
public class DashboardSummaryDto
{
    /// <summary>Total students</summary>
    public int TotalStudents { get; set; }

    /// <summary>Active students</summary>
    public int ActiveStudents { get; set; }

    /// <summary>Total courses</summary>
    public int TotalCourses { get; set; }

    /// <summary>Total instructors</summary>
    public int TotalInstructors { get; set; }

    /// <summary>Total revenue</summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>Pending payments</summary>
    public decimal PendingPayments { get; set; }

    /// <summary>Average GPA</summary>
    public decimal AverageGPA { get; set; }

    /// <summary>Passing rate percentage</summary>
    public double PassingRate { get; set; }

    /// <summary>Top performing students</summary>
    public List<TopPerformerDto> TopPerformers { get; set; }

    /// <summary>At-risk student count</summary>
    public int AtRiskStudentCount { get; set; }

    /// <summary>System status</summary>
    public string SystemStatus { get; set; }

    /// <summary>Last updated timestamp</summary>
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// DTO for financial breakdown report
/// </summary>
public class FinancialBreakdownDto
{
    /// <summary>Total revenue</summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>Revenue breakdown by payment method</summary>
    public Dictionary<string, decimal> RevenueByPaymentMethod { get; set; }

    /// <summary>Revenue breakdown by payment status</summary>
    public Dictionary<string, decimal> RevenueByStatus { get; set; }

    /// <summary>Top payment methods</summary>
    public Dictionary<string, decimal> TopPaymentMethods { get; set; }

    /// <summary>Total refunds issued</summary>
    public decimal RefundsIssued { get; set; }

    /// <summary>Average payment amount</summary>
    public decimal AveragePaymentAmount { get; set; }

    /// <summary>Breakdown generated date</summary>
    public DateTime BreakdownDate { get; set; }
}
