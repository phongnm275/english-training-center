using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.StudentAdvanced;
using EnglishTrainingCenter.Domain.Entities;

namespace EnglishTrainingCenter.Application.Mappings;

/// <summary>
/// AutoMapper profile for Student Advanced features
/// Maps between domain entities and student advanced DTOs
/// </summary>
public class StudentAdvancedMappingProfile : Profile
{
    public StudentAdvancedMappingProfile()
    {
        // ============================================================================
        // ATTENDANCE MAPPINGS
        // ============================================================================

        CreateMap<Grade, AttendanceRecordDto>()
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
            .ForMember(dest => dest.AttendanceDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.Status, opt => opt.Ignore()) // Will be set in service
            .ForMember(dest => dest.RecordedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ReverseMap();

        CreateMap<Student, AttendanceStatisticsDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ByCourse, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Course, CourseAttendanceDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseName))
            .ReverseMap();

        CreateMap<Course, CourseAttendanceSummaryDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseName))
            .ForMember(dest => dest.StudentDetails, opt => opt.Ignore())
            .ForMember(dest => dest.AttendanceDistribution, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<StudentCourse, StudentAttendanceDetailDto>()
            .ReverseMap();

        // ============================================================================
        // PERFORMANCE & ANALYTICS MAPPINGS
        // ============================================================================

        CreateMap<Grade, PerformancePredictionDto>()
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.StudentId))
            .ForMember(dest => dest.PredictedGPA, opt => opt.MapFrom(src => (decimal)src.Score / 100 * 4.0))
            .ForMember(dest => dest.ConfidenceLevel, opt => opt.Ignore())
            .ForMember(dest => dest.ProbabilityOfPassing, opt => opt.Ignore())
            .ForMember(dest => dest.RiskFactors, opt => opt.Ignore())
            .ForMember(dest => dest.Strengths, opt => opt.Ignore())
            .ForMember(dest => dest.PredictionDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.PredictionBasis, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<StudentCourse, StudentPerformancePredictionDto>()
            .ReverseMap();

        CreateMap<Student, AtRiskStudentDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CurrentGPA, opt => opt.Ignore())
            .ForMember(dest => dest.RiskLevel, opt => opt.Ignore())
            .ForMember(dest => dest.RiskScore, opt => opt.Ignore())
            .ForMember(dest => dest.RiskFactors, opt => opt.Ignore())
            .ForMember(dest => dest.CoursesAtRisk, opt => opt.Ignore())
            .ForMember(dest => dest.DaysSinceLastActivity, opt => opt.Ignore())
            .ForMember(dest => dest.RecommendedInterventions, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Student, StudentPerformanceAnalysisDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CurrentGPA, opt => opt.Ignore())
            .ForMember(dest => dest.GPATrend, opt => opt.Ignore())
            .ForMember(dest => dest.AttendancePercentage, opt => opt.Ignore())
            .ForMember(dest => dest.EngagementScore, opt => opt.Ignore())
            .ForMember(dest => dest.AverageStudyHours, opt => opt.Ignore())
            .ForMember(dest => dest.CoursePerformance, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Course, CoursePerformanceDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseName))
            .ReverseMap();

        CreateMap<StudentCourse, PerformanceBenchmarkDto>()
            .ReverseMap();

        CreateMap<Student, StudentProgressTrackingDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.OverallProgress, opt => opt.Ignore())
            .ForMember(dest => dest.CourseProgress, opt => opt.Ignore())
            .ForMember(dest => dest.Milestones, opt => opt.Ignore())
            .ForMember(dest => dest.ProgressHistory, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectedCompletion, opt => opt.Ignore())
            .ReverseMap();

        // ============================================================================
        // LEARNING PATH MAPPINGS
        // ============================================================================

        CreateMap<Course, CourseRecommendationDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseName))
            .ForMember(dest => dest.RecommendationReason, opt => opt.Ignore())
            .ForMember(dest => dest.RelevanceScore, opt => opt.Ignore())
            .ForMember(dest => dest.Prerequisites, opt => opt.Ignore())
            .ForMember(dest => dest.ExpectedDuration, opt => opt.Ignore())
            .ForMember(dest => dest.SkillsDeveloped, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<StudentCourse, CompletedCourseDto>()
            .ReverseMap();

        CreateMap<StudentCourse, InProgressCourseDto>()
            .ReverseMap();

        CreateMap<StudentCourse, PendingCourseDto>()
            .ReverseMap();

        // ============================================================================
        // PREREQUISITE MANAGEMENT MAPPINGS
        // ============================================================================

        CreateMap<Course, PrerequisiteCourseDto>()
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.CourseName))
            .ForMember(dest => dest.Reason, opt => opt.Ignore())
            .ForMember(dest => dest.Offering, opt => opt.Ignore())
            .ForMember(dest => dest.DurationWeeks, opt => opt.Ignore())
            .ForMember(dest => dest.SkillsDeveloped, opt => opt.Ignore())
            .ReverseMap();

        // ============================================================================
        // STUDY PROGRESS & ENGAGEMENT MAPPINGS
        // ============================================================================

        CreateMap<StudentCourse, StudyProgressDto>()
            .ForMember(dest => dest.CourseName, opt => opt.Ignore())
            .ForMember(dest => dest.CompletionPercentage, opt => opt.Ignore())
            .ForMember(dest => dest.ModulesCompleted, opt => opt.Ignore())
            .ForMember(dest => dest.TotalModules, opt => opt.Ignore())
            .ForMember(dest => dest.AssignmentsCompleted, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAssignments, opt => opt.Ignore())
            .ForMember(dest => dest.AverageAssignmentScore, opt => opt.Ignore())
            .ForMember(dest => dest.QuizzesCompleted, opt => opt.Ignore())
            .ForMember(dest => dest.AverageQuizScore, opt => opt.Ignore())
            .ForMember(dest => dest.CurrentGrade, opt => opt.Ignore())
            .ForMember(dest => dest.DaysRemaining, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectedFinalGrade, opt => opt.Ignore())
            .ForMember(dest => dest.StudyPace, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Student, StudentEngagementDto>()
            .ForMember(dest => dest.OverallEngagementScore, opt => opt.Ignore())
            .ForMember(dest => dest.LearningActivityScore, opt => opt.Ignore())
            .ForMember(dest => dest.ForumParticipationScore, opt => opt.Ignore())
            .ForMember(dest => dest.PeerCollaborationScore, opt => opt.Ignore())
            .ForMember(dest => dest.AssignmentSubmissionScore, opt => opt.Ignore())
            .ForMember(dest => dest.LastLoginDate, opt => opt.Ignore())
            .ForMember(dest => dest.ResourceAccessFrequency, opt => opt.Ignore())
            .ForMember(dest => dest.DaysSinceLastLogin, opt => opt.Ignore())
            .ForMember(dest => dest.Trend, opt => opt.Ignore())
            .ForMember(dest => dest.RiskLevel, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<Grade, ResourceUtilizationDto>()
            .ReverseMap();

        // ============================================================================
        // STUDY GROUP & COLLABORATION MAPPINGS
        // ============================================================================

        CreateMap<Student, StudyGroupMemberDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CourseGrade, opt => opt.Ignore())
            .ForMember(dest => dest.JoinDate, opt => opt.Ignore())
            .ForMember(dest => dest.LastActiveDate, opt => opt.Ignore())
            .ForMember(dest => dest.ContributionLevel, opt => opt.Ignore())
            .ReverseMap();
    }
}
