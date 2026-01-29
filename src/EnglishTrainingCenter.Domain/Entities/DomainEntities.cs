namespace EnglishTrainingCenter.Domain.Entities;

/// <summary>
/// Role entity
/// </summary>
public class Role : BaseEntity
{
    public string RoleName { get; set; } = string.Empty;
    public string? Description { get; set; }
}

/// <summary>
/// User entity
/// </summary>
public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLogin { get; set; }
    public DateTime? LastLogout { get; set; }

    // Navigation
    public virtual Role? Role { get; set; }
}

/// <summary>
/// Student entity
/// </summary>
public class Student : BaseEntity
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation
    public virtual User? User { get; set; }
    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}

/// <summary>
/// Course entity
/// </summary>
public class Course : BaseEntity
{
    public string CourseName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Level { get; set; } // Beginner, Intermediate, Advanced
    public int? Duration { get; set; } // hours
    public decimal? Fee { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}

/// <summary>
/// Class entity
/// </summary>
public class Class : BaseEntity
{
    public int CourseId { get; set; }
    public int InstructorId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int MaxStudents { get; set; } = 30;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; } // Upcoming, Ongoing, Completed, Cancelled
    public bool IsActive { get; set; } = true;

    // Navigation
    public virtual Course? Course { get; set; }
    public virtual Instructor? Instructor { get; set; }
    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}

/// <summary>
/// Student Course enrollment entity
/// </summary>
public class StudentCourse : BaseEntity
{
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public DateTime EnrollDate { get; set; } = DateTime.UtcNow;
    public string? Status { get; set; } // Enrolled, Completed, Dropped
    public decimal? GPA { get; set; }

    // Navigation
    public virtual Student? Student { get; set; }
    public virtual Class? Class { get; set; }
}

/// <summary>
/// Room entity
/// </summary>
public class Room : BaseEntity
{
    public string RoomNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string? Equipment { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}

/// <summary>
/// Schedule entity
/// </summary>
public class Schedule : BaseEntity
{
    public int ClassId { get; set; }
    public int RoomId { get; set; }
    public int? DayOfWeek { get; set; } // 0-6
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public virtual Class? Class { get; set; }
    public virtual Room? Room { get; set; }
}

/// <summary>
/// Instructor entity
/// </summary>
public class Instructor : BaseEntity
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Specialization { get; set; }
    public int? YearsExperience { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public virtual User? User { get; set; }
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
    public virtual ICollection<InstructorQualification> Qualifications { get; set; } = new List<InstructorQualification>();
}

/// <summary>
/// Instructor Qualification entity
/// </summary>
public class InstructorQualification : BaseEntity
{
    public int InstructorId { get; set; }
    public string? Certification { get; set; }
    public DateTime? CertificationDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Issuer { get; set; }

    // Navigation
    public virtual Instructor? Instructor { get; set; }
}

/// <summary>
/// Invoice entity
/// </summary>
public class Invoice : BaseEntity
{
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public decimal Amount { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? IssueDate { get; set; }
    public string? Status { get; set; } // Issued, Paid, Overdue, Cancelled
    public string? Notes { get; set; }

    // Navigation
    public virtual Student? Student { get; set; }
    public virtual Class? Class { get; set; }
}

/// <summary>
/// Payment entity
/// </summary>
public class Payment : BaseEntity
{
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? PaymentMethod { get; set; } // Credit Card, Bank Transfer, Cash
    public string? Status { get; set; } // Pending, Completed, Failed, Refunded
    public string? TransactionId { get; set; }
    public string? Description { get; set; }

    // Navigation
    public virtual Student? Student { get; set; }
}

/// <summary>
/// Assignment entity
/// </summary>
public class Assignment : BaseEntity
{
    public int ClassId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal? MaxScore { get; set; } = 100;

    // Navigation
    public virtual Class? Class { get; set; }
    public virtual ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
}

/// <summary>
/// Assignment Submission entity
/// </summary>
public class AssignmentSubmission : BaseEntity
{
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public string? Content { get; set; }
    public string? FileUrl { get; set; }
    public decimal? Score { get; set; }
    public string? Feedback { get; set; }
    public bool? IsLate { get; set; }

    // Navigation
    public virtual Assignment? Assignment { get; set; }
    public virtual Student? Student { get; set; }
}

/// <summary>
/// Exam entity
/// </summary>
public class Exam : BaseEntity
{
    public int ClassId { get; set; }
    public string ExamName { get; set; } = string.Empty;
    public DateTime? ExamDate { get; set; }
    public int? Duration { get; set; } // minutes
    public decimal? MaxScore { get; set; } = 100;
    public decimal? PassingScore { get; set; } = 50;

    // Navigation
    public virtual Class? Class { get; set; }
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}

/// <summary>
/// Grade entity
/// </summary>
public class Grade : BaseEntity
{
    public int StudentId { get; set; }
    public int ExamId { get; set; }
    public decimal? Score { get; set; }
    public string? GradeValue { get; set; } // A, B, C, D, F
    public string? Feedback { get; set; }
    public DateTime? GradedDate { get; set; }

    // Navigation
    public virtual Student? Student { get; set; }
    public virtual Exam? Exam { get; set; }
}
