namespace EnglishTrainingCenter.Application.DTOs.Reporting;

/// <summary>
/// Represents a generated report
/// </summary>
public class ReportDto
{
    /// <summary>
    /// Report ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Report name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Report type
    /// </summary>
    public string ReportType { get; set; } = string.Empty;

    /// <summary>
    /// Export format used
    /// </summary>
    public string Format { get; set; } = string.Empty;

    /// <summary>
    /// Report data as byte array
    /// </summary>
    public byte[] Data { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// File name for download
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// MIME type
    /// </summary>
    public string MimeType { get; set; } = string.Empty;

    /// <summary>
    /// Report generation date
    /// </summary>
    public DateTime GeneratedDate { get; set; }

    /// <summary>
    /// Report start date
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Report end date
    /// </summary>
    public DateTime? EndDate { get; set; }
}

/// <summary>
/// Report filter criteria for custom reports
/// </summary>
public class ReportFilterDto
{
    /// <summary>
    /// Report type
    /// </summary>
    public string ReportType { get; set; } = "Custom";

    /// <summary>
    /// Start date for filtering
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// End date for filtering
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Student ID filter (optional)
    /// </summary>
    public int? StudentId { get; set; }

    /// <summary>
    /// Course ID filter (optional)
    /// </summary>
    public int? CourseId { get; set; }

    /// <summary>
    /// Instructor ID filter (optional)
    /// </summary>
    public int? InstructorId { get; set; }

    /// <summary>
    /// Include detailed data
    /// </summary>
    public bool IncludeDetails { get; set; } = true;

    /// <summary>
    /// Sort field
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction (asc/desc)
    /// </summary>
    public string SortDirection { get; set; } = "asc";
}

/// <summary>
/// Available report types
/// </summary>
public class ReportTypeDto
{
    /// <summary>
    /// Report type identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Report type display name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Report description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Supported export formats
    /// </summary>
    public List<string> SupportedFormats { get; set; } = new();

    /// <summary>
    /// Whether report requires date range
    /// </summary>
    public bool RequiresDateRange { get; set; }

    /// <summary>
    /// Whether report requires filter parameters
    /// </summary>
    public bool RequiresFilter { get; set; }
}

/// <summary>
/// Enrollment forecast data
/// </summary>
public class EnrollmentForecastDto
{
    /// <summary>
    /// Forecast month
    /// </summary>
    public string Month { get; set; } = string.Empty;

    /// <summary>
    /// Forecasted enrollment count
    /// </summary>
    public int ForecastedEnrollment { get; set; }

    /// <summary>
    /// Confidence level (0-100%)
    /// </summary>
    public decimal ConfidenceLevel { get; set; }

    /// <summary>
    /// Trend direction (Up/Down/Stable)
    /// </summary>
    public string Trend { get; set; } = "Stable";

    /// <summary>
    /// Forecast date
    /// </summary>
    public DateTime ForecastDate { get; set; }
}

/// <summary>
/// Revenue forecast data
/// </summary>
public class RevenueForecastDto
{
    /// <summary>
    /// Forecast month
    /// </summary>
    public string Month { get; set; } = string.Empty;

    /// <summary>
    /// Forecasted revenue amount
    /// </summary>
    public decimal ForecastedRevenue { get; set; }

    /// <summary>
    /// Confidence level (0-100%)
    /// </summary>
    public decimal ConfidenceLevel { get; set; }

    /// <summary>
    /// Trend direction (Up/Down/Stable)
    /// </summary>
    public string Trend { get; set; } = "Stable";

    /// <summary>
    /// Growth percentage vs previous month
    /// </summary>
    public decimal GrowthPercentage { get; set; }

    /// <summary>
    /// Forecast date
    /// </summary>
    public DateTime ForecastDate { get; set; }
}

/// <summary>
/// Student performance forecast
/// </summary>
public class PerformanceForecastDto
{
    /// <summary>
    /// Forecast period
    /// </summary>
    public string Period { get; set; } = string.Empty;

    /// <summary>
    /// Forecasted average GPA
    /// </summary>
    public decimal ForecastedAverageGPA { get; set; }

    /// <summary>
    /// Forecasted passing rate percentage
    /// </summary>
    public decimal ForecastedPassingRate { get; set; }

