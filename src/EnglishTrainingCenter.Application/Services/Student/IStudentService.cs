using EnglishTrainingCenter.Application.DTOs.Student;

namespace EnglishTrainingCenter.Application.Services.Student;

/// <summary>
/// Student service interface for CRUD operations and student management
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// Get all students with pagination
    /// </summary>
    Task<(IEnumerable<StudentListDto> Students, int TotalCount)> GetAllStudentsAsync(int pageNumber = 1, int pageSize = 10);

    /// <summary>
    /// Get student by ID
    /// </summary>
    Task<StudentDetailDto?> GetStudentByIdAsync(int id);

    /// <summary>
    /// Create new student
    /// </summary>
    Task<StudentDetailDto> CreateStudentAsync(CreateStudentRequest request);

    /// <summary>
    /// Update existing student
    /// </summary>
    Task<StudentDetailDto> UpdateStudentAsync(int id, UpdateStudentRequest request);

    /// <summary>
    /// Delete student
    /// </summary>
    Task<bool> DeleteStudentAsync(int id);

    /// <summary>
    /// Search students by name, email, or phone
    /// </summary>
    Task<IEnumerable<StudentListDto>> SearchStudentsAsync(string searchTerm);

    /// <summary>
    /// Get students by enrollment status
    /// </summary>
    Task<IEnumerable<StudentListDto>> GetStudentsByStatusAsync(bool isActive);

    /// <summary>
    /// Get total number of students
    /// </summary>
    Task<int> GetTotalStudentCountAsync();

    /// <summary>
    /// Check if email exists
    /// </summary>
    Task<bool> EmailExistsAsync(string email);

    /// <summary>
    /// Get students enrolled in specific course
    /// </summary>
    Task<IEnumerable<StudentListDto>> GetStudentsInCourseAsync(int courseId);
}
