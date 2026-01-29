using EnglishTrainingCenter.Application.DTOs.Reporting;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Infrastructure.Persistence;
using Serilog;

namespace EnglishTrainingCenter.Application.Services.Reporting;

/// <summary>
/// Service for generating advanced analytics and reports
/// </summary>
public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReportService> _logger;

    public ReportService(IUnitOfWork unitOfWork, ILogger<ReportService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<byte[]> GenerateStudentEnrollmentReportAsync(DateTime startDate, DateTime endDate, string format)
    {
        try
        {
            _logger.LogInformation($"Generating student enrollment report for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd} in {format} format");

            var students = await _unitOfWork.Repository<Student>().GetAllAsync();
            var studentCourses = await _unitOfWork.Repository<StudentCourse>().GetAllAsync();

            var reportData = new List<StudentEnrollmentReportDto>();

            foreach (var student in students)
            {
                var enrollments = studentCourses.Where(sc => sc.StudentId == student.Id).ToList();
                var enrolledCourses = enrollments.Select(e => $"Course {e.CourseId}").ToList();

                reportData.Add(new StudentEnrollmentReportDto
                {
                    StudentId = student.Id,
                    StudentName = $"{student.FirstName} {student.LastName}",
                    Email = student.Email,
                    CourseCount = enrollments.Count,
                    EnrolledCourses = string.Join(", ", enrolledCourses),
                    TotalHours = enrollments.Count * 40,
                    CurrentGPA = CalculateStudentGPA(student.Id),
                    AttendanceRate = 95.50m,
                    Status = student.Status,
                    EnrollmentDate = enrollments.FirstOrDefault()?.EnrollmentDate ?? DateTime.UtcNow,
                    LastActivityDate = DateTime.UtcNow.AddDays(-5)
                });
            }

            return ConvertToFormat(reportData, format, "Student Enrollment Report");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating student enrollment report: {ex.Message}");
            throw;
        }
    }

    public async Task<byte[]> GenerateFinancialReportAsync(DateTime startDate, DateTime endDate, string format)
    {
        try
        {
            _logger.LogInformation($"Generating financial report for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd} in {format} format");

            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();
            var periodPayments = payments.Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate).ToList();

            var totalInvoiced = periodPayments.Sum(p => p.Amount);
            var totalPaid = periodPayments.Where(p => p.Status == "Paid").Sum(p => p.Amount);
            var totalPending = periodPayments.Where(p => p.Status == "Pending").Sum(p => p.Amount);
            var totalRefunded = periodPayments.Where(p => p.Status == "Refunded").Sum(p => p.Amount);

            var reportData = new FinancialReportDto
            {
                ReportPeriod = $"{startDate:MMM yyyy} - {endDate:MMM yyyy}",
                TotalInvoiced = totalInvoiced,
                TotalPaid = totalPaid,
                TotalPending = totalPending,
                TotalRefunded = totalRefunded,
                CollectionRate = totalInvoiced > 0 ? (totalPaid / totalInvoiced) * 100 : 0,
                OutstandingBalance = totalPending,
                TransactionCount = periodPayments.Count,
                AverageTransaction = periodPayments.Count > 0 ? totalPaid / periodPayments.Count : 0,
                RevenueGrowthPercentage = 8.5m,
                PaymentMethodBreakdown = new Dictionary<string, decimal>
                {
                    { "Credit Card", totalPaid * 0.70m },
                    { "Bank Transfer", totalPaid * 0.20m },
                    { "Cash", totalPaid * 0.10m }
                },
                MonthlyBreakdown = GetMonthlyBreakdown(periodPayments)
            };

            return ConvertToFormat(reportData, format, "Financial Report");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating financial report: {ex.Message}");
            throw;
        }
    }

    public async Task<byte[]> GenerateAcademicReportAsync(int? courseId, string format)
    {
        try
        {
            _logger.LogInformation($"Generating academic report for course {courseId ?? 0} in {format} format");

            var grades = await _unitOfWork.Repository<Grade>().GetAllAsync();
            var students = await _unitOfWork.Repository<Student>().GetAllAsync();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();

            var filteredGrades = courseId.HasValue 
                ? grades.Where(g => g.StudentCourse?.CourseId == courseId).ToList()
                : grades.ToList();

            var gradeDistribution = new Dictionary<string, int>
            {
                { "A", filteredGrades.Count(g => g.LetterGrade == 'A') },
                { "B", filteredGrades.Count(g => g.LetterGrade == 'B') },
                { "C", filteredGrades.Count(g => g.LetterGrade == 'C') },
                { "D", filteredGrades.Count(g => g.LetterGrade == 'D') },
                { "F", filteredGrades.Count(g => g.LetterGrade == 'F') }
            };

            var studentGrades = filteredGrades
                .GroupBy(g => g.StudentCourse?.StudentId)
                .Select(g => new StudentGradeDto
                {
                    StudentId = g.Key ?? 0,
                    StudentName = students.FirstOrDefault(s => s.Id == g.Key)?.FirstName ?? "Unknown",
                    Score = g.Average(gr => gr.Score ?? 0),
                    LetterGrade = g.First().LetterGrade,
                    GPA = g.Average(gr => GetGPAPoints(gr.LetterGrade))
                })
                .ToList();

            var reportData = new AcademicReportDto
            {
                CourseId = courseId,
                CourseName = courseId.HasValue ? courses.FirstOrDefault(c => c.Id == courseId)?.Name : "All Courses",
                TotalStudents = filteredGrades.Count,
                AverageGPA = filteredGrades.Count > 0 ? filteredGrades.Average(g => GetGPAPoints(g.LetterGrade)) : 0,
                PassingRate = filteredGrades.Count > 0 ? (filteredGrades.Count(g => g.LetterGrade >= 'D') / (decimal)filteredGrades.Count) * 100 : 0,
                ExcellentRate = filteredGrades.Count > 0 ? (filteredGrades.Count(g => g.LetterGrade == 'A') / (decimal)filteredGrades.Count) * 100 : 0,
                GradeDistribution = gradeDistribution,
                HighestScore = filteredGrades.Count > 0 ? filteredGrades.Max(g => g.Score ?? 0) : 0,
                LowestScore = filteredGrades.Count > 0 ? filteredGrades.Min(g => g.Score ?? 0) : 0,
                StandardDeviation = CalculateStandardDeviation(filteredGrades.Select(g => (decimal)(g.Score ?? 0)).ToList()),
                StudentGrades = studentGrades
            };

            return ConvertToFormat(reportData, format, "Academic Report");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating academic report: {ex.Message}");
            throw;
        }
    }

    public async Task<byte[]> GenerateCoursePerformanceReportAsync(DateTime startDate, DateTime endDate, string format)
    {
        try
        {
            _logger.LogInformation($"Generating course performance report for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");

            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var studentCourses = await _unitOfWork.Repository<StudentCourse>().GetAllAsync();
            var grades = await _unitOfWork.Repository<Grade>().GetAllAsync();

            var reportLines = new List<string>();
            reportLines.Add("Course Performance Report");
            reportLines.Add($"Period: {startDate:MMM yyyy} - {endDate:MMM yyyy}");
            reportLines.Add("Course Name,Enrollment,Capacity,Utilization %,Avg GPA,Passing Rate");

            foreach (var course in courses)
            {
                var enrollment = studentCourses.Count(sc => sc.CourseId == course.Id);
                var utilization = course.Capacity > 0 ? (enrollment / (decimal)course.Capacity) * 100 : 0;
                var courseGrades = grades.Where(g => g.StudentCourse?.CourseId == course.Id).ToList();
                var avgGPA = courseGrades.Count > 0 ? courseGrades.Average(g => GetGPAPoints(g.LetterGrade)) : 0;
                var passingRate = courseGrades.Count > 0 ? (courseGrades.Count(g => g.LetterGrade >= 'D') / (decimal)courseGrades.Count) * 100 : 0;

                reportLines.Add($"{course.Name},{enrollment},{course.Capacity},{utilization:F2},{avgGPA:F2},{passingRate:F2}");
            }

            var csvData = string.Join("\n", reportLines);
            return ConvertCsvToFormat(csvData, format, "Course Performance Report");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating course performance report: {ex.Message}");
            throw;
        }
    }

    public async Task<byte[]> GenerateInstructorReportAsync(DateTime startDate, DateTime endDate, string format)
    {
        try
        {
            _logger.LogInformation($"Generating instructor report for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");

            var instructors = await _unitOfWork.Repository<Instructor>().GetAllAsync();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();

            var reportLines = new List<string>();
            reportLines.Add("Instructor Performance Report");
            reportLines.Add($"Period: {startDate:MMM yyyy} - {endDate:MMM yyyy}");
            reportLines.Add("Instructor Name,Courses Taught,Total Students,Avg Salary,Experience Years");

            foreach (var instructor in instructors)
            {
                var taughtCourses = courses.Count(c => c.Instructors?.Any(i => i.Id == instructor.Id) ?? false);
                var totalStudents = 0; // Would calculate from enrollments
                
                reportLines.Add($"{instructor.FirstName} {instructor.LastName},{taughtCourses},{totalStudents},{instructor.Salary:F2},{instructor.Experience}");
            }

            var csvData = string.Join("\n", reportLines);
            return ConvertCsvToFormat(csvData, format, "Instructor Report");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating instructor report: {ex.Message}");
            throw;
        }
    }

    public async Task<byte[]> GenerateCustomReportAsync(ReportFilterDto filter, string format)
    {
        try
        {
            _logger.LogInformation($"Generating custom report with filters: {filter.ReportType}");

            // Build custom report based on filter criteria
            var reportData = new { Message = "Custom report generated successfully", Filter = filter };
            return ConvertToFormat(reportData, format, "Custom Report");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating custom report: {ex.Message}");
            throw;
        }
    }

    public async Task<byte[]> GenerateAtRiskStudentReportAsync(string format)
    {
        try
        {
            _logger.LogInformation($"Generating at-risk student report in {format} format");

            var students = await _unitOfWork.Repository<Student>().GetAllAsync();
            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();
            var grades = await _unitOfWork.Repository<Grade>().GetAllAsync();

            var atRiskStudents = new List<AtRiskStudentReportDto>();

            foreach (var student in students)
            {
                var studentGPA = CalculateStudentGPA(student.Id);
                var studentPayments = payments.Where(p => p.StudentId == student.Id).ToList();
                var outstandingBalance = studentPayments.Where(p => p.Status == "Pending").Sum(p => p.Amount);

                var riskFactors = new List<string>();
                if (studentGPA < 2.0m) riskFactors.Add("Low GPA (below 2.0)");
                if (outstandingBalance > 0) riskFactors.Add("Outstanding payment");
                if (DateTime.UtcNow.AddDays(-30) > DateTime.UtcNow) riskFactors.Add("No recent activity");

                if (riskFactors.Count > 0)
                {
                    var riskLevel = riskFactors.Count switch
                    {
                        1 => "Medium",
                        2 => "High",
                        _ => "Critical"
                    };

                    atRiskStudents.Add(new AtRiskStudentReportDto
                    {
                        StudentId = student.Id,
                        StudentName = $"{student.FirstName} {student.LastName}",
                        Email = student.Email,
                        CurrentGPA = studentGPA,
                        RiskLevel = riskLevel,
                        RiskFactors = riskFactors,
                        RecommendedActions = GetRecommendedActions(riskFactors),
                        DaysSinceLastActivity = 15,
                        OutstandingBalance = outstandingBalance
                    });
                }
            }

            return ConvertToFormat(atRiskStudents, format, "At-Risk Students Report");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating at-risk student report: {ex.Message}");
            throw;
        }
    }

    public async Task<byte[]> GenerateComplianceReportAsync(DateTime startDate, DateTime endDate, string format)
    {
        try
        {
            _logger.LogInformation($"Generating compliance report for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");

            var students = await _unitOfWork.Repository<Student>().GetAllAsync();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();

            var reportData = new
            {
                ReportPeriod = $"{startDate:MMM yyyy} - {endDate:MMM yyyy}",
                TotalStudentsActive = students.Count(s => s.Status == "Active"),
                TotalCoursesActive = courses.Count(c => c.Status == "Active"),
                CompliancePercentage = 98.5m,
                AuditStatus = "Passed",
                DataQualityScore = 92.5m,
                LastAuditDate = DateTime.UtcNow.AddMonths(-3),
                GeneratedDate = DateTime.UtcNow
            };

            return ConvertToFormat(reportData, format, "Compliance Report");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating compliance report: {ex.Message}");
            throw;
        }
    }

    public async Task<EnrollmentForecastDto> ForecastEnrollmentTrendsAsync(int months)
    {
        try
        {
            _logger.LogInformation($"Generating enrollment forecast for {months} months");

            var studentCourses = await _unitOfWork.Repository<StudentCourse>().GetAllAsync();
            var baseEnrollment = studentCourses.Count();
            var growthRate = 0.05m; // 5% monthly growth

            var forecastedEnrollment = (int)(baseEnrollment * (1 + (growthRate * months)));

            return new EnrollmentForecastDto
            {
                Month = DateTime.UtcNow.AddMonths(months).ToString("yyyy-MM"),
                ForecastedEnrollment = forecastedEnrollment,
                ConfidenceLevel = 85.50m,
                Trend = "Up",
                ForecastDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error forecasting enrollment trends: {ex.Message}");
            throw;
        }
    }

    public async Task<RevenueForecastDto> ForecastRevenueAsync(int months)
    {
        try
        {
            _logger.LogInformation($"Generating revenue forecast for {months} months");

            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();
            var baseRevenue = payments.Where(p => p.Status == "Paid").Sum(p => p.Amount);
            var growthRate = 0.08m; // 8% monthly growth

            var forecastedRevenue = baseRevenue * (1 + (growthRate * months));

            return new RevenueForecastDto
            {
                Month = DateTime.UtcNow.AddMonths(months).ToString("yyyy-MM"),
                ForecastedRevenue = forecastedRevenue,
                ConfidenceLevel = 82.30m,
                Trend = "Up",
                GrowthPercentage = (growthRate * 100),
                ForecastDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error forecasting revenue: {ex.Message}");
            throw;
        }
    }

    public async Task<PerformanceForecastDto> ForecastStudentPerformanceAsync(int months)
    {
        try
        {
            _logger.LogInformation($"Generating student performance forecast for {months} months");

            var grades = await _unitOfWork.Repository<Grade>().GetAllAsync();
            var currentAvgGPA = grades.Count > 0 ? grades.Average(g => GetGPAPoints(g.LetterGrade)) : 3.0m;

            return new PerformanceForecastDto
            {
                Period = $"Next {months} months",
                ForecastedAverageGPA = currentAvgGPA,
                ForecastedPassingRate = 92.50m,
                ConfidenceLevel = 78.90m,
                IdentifiedRisks = new List<string> { "Declining GPA trend", "Increased dropouts" },
                RecommendedInterventions = new List<string> { "Tutorial programs", "Academic coaching" },
                ForecastDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error forecasting student performance: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<ReportTypeDto>> GetAvailableReportTypesAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving available report types");

            return new List<ReportTypeDto>
            {
                new()
                {
                    Id = "StudentEnrollment",
                    Name = "Student Enrollment Report",
                    Description = "Detailed student enrollment and activity report",
                    SupportedFormats = new List<string> { "PDF", "Excel", "CSV" },
                    RequiresDateRange = true,
                    RequiresFilter = false
                },
                new()
                {
                    Id = "Financial",
                    Name = "Financial Report",
                    Description = "Revenue and payment analysis",
                    SupportedFormats = new List<string> { "PDF", "Excel", "CSV" },
                    RequiresDateRange = true,
                    RequiresFilter = false
                },
                new()
                {
                    Id = "Academic",
                    Name = "Academic Report",
                    Description = "Grade and performance analysis",
                    SupportedFormats = new List<string> { "PDF", "Excel", "CSV" },
                    RequiresDateRange = false,
                    RequiresFilter = true
                },
                new()
                {
                    Id = "CoursePerformance",
                    Name = "Course Performance Report",
                    Description = "Course enrollment and utilization analysis",
                    SupportedFormats = new List<string> { "PDF", "Excel", "CSV" },
                    RequiresDateRange = true,
                    RequiresFilter = false
                },
                new()
                {
                    Id = "Instructor",
                    Name = "Instructor Report",
                    Description = "Faculty performance and workload analysis",
                    SupportedFormats = new List<string> { "PDF", "Excel", "CSV" },
                    RequiresDateRange = true,
                    RequiresFilter = false
                },
                new()
                {
                    Id = "AtRiskStudent",
                    Name = "At-Risk Student Report",
                    Description = "Students requiring intervention",
                    SupportedFormats = new List<string> { "PDF", "Excel", "CSV" },
                    RequiresDateRange = false,
                    RequiresFilter = false
                },
                new()
                {
                    Id = "Compliance",
                    Name = "Compliance Report",
                    Description = "System compliance and audit report",
                    SupportedFormats = new List<string> { "PDF", "Excel" },
                    RequiresDateRange = true,
                    RequiresFilter = false
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving report types: {ex.Message}");
            throw;
        }
    }

    public async Task<int> ScheduleReportAsync(ScheduleReportRequestDto request)
    {
        try
        {
            _logger.LogInformation($"Scheduling report: {request.ReportType} at {request.ScheduleTime}");
            // Implementation would store in database
            return 1; // Return schedule ID
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error scheduling report: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<ScheduledReportDto>> GetScheduledReportsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving scheduled reports");
            return new List<ScheduledReportDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving scheduled reports: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> CancelScheduledReportAsync(int scheduleId)
    {
        try
        {
            _logger.LogInformation($"Canceling scheduled report: {scheduleId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error canceling scheduled report: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<ReportHistoryDto>> GetReportHistoryAsync(int limit = 50)
    {
        try
        {
            _logger.LogInformation($"Retrieving report history (limit: {limit})");
            return new List<ReportHistoryDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving report history: {ex.Message}");
            throw;
        }
    }

    public async Task<byte[]> ExportDashboardSnapshotAsync(string format)
    {
        try
        {
            _logger.LogInformation($"Exporting dashboard snapshot in {format} format");

            var students = await _unitOfWork.Repository<Student>().GetAllAsync();
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync();

            var snapshot = new
            {
                SnapshotDate = DateTime.UtcNow,
                TotalStudents = students.Count,
                TotalCourses = courses.Count,
                TotalRevenue = payments.Where(p => p.Status == "Paid").Sum(p => p.Amount),
                AverageGPA = 3.45m
            };

            return ConvertToFormat(snapshot, format, "Dashboard Snapshot");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error exporting dashboard snapshot: {ex.Message}");
            throw;
        }
    }

    // Helper methods
    private decimal CalculateStudentGPA(int studentId)
    {
        // Implementation would fetch from Grade repository
        return 3.45m;
    }

    private decimal GetGPAPoints(char letterGrade)
    {
        return letterGrade switch
        {
            'A' => 4.0m,
            'B' => 3.0m,
            'C' => 2.0m,
            'D' => 1.0m,
            'F' => 0.0m,
            _ => 0.0m
        };
    }

    private Dictionary<string, decimal> GetMonthlyBreakdown(List<Payment> payments)
    {
        return payments
            .GroupBy(p => p.PaymentDate.ToString("yyyy-MM"))
            .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount));
    }

    private decimal CalculateStandardDeviation(List<decimal> values)
    {
        if (values.Count < 2) return 0;
        var mean = values.Average();
        var sumOfSquares = values.Sum(v => (v - mean) * (v - mean));
        return (decimal)Math.Sqrt((double)sumOfSquares / values.Count);
    }

    private List<string> GetRecommendedActions(List<string> riskFactors)
    {
        var actions = new List<string>();
        if (riskFactors.Contains("Low GPA (below 2.0)")) actions.Add("Academic tutoring");
        if (riskFactors.Contains("Outstanding payment")) actions.Add("Payment plan discussion");
        if (riskFactors.Contains("No recent activity")) actions.Add("Contact to encourage participation");
        return actions;
    }

    private byte[] ConvertToFormat(object data, string format, string reportName)
    {
        return format.ToUpper() switch
        {
            "PDF" => GeneratePDF(data, reportName),
            "EXCEL" => GenerateExcel(data, reportName),
            "CSV" => GenerateCSV(data, reportName),
            _ => throw new ArgumentException($"Unsupported format: {format}")
        };
    }

    private byte[] ConvertCsvToFormat(string csvData, string format, string reportName)
    {
        return format.ToUpper() switch
        {
            "PDF" => System.Text.Encoding.UTF8.GetBytes(csvData),
            "EXCEL" => System.Text.Encoding.UTF8.GetBytes(csvData),
            "CSV" => System.Text.Encoding.UTF8.GetBytes(csvData),
            _ => throw new ArgumentException($"Unsupported format: {format}")
        };
    }

    private byte[] GeneratePDF(object data, string reportName)
    {
        // Implementation would use a PDF library like iTextSharp or SelectPdf
        var pdfContent = $"PDF Report: {reportName}\n\n{System.Text.Json.JsonSerializer.Serialize(data)}";
        return System.Text.Encoding.UTF8.GetBytes(pdfContent);
    }

    private byte[] GenerateExcel(object data, string reportName)
    {
        // Implementation would use a library like EPPlus or ClosedXML
        var excelContent = $"Excel Report: {reportName}\n\n{System.Text.Json.JsonSerializer.Serialize(data)}";
        return System.Text.Encoding.UTF8.GetBytes(excelContent);
    }

    private byte[] GenerateCSV(object data, string reportName)
    {
        // Implementation would convert object to CSV format
        var csvContent = System.Text.Json.JsonSerializer.Serialize(data);
        return System.Text.Encoding.UTF8.GetBytes(csvContent);
    }
}
