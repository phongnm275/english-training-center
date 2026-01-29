using EnglishTrainingCenter.Application.DTOs.Instructor;
using EnglishTrainingCenter.Common;

namespace EnglishTrainingCenter.Application.Services.Instructor;

/// <summary>
/// Service interface for instructor management operations
/// </summary>
public interface IInstructorService
{
    /// <summary>
    /// Gets all instructors with pagination
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Records per page</param>
    /// <returns>Paginated list of instructors</returns>
    Task<PagedResult<InstructorListDto>> GetAllInstructorsAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Gets a single instructor by ID
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <returns>Instructor details</returns>
    Task<InstructorDetailDto> GetInstructorByIdAsync(int id);

    /// <summary>
    /// Creates a new instructor
    /// </summary>
    /// <param name="request">Instructor creation request</param>
    /// <returns>Created instructor</returns>
    Task<InstructorDetailDto> CreateInstructorAsync(CreateInstructorRequest request);

    /// <summary>
    /// Updates an existing instructor
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <param name="request">Instructor update request</param>
    /// <returns>Updated instructor</returns>
    Task<InstructorDetailDto> UpdateInstructorAsync(int id, UpdateInstructorRequest request);

    /// <summary>
    /// Deletes an instructor
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <returns>Success status</returns>
    Task<bool> DeleteInstructorAsync(int id);

    /// <summary>
    /// Searches instructors by name or email
    /// </summary>
    /// <param name="searchTerm">Search term (name or email)</param>
    /// <returns>Matching instructors</returns>
    Task<IEnumerable<InstructorListDto>> SearchInstructorsAsync(string searchTerm);

    /// <summary>
    /// Gets instructors by qualification level
    /// </summary>
    /// <param name="qualification">Qualification level</param>
    /// <returns>Instructors with specified qualification</returns>
    Task<IEnumerable<InstructorListDto>> GetInstructorsByQualificationAsync(string qualification);

    /// <summary>
    /// Assigns instructor to a course
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Assignment details</returns>
    Task<InstructorAssignmentDto> AssignCourseAsync(int instructorId, int courseId);

    /// <summary>
    /// Removes instructor from a course
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Success status</returns>
    Task<bool> RemoveCourseAssignmentAsync(int instructorId, int courseId);

    /// <summary>
    /// Gets all courses assigned to an instructor
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <returns>Assigned courses</returns>
    Task<IEnumerable<InstructorAssignmentDto>> GetInstructorCoursesAsync(int instructorId);

    /// <summary>
    /// Gets instructors for a specific course
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Course instructors</returns>
    Task<IEnumerable<InstructorListDto>> GetCourseInstructorsAsync(int courseId);

    /// <summary>
    /// Updates instructor salary information
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <param name="baseSalary">Base salary amount</param>
    /// <param name="salaryFrequency">Salary frequency (Monthly/Quarterly/Yearly)</param>
    /// <returns>Updated instructor</returns>
    Task<InstructorDetailDto> UpdateSalaryAsync(int instructorId, decimal baseSalary, string salaryFrequency);

    /// <summary>
    /// Calculates instructor salary based on courses and experience
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <returns>Calculated salary</returns>
    Task<decimal> CalculateSalaryAsync(int instructorId);

    /// <summary>
    /// Adds performance rating for instructor
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <param name="rating">Rating 1-5</param>
    /// <param name="comments">Performance comments</param>
    /// <returns>Performance record</returns>
    Task<InstructorPerformanceDto> AddPerformanceRatingAsync(int instructorId, int rating, string comments);

    /// <summary>
    /// Gets average performance rating for instructor
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <returns>Average rating</returns>
    Task<decimal> GetAveragePerformanceRatingAsync(int instructorId);

    /// <summary>
    /// Gets instructors by status (Active/Inactive)
    /// </summary>
    /// <param name="isActive">Active status filter</param>
    /// <returns>Filtered instructors</returns>
    Task<IEnumerable<InstructorListDto>> GetInstructorsByStatusAsync(bool isActive);

    /// <summary>
    /// Updates instructor status (Active/Inactive)
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <param name="isActive">New status</param>
    /// <returns>Updated instructor</returns>
    Task<InstructorDetailDto> UpdateInstructorStatusAsync(int instructorId, bool isActive);

    /// <summary>
    /// Gets instructor statistics and analytics
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <returns>Instructor statistics</returns>
    Task<InstructorStatisticsDto> GetInstructorStatisticsAsync(int instructorId);

    /// <summary>
    /// Gets top-rated instructors
    /// </summary>
    /// <param name="count">Number of top instructors</param>
    /// <returns>Top-rated instructors</returns>
    Task<IEnumerable<InstructorPerformanceDto>> GetTopRatedInstructorsAsync(int count);
}
