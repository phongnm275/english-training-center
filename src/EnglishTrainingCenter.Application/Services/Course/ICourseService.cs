using EnglishTrainingCenter.Application.DTOs.Course;
using EnglishTrainingCenter.Common;

namespace EnglishTrainingCenter.Application.Services.Course;

/// <summary>
/// Service interface for course management operations.
/// Provides methods for CRUD operations, search, filtering, and capacity management.
/// </summary>
public interface ICourseService
{
    /// <summary>
    /// Retrieves all courses with pagination.
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of records per page</param>
    /// <returns>Paginated list of courses</returns>
    Task<PagedResult<CourseListDto>> GetAllCoursesAsync(int pageNumber = 1, int pageSize = 10);

    /// <summary>
    /// Retrieves a specific course by ID.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Course details with related data</returns>
    Task<CourseDetailDto> GetCourseByIdAsync(int courseId);

    /// <summary>
    /// Creates a new course.
    /// </summary>
    /// <param name="request">Course creation request</param>
    /// <returns>Created course details</returns>
    Task<CourseDetailDto> CreateCourseAsync(CreateCourseRequest request);

    /// <summary>
    /// Updates an existing course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <param name="request">Course update request</param>
    /// <returns>Updated course details</returns>
    Task<CourseDetailDto> UpdateCourseAsync(int courseId, UpdateCourseRequest request);

    /// <summary>
    /// Deletes a course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>True if successful, False otherwise</returns>
    Task<bool> DeleteCourseAsync(int courseId);

    /// <summary>
    /// Searches courses by name or code.
    /// </summary>
    /// <param name="searchTerm">Search term (name or code)</param>
    /// <returns>List of matching courses</returns>
    Task<IEnumerable<CourseListDto>> SearchCoursesAsync(string searchTerm);

    /// <summary>
    /// Retrieves courses by level.
    /// </summary>
    /// <param name="level">Course level (e.g., "Beginner", "Intermediate", "Advanced")</param>
    /// <returns>List of courses at specified level</returns>
    Task<IEnumerable<CourseListDto>> GetCoursesByLevelAsync(string level);

    /// <summary>
    /// Retrieves courses by status (active/inactive).
    /// </summary>
    /// <param name="isActive">Is course active</param>
    /// <returns>List of courses matching status</returns>
    Task<IEnumerable<CourseListDto>> GetCoursesByStatusAsync(bool isActive);

    /// <summary>
    /// Gets courses with available capacity.
    /// </summary>
    /// <returns>List of courses with available slots</returns>
    Task<IEnumerable<CourseListDto>> GetCoursesWithCapacityAsync();

    /// <summary>
    /// Gets total number of courses.
    /// </summary>
    /// <returns>Total course count</returns>
    Task<int> GetTotalCourseCountAsync();

    /// <summary>
    /// Gets the number of students enrolled in a course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Number of enrolled students</returns>
    Task<int> GetEnrolledStudentCountAsync(int courseId);

    /// <summary>
    /// Checks if course code already exists.
    /// </summary>
    /// <param name="courseCode">Course code</param>
    /// <param name="excludeCourseId">Course ID to exclude from check (for updates)</param>
    /// <returns>True if code exists, False otherwise</returns>
    Task<bool> CourseCodeExistsAsync(string courseCode, int? excludeCourseId = null);
}
