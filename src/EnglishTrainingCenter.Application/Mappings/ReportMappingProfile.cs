using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Reporting;

namespace EnglishTrainingCenter.Application.Mappings;

/// <summary>
/// AutoMapper profile for reporting DTOs
/// </summary>
public class ReportMappingProfile : Profile
{
    public ReportMappingProfile()
    {
        // Report DTO mappings
        CreateMap<ReportDto, ReportDto>()
            .ReverseMap();

        CreateMap<ReportFilterDto, ReportFilterDto>()
            .ReverseMap();

        CreateMap<ReportTypeDto, ReportTypeDto>()
            .ReverseMap();

        // Forecast DTO mappings
        CreateMap<EnrollmentForecastDto, EnrollmentForecastDto>()
            .ReverseMap();

        CreateMap<RevenueForecastDto, RevenueForecastDto>()
            .ReverseMap();

        CreateMap<PerformanceForecastDto, PerformanceForecastDto>()
            .ReverseMap();

        // Schedule DTO mappings
        CreateMap<ScheduleReportRequestDto, ScheduleReportRequestDto>()
            .ReverseMap();

        CreateMap<ScheduledReportDto, ScheduledReportDto>()
            .ReverseMap();

        CreateMap<ReportHistoryDto, ReportHistoryDto>()
            .ReverseMap();

        // Specific report DTO mappings
        CreateMap<StudentEnrollmentReportDto, StudentEnrollmentReportDto>()
            .ReverseMap();

        CreateMap<FinancialReportDto, FinancialReportDto>()
            .ReverseMap();

        CreateMap<AcademicReportDto, AcademicReportDto>()
            .ReverseMap();

        CreateMap<StudentGradeDto, StudentGradeDto>()
            .ReverseMap();

        CreateMap<AtRiskStudentReportDto, AtRiskStudentReportDto>()
            .ReverseMap();
    }
}