    /// <summary>
    /// Confidence level (0-100%)
    /// </summary>
    public decimal ConfidenceLevel { get; set; }

    /// <summary>
    /// Risk factors identified
    /// </summary>
    public List<string> IdentifiedRisks { get; set; } = new();

    /// <summary>
    /// Recommended interventions
    /// </summary>
    public List<string> RecommendedInterventions { get; set; } = new();

    /// <summary>
    /// Forecast date
    /// </summary>
    public DateTime ForecastDate { get; set; }
}

/// <summary>
/// Scheduled report information
/// </summary>
public class ScheduleReportRequestDto
{
    /// <summary>
    /// Report type to schedule
    /// </summary>
    public string ReportType { get; set; } = string.Empty;

    /// <summary>
    /// Export format
    /// </summary>
    public string Format { get; set; } = "PDF";

    /// <summary>
    /// Schedule time (UTC)
    /// </summary>
    public DateTime ScheduleTime { get; set; }

    /// <summary>
    /// Recurrence pattern (Once, Daily, Weekly, Monthly)
    /// </summary>
    public string Recurrence { get; set; } = "Once";

    /// <summary>
    /// Email recipient for scheduled report
    /// </summary>
    public string? EmailRecipient { get; set; }

    /// <summary>
    /// Filter criteria for report
    /// </summary>
    public ReportFilterDto? Filter { get; set; }
}

/// <summary>
/// Scheduled report details
/// </summary>
public class ScheduledReportDto
{
    /// <summary>
    /// Schedule ID
    /// </summary>
    public int ScheduleId { get; set; }

    /// <summary>
    /// Report type
    /// </summary>
    public string ReportType { get; set; } = string.Empty;

    /// <summary>
    /// Export format
    /// </summary>
    public string Format { get; set; } = string.Empty;

    /// <summary>
    /// Next execution time
    /// </summary>
    public DateTime NextExecution { get; set; }

    /// <summary>
    /// Recurrence pattern
    /// </summary>
    public string Recurrence { get; set; } = "Once";

    /// <summary>
    /// Last execution time
    /// </summary>
    public DateTime? LastExecution { get; set; }

    /// <summary>
    /// Is schedule active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Email recipient
    /// </summary>
    public string? EmailRecipient { get; set; }

    /// <summary>
    /// Created date
    /// </summary>
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// Report generation history
/// </summary>
public class ReportHistoryDto
{
    /// <summary>
    /// Report history ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Report type
    /// </summary>
    public string ReportType { get; set; } = string.Empty;

    /// <summary>
    /// Export format used
    /// </summary>
    public string Format { get; set; } = string.Empty;

    /// <summary>
    /// Generation date and time
    /// </summary>
    public DateTime GeneratedDate { get; set; }

    /// <summary>
    /// Report period start date
    /// </summary>
    public DateTime? PeriodStartDate { get; set; }

    /// <summary>
    /// Report period end date
    /// </summary>
    public DateTime? PeriodEndDate { get; set; }

    /// <summary>
    /// Generated by (user ID)
    /// </summary>
    public string? GeneratedBy { get; set; }

    /// <summary>
    /// Download count
    /// </summary>
    public int DownloadCount { get; set; }

    /// <summary>
    /// Last downloaded date
    /// </summary>
    public DateTime? LastDownloadedDate { get; set; }

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// Report status (Generated, Archived, Deleted)
    /// </summary>
    public string Status { get; set; } = "Generated";
}

/// <summary>
/// Student enrollment report data
/// </summary>
public class StudentEnrollmentReportDto
{
    /// <summary>
    /// Student ID
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Student full name
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Number of courses enrolled
    /// </summary>
    public int CourseCount { get; set; }

    /// <summary>
    /// Course names (comma-separated)
    /// </summary>
    public string EnrolledCourses { get; set; } = string.Empty;

    /// <summary>
    /// Total enrollment hours
    /// </summary>
    public int TotalHours { get; set; }

    /// <summary>
    /// Current GPA
    /// </summary>
    public decimal CurrentGPA { get; set; }

    /// <summary>
    /// Attendance rate percentage
    /// </summary>
    public decimal AttendanceRate { get; set; }

    /// <summary>
    /// Enrollment status
    /// </summary>
    public string Status { get; set; } = "Active";

