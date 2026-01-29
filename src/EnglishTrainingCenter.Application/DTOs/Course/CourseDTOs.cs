namespace EnglishTrainingCenter.Application.DTOs.Course;

/// <summary>
/// Request DTO for creating a new course.
/// </summary>
public class CreateCourseRequest
{
    /// <summary>
    /// Unique course code (e.g., ENG101, ENG201).
    /// </summary>
    public string CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// Course name (e.g., "English Fundamentals").
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Detailed course description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Course level (e.g., "Beginner", "Intermediate", "Advanced").
    /// </summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Course duration in hours.
    /// </summary>
    public int DurationHours { get; set; }

    /// <summary>
    /// Maximum number of students allowed in course.
    /// </summary>
    public int MaxCapacity { get; set; }

    /// <summary>
    /// Course tuition fee.
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// Course start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Course end date.
    /// </summary>
    public DateTime EndDate { get; set; }
}

/// <summary>
/// Request DTO for updating an existing course.
/// </summary>
public class UpdateCourseRequest
{
    /// <summary>
    /// Unique course code (e.g., ENG101, ENG201).
    /// </summary>
    public string CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// Course name (e.g., "English Fundamentals").
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Detailed course description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Course level (e.g., "Beginner", "Intermediate", "Advanced").
    /// </summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Course duration in hours.
    /// </summary>
    public int DurationHours { get; set; }

    /// <summary>
    /// Maximum number of students allowed in course.
    /// </summary>
    public int MaxCapacity { get; set; }

    /// <summary>
    /// Course tuition fee.
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// Course start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Course end date.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Is course active.
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// Response DTO for course list (paginated results).
/// </summary>
public class CourseListDto
{
    /// <summary>
    /// Unique course identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique course code.
    /// </summary>
    public string CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// Course name.
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Course level.
    /// </summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Course duration in hours.
    /// </summary>
    public int DurationHours { get; set; }

    /// <summary>
    /// Course tuition fee.
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// Number of students currently enrolled.
    /// </summary>
    public int EnrolledStudents { get; set; }

    /// <summary>
    /// Maximum capacity for the course.
    /// </summary>
    public int MaxCapacity { get; set; }

    /// <summary>
    /// Is course active.
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// Response DTO for detailed course information.
/// </summary>
public class CourseDetailDto
{
    /// <summary>
    /// Unique course identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique course code.
    /// </summary>
    public string CourseCode { get; set; } = string.Empty;

    /// <summary>
    /// Course name.
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Detailed course description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Course level.
    /// </summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Course duration in hours.
    /// </summary>
    public int DurationHours { get; set; }

    /// <summary>
    /// Maximum capacity for the course.
    /// </summary>
    public int MaxCapacity { get; set; }

    /// <summary>
    /// Course tuition fee.
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// Course start date.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Course end date.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Number of students currently enrolled.
    /// </summary>
    public int EnrolledStudents { get; set; }

    /// <summary>
    /// Number of available seats.
    /// </summary>
    public int AvailableCapacity { get; set; }

    /// <summary>
    /// Number of schedules associated with this course.
    /// </summary>
    public int ScheduleCount { get; set; }

    /// <summary>
    /// When course was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// When course was last modified.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    /// <summary>
    /// Is course active.
    /// </summary>
    public bool IsActive { get; set; }
}
