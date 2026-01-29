using EnglishTrainingCenter.Application.DTOs.StudentAdvanced;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnglishTrainingCenter.Application.Services.StudentAdvanced;

/// <summary>
/// Service interface for advanced student management features
/// Includes attendance tracking, performance prediction, learning paths, and analytics
/// </summary>
public interface IStudentAdvancedService
{
    // ============================================================================
    // ATTENDANCE TRACKING (6 methods)
    // ============================================================================

    /// <summary>
    /// Mark student attendance for a specific class
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <param name="attendanceDate">Date of class</param>
    /// <param name="status">Attendance status (Present, Absent, Late, Excused)</param>
    /// <returns>Attendance record ID</returns>
    Task<int> MarkAttendanceAsync(int studentId, int courseId, DateTime attendanceDate, string status);

    /// <summary>
    /// Get attendance records for a student in a course
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID (optional for all courses)</param>
    /// <returns>List of attendance records</returns>
    Task<IEnumerable<AttendanceRecordDto>> GetAttendanceRecordsAsync(int studentId, int? courseId = null);

    /// <summary>
    /// Get attendance statistics for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Attendance statistics with rates and trends</returns>
    Task<AttendanceStatisticsDto> GetAttendanceStatisticsAsync(int studentId);

    /// <summary>
    /// Get attendance summary for a course
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Course attendance summary with student details</returns>
    Task<CourseAttendanceSummaryDto> GetCourseAttendanceSummaryAsync(int courseId);

    /// <summary>
    /// Bulk mark attendance for multiple students
    /// </summary>
    /// <param name="bulkAttendance">List of attendance records to mark</param>
    /// <returns>Number of successfully recorded attendances</returns>
    Task<int> BulkMarkAttendanceAsync(List<BulkAttendanceDto> bulkAttendance);

    /// <summary>
    /// Get low attendance warnings for students
    /// </summary>
    /// <param name="minimumAttendanceRate">Minimum attendance rate threshold (default 75%)</param>
    /// <returns>List of students with low attendance</returns>
    Task<IEnumerable<LowAttendanceWarningDto>> GetLowAttendanceWarningsAsync(decimal minimumAttendanceRate = 0.75m);

    // ============================================================================
    // PERFORMANCE PREDICTION & ANALYTICS (7 methods)
    // ============================================================================

    /// <summary>
    /// Predict student performance based on current data
    /// Uses historical data, attendance, and engagement metrics
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Performance prediction with confidence level</returns>
    Task<PerformancePredictionDto> PredictStudentPerformanceAsync(int studentId);

    /// <summary>
    /// Get performance prediction for entire course
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>List of performance predictions for all enrolled students</returns>
    Task<IEnumerable<StudentPerformancePredictionDto>> GetCoursePerformancePredictionsAsync(int courseId);

    /// <summary>
    /// Identify at-risk students (likely to fail or struggle)
    /// </summary>
    /// <param name="riskThreshold">Risk threshold (0.0-1.0, default 0.5)</param>
    /// <returns>List of at-risk students with risk factors</returns>
    Task<IEnumerable<AtRiskStudentDto>> GetAtRiskStudentsAsync(decimal riskThreshold = 0.5m);

    /// <summary>
    /// Get detailed performance analysis for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Comprehensive performance analysis with trends</returns>
    Task<StudentPerformanceAnalysisDto> GetStudentPerformanceAnalysisAsync(int studentId);

    /// <summary>
    /// Get recommended interventions for at-risk student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>List of recommended interventions and support</returns>
    Task<InterventionRecommendationsDto> GetInterventionRecommendationsAsync(int studentId);

    /// <summary>
    /// Get performance benchmarks and comparisons
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Performance benchmarks compared to peers</returns>
    Task<PerformanceBenchmarkDto> GetPerformanceBenchmarkAsync(int studentId, int courseId);

