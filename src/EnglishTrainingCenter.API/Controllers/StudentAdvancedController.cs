using EnglishTrainingCenter.Application.DTOs.StudentAdvanced;
using EnglishTrainingCenter.Application.Services.StudentAdvanced;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnglishTrainingCenter.Api.Controllers;

/// <summary>
/// Advanced Student Management API Controller
/// Provides endpoints for attendance tracking, performance prediction, learning paths, and analytics
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StudentAdvancedController : ControllerBase
{
    private readonly IStudentAdvancedService _service;

    public StudentAdvancedController(IStudentAdvancedService service)
    {
        _service = service;
    }

    // ============================================================================
    // ATTENDANCE TRACKING ENDPOINTS
    // ============================================================================

    /// <summary>
    /// Mark attendance for a student in a course
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <param name="attendanceDate">Attendance date</param>
    /// <param name="status">Attendance status (Present, Absent, Late, Excused)</param>
    /// <returns>Attendance record ID</returns>
    [HttpPost("mark-attendance")]
    public async Task<ActionResult<int>> MarkAttendance(int studentId, int courseId, DateTime attendanceDate, string status)
    {
        try
        {
            var result = await _service.MarkAttendanceAsync(studentId, courseId, attendanceDate, status);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get attendance records for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Optional course ID filter</param>
    /// <returns>List of attendance records</returns>
    [HttpGet("attendance/{studentId}")]
    public async Task<ActionResult<IEnumerable<AttendanceRecordDto>>> GetAttendanceRecords(int studentId, int? courseId = null)
    {
        try
        {
            var result = await _service.GetAttendanceRecordsAsync(studentId, courseId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get attendance statistics for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Attendance statistics</returns>
    [HttpGet("attendance-statistics/{studentId}")]
    public async Task<ActionResult<AttendanceStatisticsDto>> GetAttendanceStatistics(int studentId)
    {
        try
        {
            var result = await _service.GetAttendanceStatisticsAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get course attendance summary
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Course attendance summary</returns>
    [HttpGet("course-attendance-summary/{courseId}")]
    public async Task<ActionResult<CourseAttendanceSummaryDto>> GetCourseAttendanceSummary(int courseId)
    {
        try
        {
            var result = await _service.GetCourseAttendanceSummaryAsync(courseId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Bulk mark attendance for multiple students
    /// </summary>
    /// <param name="bulkAttendance">List of attendance records</param>
    /// <returns>Number of records marked</returns>
    [HttpPost("bulk-mark-attendance")]
    public async Task<ActionResult<int>> BulkMarkAttendance([FromBody] List<BulkAttendanceDto> bulkAttendance)
    {
        try
        {
            var result = await _service.BulkMarkAttendanceAsync(bulkAttendance);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get students with low attendance
    /// </summary>
    /// <param name="minimumAttendanceRate">Minimum acceptable attendance rate (0-1)</param>
    /// <returns>List of students with low attendance</returns>
    [HttpGet("low-attendance-warnings")]
    public async Task<ActionResult<IEnumerable<LowAttendanceWarningDto>>> GetLowAttendanceWarnings(decimal minimumAttendanceRate = 0.75m)
    {
        try
        {
            var result = await _service.GetLowAttendanceWarningsAsync(minimumAttendanceRate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // ============================================================================
    // PERFORMANCE PREDICTION ENDPOINTS
    // ============================================================================

    /// <summary>
    /// Predict student performance
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Performance prediction with confidence level</returns>
    [HttpGet("performance-prediction/{studentId}")]
    public async Task<ActionResult<PerformancePredictionDto>> PredictPerformance(int studentId)
    {
        try
        {
            var result = await _service.PredictStudentPerformanceAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get performance predictions for all students in a course
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>List of performance predictions</returns>
    [HttpGet("course-performance-predictions/{courseId}")]
    public async Task<ActionResult<IEnumerable<StudentPerformancePredictionDto>>> GetCoursePerformancePredictions(int courseId)
    {
        try
        {
            var result = await _service.GetCoursePerformancePredictionsAsync(courseId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get at-risk students
    /// </summary>
    /// <param name="riskThreshold">Risk threshold (0-1)</param>
    /// <returns>List of at-risk students</returns>
    [HttpGet("at-risk-students")]
    public async Task<ActionResult<IEnumerable<AtRiskStudentDto>>> GetAtRiskStudents(decimal riskThreshold = 0.5m)
    {
        try
        {
            var result = await _service.GetAtRiskStudentsAsync(riskThreshold);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get comprehensive performance analysis for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Performance analysis</returns>
    [HttpGet("performance-analysis/{studentId}")]
    public async Task<ActionResult<StudentPerformanceAnalysisDto>> GetPerformanceAnalysis(int studentId)
    {
        try
        {
            var result = await _service.GetStudentPerformanceAnalysisAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get intervention recommendations for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Intervention recommendations</returns>
    [HttpGet("intervention-recommendations/{studentId}")]
    public async Task<ActionResult<InterventionRecommendationsDto>> GetInterventionRecommendations(int studentId)
    {
        try
        {
            var result = await _service.GetInterventionRecommendationsAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get performance benchmark comparing student with peers
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Performance benchmark</returns>
    [HttpGet("performance-benchmark/{studentId}/{courseId}")]
    public async Task<ActionResult<PerformanceBenchmarkDto>> GetPerformanceBenchmark(int studentId, int courseId)
    {
        try
        {
            var result = await _service.GetPerformanceBenchmarkAsync(studentId, courseId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get student progress tracking over time
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Student progress tracking</returns>
    [HttpGet("progress-tracking/{studentId}")]
    public async Task<ActionResult<StudentProgressTrackingDto>> GetProgressTracking(int studentId)
    {
        try
        {
            var result = await _service.GetStudentProgressTrackingAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // ============================================================================
    // LEARNING PATHS ENDPOINTS
    // ============================================================================

    /// <summary>
    /// Create a learning path for a student
    /// </summary>
    /// <param name="request">Learning path creation request</param>
    /// <returns>Learning path ID</returns>
    [HttpPost("learning-paths")]
    public async Task<ActionResult<int>> CreateLearningPath([FromBody] CreateLearningPathRequestDto request)
    {
        try
        {
            var result = await _service.CreateLearningPathAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get student's learning path
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Learning path details</returns>
    [HttpGet("learning-paths/{studentId}")]
    public async Task<ActionResult<LearningPathDto>> GetLearningPath(int studentId)
    {
        try
        {
            var result = await _service.GetLearningPathAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Update student's learning path
    /// </summary>
    /// <param name="learningPathId">Learning path ID</param>
    /// <param name="request">Update request</param>
    /// <returns>Updated learning path</returns>
    [HttpPut("learning-paths/{learningPathId}")]
    public async Task<ActionResult<LearningPathDto>> UpdateLearningPath(int learningPathId, [FromBody] UpdateLearningPathRequestDto request)
    {
        try
        {
            var result = await _service.UpdateLearningPathAsync(learningPathId, request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get course recommendations for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>List of recommended courses</returns>
    [HttpGet("course-recommendations/{studentId}")]
    public async Task<ActionResult<IEnumerable<CourseRecommendationDto>>> GetCourseRecommendations(int studentId)
    {
        try
        {
            var result = await _service.GetCourseRecommendationsAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get learning path progress for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Learning path progress</returns>
    [HttpGet("learning-path-progress/{studentId}")]
    public async Task<ActionResult<LearningPathProgressDto>> GetLearningPathProgress(int studentId)
    {
        try
        {
            var result = await _service.GetLearningPathProgressAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // ============================================================================
    // PREREQUISITE MANAGEMENT ENDPOINTS
    // ============================================================================

    /// <summary>
    /// Check if student meets prerequisites for a course
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Prerequisite check result</returns>
    [HttpGet("check-prerequisites/{studentId}/{courseId}")]
    public async Task<ActionResult<PrerequisiteCheckDto>> CheckPrerequisites(int studentId, int courseId)
    {
        try
        {
            var result = await _service.CheckPrerequisitesAsync(studentId, courseId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get prerequisite recommendations for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>List of prerequisite recommendations</returns>
    [HttpGet("prerequisite-recommendations/{studentId}")]
    public async Task<ActionResult<IEnumerable<PrerequisiteCourseDto>>> GetPrerequisiteRecommendations(int studentId)
    {
        try
        {
            var result = await _service.GetPrerequisiteRecommendationsAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Waive prerequisite requirement for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <param name="reason">Reason for waiver</param>
    /// <returns>True if waiver successful</returns>
    [HttpPost("waive-prerequisite")]
    public async Task<ActionResult<bool>> WaivePrerequisite(int studentId, int courseId, string reason)
    {
        try
        {
            var result = await _service.WaivePrerequisiteAsync(studentId, courseId, reason);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // ============================================================================
    // STUDY PROGRESS & ENGAGEMENT ENDPOINTS
    // ============================================================================

    /// <summary>
    /// Get study progress for a student in a course
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Study progress</returns>
    [HttpGet("study-progress/{studentId}/{courseId}")]
    public async Task<ActionResult<StudyProgressDto>> GetStudyProgress(int studentId, int courseId)
    {
        try
        {
            var result = await _service.GetStudyProgressAsync(studentId, courseId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get student engagement metrics
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Student engagement data</returns>
    [HttpGet("engagement/{studentId}")]
    public async Task<ActionResult<StudentEngagementDto>> GetEngagement(int studentId)
    {
        try
        {
            var result = await _service.GetStudentEngagementAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get study time analytics for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Study time analytics</returns>
    [HttpGet("study-time-analytics/{studentId}")]
    public async Task<ActionResult<StudyTimeAnalyticsDto>> GetStudyTimeAnalytics(int studentId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var result = await _service.GetStudyTimeAnalyticsAsync(studentId, startDate, endDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get resource utilization for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Resource utilization data</returns>
    [HttpGet("resource-utilization/{studentId}")]
    public async Task<ActionResult<ResourceUtilizationDto>> GetResourceUtilization(int studentId)
    {
        try
        {
            var result = await _service.GetResourceUtilizationAsync(studentId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // ============================================================================
    // STUDY GROUP & COLLABORATION ENDPOINTS
    // ============================================================================

    /// <summary>
    /// Create or join a study group
    /// </summary>
    /// <param name="request">Study group management request</param>
    /// <returns>Study group ID</returns>
    [HttpPost("study-groups")]
    public async Task<ActionResult<int>> ManageStudyGroup([FromBody] ManageStudyGroupRequestDto request)
    {
        try
        {
            var result = await _service.ManageStudyGroupAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Get study group details
    /// </summary>
    /// <param name="groupId">Study group ID</param>
    /// <returns>Study group details</returns>
    [HttpGet("study-groups/{groupId}")]
    public async Task<ActionResult<StudyGroupDto>> GetStudyGroup(int groupId)
    {
        try
        {
            var result = await _service.GetStudyGroupAsync(groupId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
