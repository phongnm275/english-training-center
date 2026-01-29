using System.ComponentModel.DataAnnotations;

namespace EnglishTrainingCenter.Application.DTOs.Student;

/// <summary>
/// Request DTO for creating a new student
/// </summary>
public class CreateStudentRequest
{
    /// <summary>
    /// User ID associated with student
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Student full name
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(256, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 256 characters")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Student email address
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Student phone number
    /// </summary>
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string? Phone { get; set; }

    /// <summary>
    /// Student date of birth
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Student address
    /// </summary>
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }

    /// <summary>
    /// Student avatar URL
    /// </summary>
    [StringLength(500, ErrorMessage = "Avatar URL cannot exceed 500 characters")]
    public string? Avatar { get; set; }
}

/// <summary>
/// Request DTO for updating an existing student
/// </summary>
public class UpdateStudentRequest
{
    /// <summary>
    /// Student full name
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [StringLength(256, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 256 characters")]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Student email address
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Student phone number
    /// </summary>
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string? Phone { get; set; }

    /// <summary>
    /// Student date of birth
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Student address
    /// </summary>
    [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
    public string? Address { get; set; }

    /// <summary>
    /// Student avatar URL
    /// </summary>
    [StringLength(500, ErrorMessage = "Avatar URL cannot exceed 500 characters")]
    public string? Avatar { get; set; }

    /// <summary>
    /// Whether student is active
    /// </summary>
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO for student list view (summary information)
/// </summary>
public class StudentListDto
{
    /// <summary>
    /// Student ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Student full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Student email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Student phone
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Enrollment date
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// Whether student is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Number of enrolled courses
    /// </summary>
    public int EnrolledCourses { get; set; }
}

/// <summary>
/// DTO for student detail view (complete information)
/// </summary>
public class StudentDetailDto
{
    /// <summary>
    /// Student ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Associated User ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Student full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Student email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Student phone
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Avatar URL
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// Enrollment date
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// Whether student is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Date record was created
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Date record was last modified
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    /// <summary>
    /// Number of enrolled courses
    /// </summary>
    public int EnrolledCourses { get; set; }

    /// <summary>
    /// Total payments made
    /// </summary>
    public decimal TotalPayments { get; set; }

    /// <summary>
    /// Average GPA
    /// </summary>
    public decimal? AverageGPA { get; set; }
}
