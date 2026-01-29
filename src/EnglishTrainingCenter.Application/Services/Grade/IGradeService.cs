using EnglishTrainingCenter.Application.DTOs.Grade;
using EnglishTrainingCenter.Common;

namespace EnglishTrainingCenter.Application.Services.Grade;

/// <summary>
/// Service interface for grade management and academic tracking.
/// Handles grade CRUD operations, GPA calculations, and academic reporting.
/// </summary>
public interface IGradeService
{
    /// <summary>
    /// Retrieves all grades with pagination.
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of records per page</param>
    /// <returns>Paginated list of grades</returns>
    Task<PagedResult<GradeListDto>> GetAllGradesAsync(int pageNumber = 1, int pageSize = 10);

    /// <summary>
    /// Retrieves a specific grade by ID.
    /// </summary>
    /// <param name="gradeId">Grade ID</param>
    /// <returns>Grade details</returns>
    Task<GradeDetailDto> GetGradeByIdAsync(int gradeId);

    /// <summary>
    /// Creates a new grade record.
    /// </summary>
    /// <param name="request">Grade creation request</param>
    /// <returns>Created grade details</returns>
    Task<GradeDetailDto> CreateGradeAsync(CreateGradeRequest request);

    /// <summary>
    /// Updates an existing grade.
    /// </summary>
    /// <param name="gradeId">Grade ID</param>
    /// <param name="request">Grade update request</param>
    /// <returns>Updated grade details</returns>
    Task<GradeDetailDto> UpdateGradeAsync(int gradeId, UpdateGradeRequest request);

    /// <summary>
    /// Deletes a grade record.
    /// </summary>
    /// <param name="gradeId">Grade ID</param>
    /// <returns>True if successful</returns>
    Task<bool> DeleteGradeAsync(int gradeId);

    /// <summary>
    /// Retrieves all grades for a specific student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>List of student's grades</returns>
    Task<IEnumerable<GradeListDto>> GetGradesByStudentAsync(int studentId);

    /// <summary>
    /// Retrieves all grades for a specific course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>List of course grades</returns>
    Task<IEnumerable<GradeListDto>> GetGradesByCourseAsync(int courseId);

    /// <summary>
    /// Calculates GPA for a student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Calculated GPA (0.0 - 4.0 scale)</returns>
    Task<decimal> CalculateStudentGPAAsync(int studentId);

    /// <summary>
    /// Gets grade distribution statistics for a course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Grade distribution statistics</returns>
    Task<GradeDistributionDto> GetCourseGradeDistributionAsync(int courseId);

    /// <summary>
    /// Generates an academic report card for a student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Report card with GPA, grades, and academic status</returns>
    Task<AcademicReportCardDto> GetStudentReportCardAsync(int studentId);

    /// <summary>
    /// Gets all grades for a student in a specific course.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Grades for the student-course combination</returns>
    Task<IEnumerable<GradeListDto>> GetStudentCourseGradesAsync(int studentId, int courseId);

    /// <summary>
    /// Gets students with grades below minimum threshold.
    /// </summary>
    /// <param name="minimumGrade">Minimum grade threshold</param>
    /// <returns>Students and their low grades</returns>
    Task<IEnumerable<LowGradeStudentDto>> GetLowGradeStudentsAsync(decimal minimumGrade = 60);

    /// <summary>
    /// Gets top performing students by GPA.
    /// </summary>
    /// <param name="count">Number of top students to return</param>
    /// <returns>Top performing students</returns>
    Task<IEnumerable<TopStudentDto>> GetTopStudentsAsync(int count = 10);

    /// <summary>
    /// Gets course average grade.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Average grade for the course</returns>
    Task<decimal> GetCourseAverageGradeAsync(int courseId);
}
