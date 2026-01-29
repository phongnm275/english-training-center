using EnglishTrainingCenter.Application.Services.Auth;
using EnglishTrainingCenter.Application.Services.Student;
using EnglishTrainingCenter.Application.Services.Course;
using EnglishTrainingCenter.Application.Services.Payment;
using EnglishTrainingCenter.Application.Services.Grade;
using EnglishTrainingCenter.Application.Services.Instructor;
using EnglishTrainingCenter.Application.Services.Dashboard;
using EnglishTrainingCenter.Application.Services.Reporting;
using EnglishTrainingCenter.Application.Services.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnglishTrainingCenter.Application.Extensions;

/// <summary>
/// Extension methods for dependency injection in the Application layer
/// </summary>
public static class ApplicationDependencyInjection
{
    /// <summary>
    /// Register application services
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Authentication Services
        var jwtSettings = configuration.GetSection("Jwt");
        var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured");
        var issuer = jwtSettings["Issuer"] ?? "EnglishTrainingCenter";
        var audience = jwtSettings["Audience"] ?? "EnglishTrainingCenterUsers";
        var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

        services.AddSingleton<ITokenService>(new JwtTokenService(secret, issuer, audience, expiryMinutes));
        services.AddScoped<IAuthService, AuthService>();

        // Register Student Services
        services.AddScoped<IStudentService, StudentService>();

        // Register Course Services
        services.AddScoped<ICourseService, CourseService>();

        // Register Payment Services
        services.AddScoped<IPaymentService, PaymentService>();

        // Register Grade Services
        services.AddScoped<IGradeService, GradeService>();

        // Register Instructor Services
        services.AddScoped<IInstructorService, InstructorService>();

        // Register Dashboard Services
        services.AddScoped<IDashboardService, DashboardService>();

        // Register Reporting Services (Phase 5 Option A)
        services.AddScoped<IReportService, ReportService>();

        // Register Notification Services (Phase 5 Option B)
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
