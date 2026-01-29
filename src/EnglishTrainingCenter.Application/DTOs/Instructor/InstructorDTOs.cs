namespace EnglishTrainingCenter.Application.DTOs.Instructor;

/// <summary>
/// Request DTO for creating a new instructor
/// </summary>
public class CreateInstructorRequest
{
    /// <summary>First name (Required)</summary>
    public string FirstName { get; set; }

    /// <summary>Last name (Required)</summary>
    public string LastName { get; set; }

    /// <summary>Email address (Required, unique)</summary>
    public string Email { get; set; }

    /// <summary>Phone number (Optional)</summary>
    public string PhoneNumber { get; set; }

    /// <summary>Area of specialization (Required)</summary>
    public string Specialization { get; set; }

    /// <summary>Academic qualification level (Required, e.g., Bachelor, Master, PhD)</summary>
    public string Qualification { get; set; }

    /// <summary>Years of experience (Required, minimum 0)</summary>
    public int YearsOfExperience { get; set; }

    /// <summary>Base salary amount (Required, must be > 0)</summary>
    public decimal BaseSalary { get; set; }

    /// <summary>Salary frequency (Required: Monthly, Quarterly, or Yearly)</summary>
    public string SalaryFrequency { get; set; }
}

/// <summary>
/// Request DTO for updating an instructor
/// </summary>
public class UpdateInstructorRequest
{
    /// <summary>First name (Required)</summary>
    public string FirstName { get; set; }

    /// <summary>Last name (Required)</summary>
    public string LastName { get; set; }

    /// <summary>Email address (Required, unique)</summary>
    public string Email { get; set; }

    /// <summary>Phone number (Optional)</summary>
    public string PhoneNumber { get; set; }

    /// <summary>Area of specialization (Required)</summary>
    public string Specialization { get; set; }

    /// <summary>Academic qualification level (Required)</summary>
    public string Qualification { get; set; }

    /// <summary>Years of experience (Required)</summary>
    public int YearsOfExperience { get; set; }

    /// <summary>Base salary amount (Required, must be > 0)</summary>
    public decimal BaseSalary { get; set; }

    /// <summary>Salary frequency (Required)</summary>
    public string SalaryFrequency { get; set; }
}

/// <summary>
/// List DTO for compact instructor information
/// </summary>
public class InstructorListDto
{
    /// <summary>Instructor ID</summary>
    public int Id { get; set; }

    /// <summary>Full name (FirstName + LastName)</summary>
    public string FullName { get; set; }

    /// <summary>Email address</summary>
    public string Email { get; set; }

    /// <summary>Phone number</summary>
    public string PhoneNumber { get; set; }

    /// <summary>Specialization area</summary>
    public string Specialization { get; set; }

    /// <summary>Academic qualification</summary>
    public string Qualification { get; set; }

    /// <summary>Active status</summary>
    public bool IsActive { get; set; }

    /// <summary>Years of teaching experience</summary>
    public int YearsOfExperience { get; set; }
}

/// <summary>
/// Detail DTO for complete instructor information
/// </summary>
public class InstructorDetailDto
{
    /// <summary>Instructor ID</summary>
    public int Id { get; set; }

    /// <summary>First name</summary>
    public string FirstName { get; set; }

    /// <summary>Last name</summary>
    public string LastName { get; set; }

    /// <summary>Full name (FirstName + LastName)</summary>
    public string FullName { get; set; }

    /// <summary>Email address</summary>
    public string Email { get; set; }

    /// <summary>Phone number</summary>
    public string PhoneNumber { get; set; }

    /// <summary>Area of specialization</summary>
    public string Specialization { get; set; }

    /// <summary>Academic qualification level</summary>
    public string Qualification { get; set; }

    /// <summary>Years of teaching experience</summary>
    public int YearsOfExperience { get; set; }

    /// <summary>Base salary amount</summary>
    public decimal BaseSalary { get; set; }

    /// <summary>Salary frequency (Monthly/Quarterly/Yearly)</summary>
    public string SalaryFrequency { get; set; }

    /// <summary>Number of assigned courses</summary>
    public int AssignedCourseCount { get; set; }

    /// <summary>Average performance rating (1-5)</summary>
    public decimal AveragePerformanceRating { get; set; }

    /// <summary>Date joined the center</summary>
    public DateTime JoinDate { get; set; }

    /// <summary>Active status</summary>
    public bool IsActive { get; set; }

    /// <summary>Record creation date</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Record last updated date</summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// DTO for instructor course assignment
/// </summary>
public class InstructorAssignmentDto
{
    /// <summary>Instructor ID</summary>
    public int InstructorId { get; set; }

    /// <summary>Instructor full name</summary>
    public string InstructorName { get; set; }

    /// <summary>Course ID</summary>
    public int CourseId { get; set; }

    /// <summary>Course name</summary>
    public string CourseName { get; set; }

    /// <summary>Course code</summary>
    public string CourseCode { get; set; }

    /// <summary>Assignment date</summary>
    public DateTime AssignmentDate { get; set; }
}

/// <summary>
/// DTO for instructor performance rating
/// </summary>
public class InstructorPerformanceDto
{
    /// <summary>Instructor ID</summary>
    public int InstructorId { get; set; }

    /// <summary>Instructor full name</summary>
    public string InstructorName { get; set; }

    /// <summary>Performance rating (1-5)</summary>
    public int Rating { get; set; }

    /// <summary>Performance comments</summary>
    public string Comments { get; set; }

    /// <summary>Rating date</summary>
    public DateTime RatingDate { get; set; }
}

/// <summary>
/// DTO for instructor statistics and analytics
/// </summary>
public class InstructorStatisticsDto
{
    /// <summary>Instructor ID</summary>
    public int InstructorId { get; set; }

    /// <summary>Instructor full name</summary>
    public string InstructorName { get; set; }

    /// <summary>Total number of assigned courses</summary>
    public int TotalCoursesAssigned { get; set; }

    /// <summary>Years of teaching experience</summary>
    public int YearsOfExperience { get; set; }

    /// <summary>Base salary</summary>
    public decimal BaseSalary { get; set; }

    /// <summary>Calculated salary based on performance bonuses</summary>
    public decimal CalculatedSalary { get; set; }

    /// <summary>Average performance rating from all evaluations</summary>
    public decimal AveragePerformanceRating { get; set; }

    /// <summary>Date joined the center</summary>
    public DateTime JoinDate { get; set; }

    /// <summary>Active status</summary>
    public bool IsActive { get; set; }
}
