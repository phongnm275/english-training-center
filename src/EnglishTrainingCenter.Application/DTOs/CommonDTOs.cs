namespace EnglishTrainingCenter.Application.DTOs;

/// <summary>
/// DTO for Student
/// </summary>
public class StudentDto
{
    public int StudentId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for creating a Student
/// </summary>
public class CreateStudentRequest
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
}

/// <summary>
/// DTO for updating a Student
/// </summary>
public class UpdateStudentRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
}

/// <summary>
/// DTO for User
/// </summary>
public class UserDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for Course
/// </summary>
public class CourseDto
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Level { get; set; }
    public int? Duration { get; set; }
    public decimal? Fee { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for Class
/// </summary>
public class ClassDto
{
    public int ClassId { get; set; }
    public int CourseId { get; set; }
    public int InstructorId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int MaxStudents { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }
    public bool IsActive { get; set; }
}