    /// <summary>
    /// First enrollment date
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// Last activity date
    /// </summary>
    public DateTime? LastActivityDate { get; set; }
}

/// <summary>
/// Financial report data
/// </summary>
public class FinancialReportDto
{
    /// <summary>
    /// Report period
    /// </summary>
    public string ReportPeriod { get; set; } = string.Empty;

    /// <summary>
    /// Total invoiced amount
    /// </summary>
    public decimal TotalInvoiced { get; set; }

    /// <summary>
    /// Total paid amount
    /// </summary>
    public decimal TotalPaid { get; set; }

    /// <summary>
    /// Total pending amount
    /// </summary>
    public decimal TotalPending { get; set; }

    /// <summary>
    /// Total refunded amount
    /// </summary>
    public decimal TotalRefunded { get; set; }

    /// <summary>
    /// Collection rate percentage
    /// </summary>
    public decimal CollectionRate { get; set; }

    /// <summary>
    /// Outstanding balance
    /// </summary>
    public decimal OutstandingBalance { get; set; }

    /// <summary>
    /// Number of transactions
    /// </summary>
    public int TransactionCount { get; set; }

    /// <summary>
    /// Average transaction amount
    /// </summary>
    public decimal AverageTransaction { get; set; }

    /// <summary>
    /// Revenue growth percentage
    /// </summary>
    public decimal RevenueGrowthPercentage { get; set; }

    /// <summary>
    /// Top payment methods
    /// </summary>
    public Dictionary<string, decimal> PaymentMethodBreakdown { get; set; } = new();

    /// <summary>
    /// Monthly breakdown
    /// </summary>
    public Dictionary<string, decimal> MonthlyBreakdown { get; set; } = new();
}

/// <summary>
/// Academic report data
/// </summary>
public class AcademicReportDto
{
    /// <summary>
    /// Course ID (if filtered)
    /// </summary>
    public int? CourseId { get; set; }

    /// <summary>
    /// Course name (if filtered)
    /// </summary>
    public string? CourseName { get; set; }

    /// <summary>
    /// Total students graded
    /// </summary>
    public int TotalStudents { get; set; }

    /// <summary>
    /// Average GPA
    /// </summary>
    public decimal AverageGPA { get; set; }

    /// <summary>
    /// Passing rate percentage
    /// </summary>
    public decimal PassingRate { get; set; }

    /// <summary>
    /// Excellent rate (Grade A) percentage
    /// </summary>
    public decimal ExcellentRate { get; set; }

    /// <summary>
    /// Grade distribution
    /// </summary>
    public Dictionary<string, int> GradeDistribution { get; set; } = new();

    /// <summary>
    /// Highest score
    /// </summary>
    public decimal HighestScore { get; set; }

    /// <summary>
    /// Lowest score
    /// </summary>
    public decimal LowestScore { get; set; }

    /// <summary>
    /// Score standard deviation
    /// </summary>
    public decimal StandardDeviation { get; set; }

    /// <summary>
    /// Student performance list
    /// </summary>
    public List<StudentGradeDto> StudentGrades { get; set; } = new();
}

/// <summary>
/// Student grade information for report
/// </summary>
public class StudentGradeDto
{
    /// <summary>
    /// Student ID
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Student name
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Score
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// Letter grade
    /// </summary>
    public char LetterGrade { get; set; }

    /// <summary>
    /// GPA points
    /// </summary>
    public decimal GPA { get; set; }
}

/// <summary>
/// At-risk student report data
/// </summary>
public class AtRiskStudentReportDto
{
    /// <summary>
    /// Student ID
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Student name
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Current GPA
    /// </summary>
    public decimal CurrentGPA { get; set; }

    /// <summary>
    /// Risk level (Low, Medium, High, Critical)
    /// </summary>
    public string RiskLevel { get; set; } = "Low";

    /// <summary>
    /// Risk factors identified
    /// </summary>
    public List<string> RiskFactors { get; set; } = new();

    /// <summary>
    /// Recommended interventions
    /// </summary>
    public List<string> RecommendedActions { get; set; } = new();

    /// <summary>
    /// Days since last activity
    /// </summary>
    public int DaysSinceLastActivity { get; set; }

    /// <summary>
    /// Outstanding balance
    /// </summary>
    public decimal OutstandingBalance { get; set; }
}
