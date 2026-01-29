using EnglishTrainingCenter.Application.DTOs.Dashboard;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace EnglishTrainingCenter.Application.Services.Dashboard;

/// <summary>
/// Service implementation for admin dashboard analytics and reporting
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<Domain.Entities.Instructor> _instructorRepository;
    private readonly IRepository<Grade> _gradeRepository;
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IRepository<StudentCourse> _studentCourseRepository;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(
        IRepository<Student> studentRepository,
        IRepository<Course> courseRepository,
        IRepository<Domain.Entities.Instructor> instructorRepository,
        IRepository<Grade> gradeRepository,
        IRepository<Payment> paymentRepository,
        IRepository<StudentCourse> studentCourseRepository,
        ILogger<DashboardService> logger)
    {
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _instructorRepository = instructorRepository;
        _gradeRepository = gradeRepository;
        _paymentRepository = paymentRepository;
        _studentCourseRepository = studentCourseRepository;
        _logger = logger;
    }

    public async Task<SystemOverviewDto> GetSystemOverviewAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving system overview");

            var students = await _studentRepository.GetAsync();
            var courses = await _courseRepository.GetAsync();
            var instructors = await _instructorRepository.GetAsync();
            var enrollments = await _studentCourseRepository.GetAsync();
            var payments = await _paymentRepository.GetAsync();

            var totalRevenue = payments.Sum(p => p.Amount);
            var pendingPayments = payments.Where(p => p.Status == "Pending").Sum(p => p.Amount);
            var completedPayments = payments.Where(p => p.Status == "Completed").Sum(p => p.Amount);

            return new SystemOverviewDto
            {
                TotalStudents = students.Count(),
                ActiveStudents = students.Count(s => s.IsActive),
                InactiveStudents = students.Count(s => !s.IsActive),
                TotalCourses = courses.Count(),
                ActiveCourses = courses.Count(c => c.IsActive),
                TotalInstructors = instructors.Count(),
                ActiveInstructors = instructors.Count(i => i.IsActive),
                TotalEnrollments = enrollments.Count(),
                TotalRevenue = totalRevenue,
                PendingPayments = pendingPayments,
                CompletedPayments = completedPayments,
                OverviewDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving system overview: {ex.Message}");
            throw;
        }
    }

    public async Task<StudentStatisticsDto> GetStudentStatisticsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving student statistics");

            var students = await _studentRepository.GetAsync();
            var enrollments = await _studentCourseRepository.GetAsync();
            var grades = await _gradeRepository.GetAsync();

            var totalStudents = students.Count();
            var newStudentsThisMonth = students.Count(s => s.CreatedAt >= DateTime.UtcNow.AddMonths(-1));
            var averageGPA = grades.Any() ? Math.Round(grades.Average(g => GetGPAPoints(g.Grade)), 2) : 0;
            var enrollmentRate = totalStudents > 0 ? Math.Round(((double)enrollments.Count() / totalStudents) * 100, 2) : 0;
            var completionRate = CalculateCompletionRate(students, enrollments);

            return new StudentStatisticsDto
            {
                TotalStudents = totalStudents,
                ActiveStudents = students.Count(s => s.IsActive),
                InactiveStudents = students.Count(s => !s.IsActive),
                NewStudentsThisMonth = newStudentsThisMonth,
                EnrollmentRate = enrollmentRate,
                AverageGPA = averageGPA,
                CompletionRate = completionRate,
                TotalEnrollments = enrollments.Count(),
                AverageStudentAge = students.Any() ? Math.Round(students.Average(s => CalculateAge(s.DateOfBirth)), 1) : 0,
                StatisticsDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving student statistics: {ex.Message}");
            throw;
        }
    }

    public async Task<CourseStatisticsDto> GetCourseStatisticsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving course statistics");

            var courses = await _courseRepository.GetAsync();
            var enrollments = await _studentCourseRepository.GetAsync();

            var totalCourses = courses.Count();
            var averageCapacity = courses.Any() ? Math.Round(courses.Average(c => c.MaxCapacity), 1) : 0;
            var averageEnrollment = courses.Any() ? Math.Round((double)enrollments.Count() / Math.Max(totalCourses, 1), 1) : 0;
            var capacityUtilization = courses.Any() ? 
                Math.Round((enrollments.Count() / (double)(courses.Sum(c => c.MaxCapacity) ?? 1)) * 100, 2) : 0;

            return new CourseStatisticsDto
            {
                TotalCourses = totalCourses,
                ActiveCourses = courses.Count(c => c.IsActive),
                InactiveCourses = courses.Count(c => !c.IsActive),
                AverageCapacity = averageCapacity,
                AverageEnrollment = averageEnrollment,
                CapacityUtilization = capacityUtilization,
                TotalEnrollments = enrollments.Count(),
                CourseLevels = courses.GroupBy(c => c.Level).ToDictionary(g => g.Key, g => g.Count()),
                StatisticsDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving course statistics: {ex.Message}");
            throw;
        }
    }

    public async Task<InstructorStatisticsDto> GetInstructorStatisticsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving instructor statistics");

            var instructors = await _instructorRepository.GetAsync();

            var totalInstructors = instructors.Count();
            var activeInstructors = instructors.Count(i => i.IsActive);
            var averageSalary = instructors.Any() ? Math.Round(instructors.Average(i => i.BaseSalary), 2) : 0;
            var averageExperience = instructors.Any() ? Math.Round(instructors.Average(i => i.YearsOfExperience), 1) : 0;

            return new InstructorStatisticsDto
            {
                TotalInstructors = totalInstructors,
                ActiveInstructors = activeInstructors,
                InactiveInstructors = totalInstructors - activeInstructors,
                AverageSalary = averageSalary,
                AverageExperience = averageExperience,
                QualificationBreakdown = instructors.GroupBy(i => i.Qualification).ToDictionary(g => g.Key, g => g.Count()),
                StatisticsDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving instructor statistics: {ex.Message}");
            throw;
        }
    }

    public async Task<FinancialMetricsDto> GetFinancialMetricsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving financial metrics");

            var payments = await _paymentRepository.GetAsync();
            var students = await _studentRepository.GetAsync();

            var totalRevenue = payments.Sum(p => p.Amount);
            var completedPayments = payments.Where(p => p.Status == "Completed").Sum(p => p.Amount);
            var pendingPayments = payments.Where(p => p.Status == "Pending").Sum(p => p.Amount);
            var failedPayments = payments.Where(p => p.Status == "Failed").Sum(p => p.Amount);
            var averageTransactionValue = payments.Any() ? Math.Round(payments.Average(p => p.Amount), 2) : 0;

            var thisMonthRevenue = payments.Where(p => p.PaymentDate >= DateTime.UtcNow.AddMonths(-1) && p.Status == "Completed").Sum(p => p.Amount);
            var lastMonthRevenue = payments.Where(p => p.PaymentDate >= DateTime.UtcNow.AddMonths(-2) && p.PaymentDate < DateTime.UtcNow.AddMonths(-1) && p.Status == "Completed").Sum(p => p.Amount);

            return new FinancialMetricsDto
            {
                TotalRevenue = totalRevenue,
                CompletedPayments = completedPayments,
                PendingPayments = pendingPayments,
                FailedPayments = failedPayments,
                AverageTransactionValue = averageTransactionValue,
                CollectionRate = payments.Any() ? Math.Round((completedPayments / totalRevenue) * 100, 2) : 0,
                ThisMonthRevenue = thisMonthRevenue,
                LastMonthRevenue = lastMonthRevenue,
                RevenueGrowth = lastMonthRevenue > 0 ? Math.Round(((thisMonthRevenue - lastMonthRevenue) / lastMonthRevenue) * 100, 2) : 0,
                OutstandingBalance = CalculateOutstandingBalance(students, payments),
                MetricsDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving financial metrics: {ex.Message}");
            throw;
        }
    }

    public async Task<AcademicMetricsDto> GetAcademicMetricsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving academic metrics");

            var grades = await _gradeRepository.GetAsync();

            var averageGPA = grades.Any() ? Math.Round(grades.Average(g => GetGPAPoints(g.Grade)), 2) : 0;
            var gradeDistribution = grades.GroupBy(g => g.Grade).ToDictionary(g => g.Key, g => g.Count());
            var passingRate = grades.Any() ? 
                Math.Round((grades.Count(g => GetGPAPoints(g.Grade) >= 1.0m) / (double)grades.Count()) * 100, 2) : 0;
            var excellentRate = grades.Any() ? 
                Math.Round((grades.Count(g => g.Grade == "A") / (double)grades.Count()) * 100, 2) : 0;

            return new AcademicMetricsDto
            {
                TotalGrades = grades.Count(),
                AverageGPA = averageGPA,
                PassingRate = passingRate,
                ExcellentRate = excellentRate,
                GradeDistribution = gradeDistribution,
                StudentsWithAGrades = grades.Count(g => g.Grade == "A"),
                StudentsWithBGrades = grades.Count(g => g.Grade == "B"),
                StudentsWithCGrades = grades.Count(g => g.Grade == "C"),
                StudentsWithDGrades = grades.Count(g => g.Grade == "D"),
                StudentsWithFGrades = grades.Count(g => g.Grade == "F"),
                MetricsDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving academic metrics: {ex.Message}");
            throw;
        }
    }

    public async Task<RevenueReportDto> GetRevenueReportAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            _logger.LogInformation("Retrieving revenue report");

            var payments = await _paymentRepository.GetAsync();
            
            var start = startDate ?? DateTime.UtcNow.AddMonths(-12);
            var end = endDate ?? DateTime.UtcNow;

            var filteredPayments = payments.Where(p => p.PaymentDate >= start && p.PaymentDate <= end);

            var totalRevenue = filteredPayments.Sum(p => p.Amount);
            var completedRevenue = filteredPayments.Where(p => p.Status == "Completed").Sum(p => p.Amount);
            var pendingRevenue = filteredPayments.Where(p => p.Status == "Pending").Sum(p => p.Amount);

            var monthlyRevenue = filteredPayments.GroupBy(p => p.PaymentDate.ToString("yyyy-MM"))
                .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount));

            return new RevenueReportDto
            {
                TotalRevenue = totalRevenue,
                CompletedRevenue = completedRevenue,
                PendingRevenue = pendingRevenue,
                TransactionCount = filteredPayments.Count(),
                CompletedTransactions = filteredPayments.Count(p => p.Status == "Completed"),
                PendingTransactions = filteredPayments.Count(p => p.Status == "Pending"),
                FailedTransactions = filteredPayments.Count(p => p.Status == "Failed"),
                AverageTransaction = filteredPayments.Any() ? Math.Round(filteredPayments.Average(p => p.Amount), 2) : 0,
                MonthlyRevenue = monthlyRevenue,
                ReportPeriodStart = start,
                ReportPeriodEnd = end,
                GeneratedDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving revenue report: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<CoursePerformanceDto>> GetCoursePerformanceReportAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving course performance report");

            var courses = await _courseRepository.GetAsync();
            var enrollments = await _studentCourseRepository.GetAsync();
            var grades = await _gradeRepository.GetAsync();

            var report = new List<CoursePerformanceDto>();

            foreach (var course in courses)
            {
                var courseEnrollments = enrollments.Where(e => e.CourseId == course.Id).Count();
                var courseGrades = grades.Where(g => g.CourseId == course.Id).ToList();
                var avgGrade = courseGrades.Any() ? Math.Round(courseGrades.Average(g => GetGPAPoints(g.Grade)), 2) : 0;
                var enrollmentRate = course.MaxCapacity.HasValue && course.MaxCapacity > 0 ? 
                    Math.Round((courseEnrollments / (double)course.MaxCapacity.Value) * 100, 2) : 0;

                report.Add(new CoursePerformanceDto
                {
                    CourseId = course.Id,
                    CourseName = course.Name,
                    CourseCode = course.Code,
                    TotalEnrollments = courseEnrollments,
                    Capacity = course.MaxCapacity ?? 0,
                    EnrollmentRate = enrollmentRate,
                    AverageStudentGPA = avgGrade,
                    TotalGradesRecorded = courseGrades.Count(),
                    PassingStudents = courseGrades.Count(g => GetGPAPoints(g.Grade) >= 1.0m),
                    InstructorCount = 0, // Would need additional query
                    ReportDate = DateTime.UtcNow
                });
            }

            return report.OrderByDescending(r => r.EnrollmentRate);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving course performance report: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<EnrollmentTrendDto>> GetEnrollmentTrendsAsync(int months = 12)
    {
        try
        {
            _logger.LogInformation($"Retrieving enrollment trends for {months} months");

            var students = await _studentRepository.GetAsync();

            var trends = new List<EnrollmentTrendDto>();

            for (int i = months - 1; i >= 0; i--)
            {
                var monthStart = DateTime.UtcNow.AddMonths(-i).AddDays(-DateTime.UtcNow.Day + 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var monthStudents = students.Count(s => s.CreatedAt >= monthStart && s.CreatedAt <= monthEnd);

                trends.Add(new EnrollmentTrendDto
                {
                    Month = monthStart.ToString("yyyy-MM"),
                    NewEnrollments = monthStudents,
                    TrendDate = monthStart
                });
            }

            return trends;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving enrollment trends: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<PaymentTrendDto>> GetPaymentTrendsAsync(int months = 12)
    {
        try
        {
            _logger.LogInformation($"Retrieving payment trends for {months} months");

            var payments = await _paymentRepository.GetAsync();

            var trends = new List<PaymentTrendDto>();

            for (int i = months - 1; i >= 0; i--)
            {
                var monthStart = DateTime.UtcNow.AddMonths(-i).AddDays(-DateTime.UtcNow.Day + 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var monthPayments = payments.Where(p => p.PaymentDate >= monthStart && p.PaymentDate <= monthEnd);
                var monthRevenue = monthPayments.Where(p => p.Status == "Completed").Sum(p => p.Amount);

                trends.Add(new PaymentTrendDto
                {
                    Month = monthStart.ToString("yyyy-MM"),
                    Revenue = monthRevenue,
                    TransactionCount = monthPayments.Count(),
                    CompletedTransactions = monthPayments.Count(p => p.Status == "Completed"),
                    TrendDate = monthStart
                });
            }

            return trends;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving payment trends: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<TopPerformerDto>> GetTopStudentsAsync(int count = 10)
    {
        try
        {
            _logger.LogInformation($"Retrieving top {count} students");

            var students = await _studentRepository.GetAsync();
            var grades = await _gradeRepository.GetAsync();

            var topStudents = new List<TopPerformerDto>();

            foreach (var student in students)
            {
                var studentGrades = grades.Where(g => g.StudentId == student.Id).ToList();
                if (studentGrades.Any())
                {
                    var gpa = Math.Round(studentGrades.Average(g => GetGPAPoints(g.Grade)), 2);

                    topStudents.Add(new TopPerformerDto
                    {
                        StudentId = student.Id,
                        StudentName = $"{student.FirstName} {student.LastName}",
                        Email = student.Email,
                        GPA = gpa,
                        CoursesEnrolled = studentGrades.Select(g => g.CourseId).Distinct().Count(),
                        AverageScore = Math.Round(studentGrades.Average(g => g.NumericScore ?? 0), 2)
                    });
                }
            }

            return topStudents.OrderByDescending(s => s.GPA).Take(count);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving top students: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<AtRiskStudentDto>> GetAtRiskStudentsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving at-risk students");

            var students = await _studentRepository.GetAsync();
            var grades = await _gradeRepository.GetAsync();
            var payments = await _paymentRepository.GetAsync();

            var atRiskStudents = new List<AtRiskStudentDto>();

            foreach (var student in students.Where(s => s.IsActive))
            {
                var studentGrades = grades.Where(g => g.StudentId == student.Id).ToList();
                var studentPayments = payments.Where(p => p.StudentId == student.Id);

                var hasLowGrades = studentGrades.Any() && studentGrades.Average(g => GetGPAPoints(g.Grade)) < 2.0m;
                var hasPendingPayments = studentPayments.Any(p => p.Status == "Pending");
                var hasFailedPayments = studentPayments.Any(p => p.Status == "Failed");

                if (hasLowGrades || hasPendingPayments || hasFailedPayments)
                {
                    var gpa = studentGrades.Any() ? Math.Round(studentGrades.Average(g => GetGPAPoints(g.Grade)), 2) : 0;

                    atRiskStudents.Add(new AtRiskStudentDto
                    {
                        StudentId = student.Id,
                        StudentName = $"{student.FirstName} {student.LastName}",
                        Email = student.Email,
                        CurrentGPA = gpa,
                        RiskFactors = new List<string>
                        {
                            hasLowGrades ? "Low Academic Performance" : null,
                            hasPendingPayments ? "Pending Payment" : null,
                            hasFailedPayments ? "Failed Payment" : null
                        }.Where(x => x != null).ToList(),
                        EnrollmentCount = grades.Where(g => g.StudentId == student.Id).Select(g => g.CourseId).Distinct().Count()
                    });
                }
            }

            return atRiskStudents.OrderBy(s => s.CurrentGPA);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving at-risk students: {ex.Message}");
            throw;
        }
    }

    public async Task<SystemHealthDto> GetSystemHealthAsync()
    {
        try
        {
            _logger.LogInformation("Checking system health");

            var students = await _studentRepository.GetAsync();
            var courses = await _courseRepository.GetAsync();
            var instructors = await _instructorRepository.GetAsync();
            var grades = await _gradeRepository.GetAsync();

            var dataQualityScore = CalculateDataQualityScore(students, courses, instructors, grades);
            var systemLoadPercentage = Math.Round((students.Count() / 10000.0) * 100, 2); // Assume 10K capacity

            return new SystemHealthDto
            {
                SystemStatus = dataQualityScore > 80 ? "Healthy" : dataQualityScore > 50 ? "Warning" : "Critical",
                DataQualityScore = dataQualityScore,
                SystemLoadPercentage = systemLoadPercentage,
                ActiveRecordCount = students.Count(s => s.IsActive) + courses.Count(c => c.IsActive) + instructors.Count(i => i.IsActive),
                InactiveRecordCount = students.Count(s => !s.IsActive) + courses.Count(c => !c.IsActive) + instructors.Count(i => !i.IsActive),
                LastHealthCheckTime = DateTime.UtcNow,
                UpcomingMaintenanceRequired = dataQualityScore < 60
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking system health: {ex.Message}");
            throw;
        }
    }

    public async Task<UserActivityDto> GetUserActivityAsync(int days = 30)
    {
        try
        {
            _logger.LogInformation($"Retrieving user activity for last {days} days");

            var students = await _studentRepository.GetAsync();

            var periodStart = DateTime.UtcNow.AddDays(-days);

            var activeUsers = students.Count(s => s.UpdatedAt >= periodStart);
            var newUsers = students.Count(s => s.CreatedAt >= periodStart);

            return new UserActivityDto
            {
                PeriodDays = days,
                ActiveUsers = activeUsers,
                NewUsers = newUsers,
                TotalUsers = students.Count(),
                UserEngagementPercentage = students.Any() ? Math.Round((activeUsers / (double)students.Count()) * 100, 2) : 0,
                LastActivityDate = students.Any() ? students.Max(s => s.UpdatedAt) : DateTime.UtcNow,
                ReportDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving user activity: {ex.Message}");
            throw;
        }
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving dashboard summary");

            var overview = await GetSystemOverviewAsync();
            var financial = await GetFinancialMetricsAsync();
            var academic = await GetAcademicMetricsAsync();
            var topStudents = (await GetTopStudentsAsync(5)).ToList();
            var atRiskStudents = (await GetAtRiskStudentsAsync()).ToList();

            return new DashboardSummaryDto
            {
                TotalStudents = overview.TotalStudents,
                ActiveStudents = overview.ActiveStudents,
                TotalCourses = overview.TotalCourses,
                TotalInstructors = overview.TotalInstructors,
                TotalRevenue = financial.TotalRevenue,
                PendingPayments = financial.PendingPayments,
                AverageGPA = academic.AverageGPA,
                PassingRate = academic.PassingRate,
                TopPerformers = topStudents,
                AtRiskStudentCount = atRiskStudents.Count,
                SystemStatus = (await GetSystemHealthAsync()).SystemStatus,
                LastUpdated = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving dashboard summary: {ex.Message}");
            throw;
        }
    }

    public async Task<FinancialBreakdownDto> GetFinancialBreakdownAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving financial breakdown");

            var payments = await _paymentRepository.GetAsync();

            var byMethod = payments.GroupBy(p => p.Method).ToDictionary(g => g.Key, g => g.Sum(p => p.Amount));
            var byStatus = payments.GroupBy(p => p.Status).ToDictionary(g => g.Key, g => g.Sum(p => p.Amount));

            return new FinancialBreakdownDto
            {
                TotalRevenue = payments.Sum(p => p.Amount),
                RevenueByPaymentMethod = byMethod,
                RevenueByStatus = byStatus,
                TopPaymentMethods = byMethod.OrderByDescending(x => x.Value).Take(5).ToDictionary(x => x.Key, x => x.Value),
                RefundsIssued = payments.Where(p => p.Status == "Refunded").Sum(p => p.Amount),
                AveragePaymentAmount = payments.Any() ? Math.Round(payments.Average(p => p.Amount), 2) : 0,
                BreakdownDate = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving financial breakdown: {ex.Message}");
            throw;
        }
    }

    // Helper methods
    private decimal GetGPAPoints(string grade)
    {
        return grade?.ToUpper() switch
        {
            "A" => 4.0m,
            "B" => 3.0m,
            "C" => 2.0m,
            "D" => 1.0m,
            "F" => 0.0m,
            _ => 0.0m
        };
    }

    private int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    private double CalculateCompletionRate(IEnumerable<Student> students, IEnumerable<StudentCourse> enrollments)
    {
        if (!students.Any()) return 0;
        
        var enrolledStudents = students.Count(s => enrollments.Any(e => e.StudentId == s.Id));
        return Math.Round((enrolledStudents / (double)students.Count()) * 100, 2);
    }

    private decimal CalculateOutstandingBalance(IEnumerable<Student> students, IEnumerable<Payment> payments)
    {
        var totalCourseEees = 0m; // Would need course pricing
        var totalPaid = payments.Where(p => p.Status == "Completed").Sum(p => p.Amount);
        return Math.Max(0, totalCourseEees - totalPaid);
    }

    private double CalculateDataQualityScore(IEnumerable<Student> students, IEnumerable<Course> courses, 
        IEnumerable<Domain.Entities.Instructor> instructors, IEnumerable<Grade> grades)
    {
        var score = 0.0;

        // Student data quality (25%)
        var studentScore = students.Count(s => !string.IsNullOrEmpty(s.FirstName) && !string.IsNullOrEmpty(s.LastName) && !string.IsNullOrEmpty(s.Email));
        score += (students.Any() ? (studentScore / (double)students.Count()) * 25 : 0);

        // Course data quality (25%)
        var courseScore = courses.Count(c => !string.IsNullOrEmpty(c.Name) && !string.IsNullOrEmpty(c.Code) && !string.IsNullOrEmpty(c.Description));
        score += (courses.Any() ? (courseScore / (double)courses.Count()) * 25 : 0);

        // Instructor data quality (25%)
        var instructorScore = instructors.Count(i => !string.IsNullOrEmpty(i.FirstName) && !string.IsNullOrEmpty(i.Email));
        score += (instructors.Any() ? (instructorScore / (double)instructors.Count()) * 25 : 0);

        // Grade data quality (25%)
        var gradeScore = grades.Count(g => !string.IsNullOrEmpty(g.Grade) && g.NumericScore >= 0 && g.NumericScore <= 100);
        score += (grades.Any() ? (gradeScore / (double)grades.Count()) * 25 : 0);

        return Math.Round(score, 2);
    }
}