    /// <summary>
    /// Get student progress tracking over time
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Progress metrics with historical trend</returns>
    Task<StudentProgressTrackingDto> GetStudentProgressTrackingAsync(int studentId);

    // ============================================================================
    // LEARNING PATHS & CUSTOMIZATION (5 methods)
    // ============================================================================

    /// <summary>
    /// Create a customized learning path for a student
    /// </summary>
    /// <param name="request">Learning path creation request</param>
    /// <returns>Created learning path ID</returns>
    Task<int> CreateLearningPathAsync(CreateLearningPathRequestDto request);

    /// <summary>
    /// Get learning path for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Current learning path with milestones and courses</returns>
    Task<LearningPathDto> GetLearningPathAsync(int studentId);

    /// <summary>
    /// Update learning path based on student progress
    /// </summary>
    /// <param name="learningPathId">Learning path ID</param>
    /// <param name="request">Update request with new milestones</param>
    /// <returns>Updated learning path</returns>
    Task<LearningPathDto> UpdateLearningPathAsync(int learningPathId, UpdateLearningPathRequestDto request);

    /// <summary>
    /// Recommend courses for student based on learning path and abilities
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>List of recommended courses with reasoning</returns>
    Task<IEnumerable<CourseRecommendationDto>> GetCourseRecommendationsAsync(int studentId);

    /// <summary>
    /// Track learning path progress and milestones
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Learning path progress with completed/pending milestones</returns>
    Task<LearningPathProgressDto> GetLearningPathProgressAsync(int studentId);

    // ============================================================================
    // PREREQUISITE MANAGEMENT (3 methods)
    // ============================================================================

    /// <summary>
    /// Check if student has met prerequisites for a course
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID to check</param>
    /// <returns>Prerequisite check result with missing requirements if any</returns>
    Task<PrerequisiteCheckDto> CheckPrerequisitesAsync(int studentId, int courseId);

    /// <summary>
    /// Get prerequisite recommendations for student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>List of prerequisite courses to take</returns>
    Task<IEnumerable<PrerequisiteCourseDto>> GetPrerequisiteRecommendationsAsync(int studentId);

    /// <summary>
    /// Waive prerequisites for a student (admin only)
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <param name="reason">Reason for waiving</param>
    /// <returns>Success status</returns>
    Task<bool> WaivePrerequisiteAsync(int studentId, int courseId, string reason);

    // ============================================================================
    // STUDY PROGRESS & ENGAGEMENT (4 methods)
    // ============================================================================

    /// <summary>
    /// Get study progress for a student in a course
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Detailed study progress with completion percentage</returns>
    Task<StudyProgressDto> GetStudyProgressAsync(int studentId, int courseId);

    /// <summary>
    /// Get engagement metrics for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Engagement scores and activity metrics</returns>
    Task<StudentEngagementDto> GetStudentEngagementAsync(int studentId);

    /// <summary>
    /// Get study time analytics for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="startDate">Start date for analytics</param>
    /// <param name="endDate">End date for analytics</param>
    /// <returns>Study time analysis with patterns and recommendations</returns>
    Task<StudyTimeAnalyticsDto> GetStudyTimeAnalyticsAsync(int studentId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Get resource utilization by student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Resource usage statistics and recommendations</returns>
    Task<ResourceUtilizationDto> GetResourceUtilizationAsync(int studentId);

    // ============================================================================
    // GROUP LEARNING & COLLABORATION (2 methods)
    // ============================================================================

    /// <summary>
    /// Create or join a study group for a course
    /// </summary>
    /// <param name="request">Study group creation/join request</param>
    /// <returns>Study group ID</returns>
    Task<int> ManageStudyGroupAsync(ManageStudyGroupRequestDto request);

    /// <summary>
    /// Get study group details and members
    /// </summary>
    /// <param name="groupId">Study group ID</param>
    /// <returns>Study group information with member details</returns>
    Task<StudyGroupDto> GetStudyGroupAsync(int groupId);
}
