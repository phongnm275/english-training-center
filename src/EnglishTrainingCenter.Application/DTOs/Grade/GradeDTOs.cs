namespace EnglishTrainingCenter.Application.DTOs.Grade;

/// <summary>
/// Request DTO for creating a new grade.
/// </summary>
public class CreateGradeRequest
{
    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Course ID.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Letter grade (A, B, C, D, F).
    /// </summary>
    public string Grade { get; set; } = string.Empty;

    /// <summary>
    /// Optional numeric score (0-100).
    /// </summary>
    public decimal? NumericScore { get; set; }

    /// <summary>
    /// Optional comments or feedback.
    /// </summary>
    public string Comments { get; set; } = string.Empty;
}

/// <summary>
/// Request DTO for updating an existing grade.
/// </summary>
public class UpdateGradeRequest
{
    /// <summary>
    /// Letter grade (A, B, C, D, F).
    /// </summary>
    public string Grade { get; set; } = string.Empty;

    /// <summary>
    /// Optional numeric score (0-100).
    /// </summary>
    public decimal? NumericScore { get; set; }

    /// <summary>
    /// Optional comments or feedback.
    /// </summary>
    public string Comments { get; set; } = string.Empty;
}

/// <summary>
/// Response DTO for grade list (paginated results).
/// </summary>
public class GradeListDto
{
    /// <summary>
    /// Unique grade identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Course ID.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Letter grade.
    /// </summary>
    public string Grade { get; set; } = string.Empty;

    /// <summary>
    /// Numeric score.
    /// </summary>
    public decimal? NumericScore { get; set; }

    /// <summary>
    /// Grade date.
    /// </summary>
    public DateTime GradeDate { get; set; }
}

/// <summary>
/// Response DTO for detailed grade information.
/// </summary>
public class GradeDetailDto
{
    /// <summary>
    /// Unique grade identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Student name (populated from Student entity).
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Course ID.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Course name (populated from Course entity).
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Letter grade.
    /// </summary>
    public string Grade { get; set; } = string.Empty;

    /// <summary>
    /// Numeric score.
    /// </summary>
    public decimal? NumericScore { get; set; }

    /// <summary>
    /// Comments or feedback.
    /// </summary>
    public string Comments { get; set; } = string.Empty;

    /// <summary>
    /// Grade date.
    /// </summary>
    public DateTime GradeDate { get; set; }

    /// <summary>
    /// When grade was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// When grade was last modified.
    /// </summary>
    public DateTime ModifiedDate { get; set; }
}

/// <summary>
/// Grade distribution statistics for a course.
/// </summary>
public class GradeDistributionDto
{
    /// <summary>
    /// Course ID.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Total number of grades.
    /// </summary>
    public int TotalGrades { get; set; }

    /// <summary>
    /// Number of A grades.
    /// </summary>
    public int ACount { get; set; }

    /// <summary>
    /// Number of B grades.
    /// </summary>
    public int BCount { get; set; }

    /// <summary>
    /// Number of C grades.
    /// </summary>
    public int CCount { get; set; }

    /// <summary>
    /// Number of D grades.
    /// </summary>
    public int DCount { get; set; }

    /// <summary>
    /// Number of F grades.
    /// </summary>
    public int FCount { get; set; }

    /// <summary>
    /// Average grade percentage.
    /// </summary>
    public decimal AverageGrade { get; set; }
}

/// <summary>
/// Academic report card for a student.
/// </summary>
public class AcademicReportCardDto
{
    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Student full name.
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Student email.
    /// </summary>
    public string StudentEmail { get; set; } = string.Empty;

    /// <summary>
    /// Current GPA (0.0-4.0 scale).
    /// </summary>
    public decimal GPA { get; set; }

    /// <summary>
    /// Academic status based on GPA.
    /// </summary>
    public string AcademicStatus { get; set; } = string.Empty;

    /// <summary>
    /// Total number of courses completed.
    /// </summary>
    public int TotalCoursesCompleted { get; set; }

    /// <summary>
    /// List of course grades.
    /// </summary>
    public List<CourseGradeDto> CourseGrades { get; set; } = new();

    /// <summary>
    /// Report card generation date.
    /// </summary>
    public DateTime GeneratedDate { get; set; }
}

/// <summary>
/// Course grade information for report card.
/// </summary>
public class CourseGradeDto
{
    /// <summary>
    /// Course ID.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Course name.
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Course code.
    /// </summary>
    public string CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// Letter grade.
    /// </summary>
    public string Grade { get; set; } = string.Empty;

    /// <summary>
    /// Grade date.
    /// </summary>
    public DateTime GradeDate { get; set; }
}

/// <summary>
/// Low grade student information.
/// </summary>
public class LowGradeStudentDto
{
    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Student full name.
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// List of low grades.
    /// </summary>
    public List<StudentLowGradeDto> LowGrades { get; set; } = new();
}

/// <summary>
/// Student's low grade detail.
/// </summary>
public class StudentLowGradeDto
{
    /// <summary>
    /// Course ID.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Course name.
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Letter grade.
    /// </summary>
    public string Grade { get; set; } = string.Empty;

    /// <summary>
    /// Grade as percentage.
    /// </summary>
    public decimal GradePercentage { get; set; }

    /// <summary>
    /// Grade date.
    /// </summary>
    public DateTime GradeDate { get; set; }
}

/// <summary>
/// Top performing student information.
/// </summary>
public class TopStudentDto
{
    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Student full name.
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Student GPA.
    /// </summary>
    public decimal GPA { get; set; }
}
