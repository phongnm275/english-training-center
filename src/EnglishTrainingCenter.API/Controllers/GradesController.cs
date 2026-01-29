using EnglishTrainingCenter.Application.DTOs.Grade;
using EnglishTrainingCenter.Application.Services.Grade;
using EnglishTrainingCenter.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// API controller for grade and academic performance management.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;
    private readonly ILogger<GradesController> _logger;

    public GradesController(IGradeService gradeService, ILogger<GradesController> logger)
    {
        _gradeService = gradeService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all grades with pagination.
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Records per page (default: 10, max: 50)</param>
    /// <returns>Paginated list of grades</returns>
    [HttpGet]
    [ProduceResponseType(typeof(ApiResponse<PagedResult<GradeListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllGrades(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 10;

            var result = await _gradeService.GetAllGradesAsync(pageNumber, pageSize);

            return Ok(new ApiResponse<PagedResult<GradeListDto>>
            {
                Success = true,
                Message = $"Retrieved {result.Data.Count} grades",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting grades: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving grades",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets a grade by ID.
    /// </summary>
    /// <param name="id">Grade ID</param>
    /// <returns>Grade details</returns>
    [HttpGet("{id}")]
    [ProduceResponseType(typeof(ApiResponse<GradeDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGradeById(int id)
    {
        try
        {
            var result = await _gradeService.GetGradeByIdAsync(id);

            return Ok(new ApiResponse<GradeDetailDto>
            {
                Success = true,
                Message = "Grade retrieved successfully",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Grade not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting grade {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving grade",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Creates a new grade.
    /// </summary>
    /// <param name="request">Grade creation request</param>
    /// <returns>Created grade</returns>
    [HttpPost]
    [ProduceResponseType(typeof(ApiResponse<GradeDetailDto>), StatusCodes.Status201Created)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateGrade([FromBody] CreateGradeRequest request)
    {
        try
        {
            var result = await _gradeService.CreateGradeAsync(request);

            return CreatedAtAction(nameof(GetGradeById), new { id = result.Id },
                new ApiResponse<GradeDetailDto>
                {
                    Success = true,
                    Message = "Grade created successfully",
                    Data = result
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid grade data",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating grade: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error creating grade",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Updates an existing grade.
    /// </summary>
    /// <param name="id">Grade ID</param>
    /// <param name="request">Grade update request</param>
    /// <returns>Updated grade</returns>
    [HttpPut("{id}")]
    [ProduceResponseType(typeof(ApiResponse<GradeDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGrade(int id, [FromBody] UpdateGradeRequest request)
    {
        try
        {
            var result = await _gradeService.UpdateGradeAsync(id, request);

            return Ok(new ApiResponse<GradeDetailDto>
            {
                Success = true,
                Message = "Grade updated successfully",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Grade not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating grade {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error updating grade",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Deletes a grade (Admin only).
    /// </summary>
    /// <param name="id">Grade ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        try
        {
            var result = await _gradeService.DeleteGradeAsync(id);

            return Ok(new ApiResponse<bool>
            {
                Success = result,
                Message = result ? "Grade deleted successfully" : "Failed to delete grade",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Grade not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting grade {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error deleting grade",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets all grades for a specific student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Student's grades</returns>
    [HttpGet("student/{studentId}")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<GradeListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGradesByStudent(int studentId)
    {
        try
        {
            var result = await _gradeService.GetGradesByStudentAsync(studentId);

            return Ok(new ApiResponse<IEnumerable<GradeListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} grades for student {studentId}",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting student grades: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving grades",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets all grades for a specific course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Course grades</returns>
    [HttpGet("course/{courseId}")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<GradeListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGradesByCourse(int courseId)
    {
        try
        {
            var result = await _gradeService.GetGradesByCourseAsync(courseId);

            return Ok(new ApiResponse<IEnumerable<GradeListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} grades for course {courseId}",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting course grades: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving grades",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Calculates GPA for a student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Student's GPA</returns>
    [HttpGet("student/{studentId}/gpa")]
    [ProduceResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CalculateStudentGPA(int studentId)
    {
        try
        {
            var gpa = await _gradeService.CalculateStudentGPAAsync(studentId);

            return Ok(new ApiResponse<decimal>
            {
                Success = true,
                Message = "GPA calculated successfully",
                Data = gpa
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating GPA: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error calculating GPA",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets grade distribution for a course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Grade distribution statistics</returns>
    [HttpGet("course/{courseId}/distribution")]
    [ProduceResponseType(typeof(ApiResponse<GradeDistributionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseGradeDistribution(int courseId)
    {
        try
        {
            var distribution = await _gradeService.GetCourseGradeDistributionAsync(courseId);

            return Ok(new ApiResponse<GradeDistributionDto>
            {
                Success = true,
                Message = "Grade distribution retrieved successfully",
                Data = distribution
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting grade distribution: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving grade distribution",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets academic report card for a student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Report card with GPA and grades</returns>
    [HttpGet("student/{studentId}/report-card")]
    [ProduceResponseType(typeof(ApiResponse<AcademicReportCardDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentReportCard(int studentId)
    {
        try
        {
            var reportCard = await _gradeService.GetStudentReportCardAsync(studentId);

            return Ok(new ApiResponse<AcademicReportCardDto>
            {
                Success = true,
                Message = "Report card generated successfully",
                Data = reportCard
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Student not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating report card: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error generating report card",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets all grades for student in a specific course.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="courseId">Course ID</param>
    /// <returns>Grades for student-course combination</returns>
    [HttpGet("student/{studentId}/course/{courseId}")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<GradeListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentCourseGrades(int studentId, int courseId)
    {
        try
        {
            var grades = await _gradeService.GetStudentCourseGradesAsync(studentId, courseId);

            return Ok(new ApiResponse<IEnumerable<GradeListDto>>
            {
                Success = true,
                Message = $"Found {grades.Count()} grades for student {studentId} in course {courseId}",
                Data = grades
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting student course grades: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving grades",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets students with grades below minimum threshold.
    /// </summary>
    /// <param name="minimumGrade">Minimum grade threshold (default: 60)</param>
    /// <returns>Students with low grades</returns>
    [HttpGet("low-grades")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<LowGradeStudentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLowGradeStudents([FromQuery] decimal minimumGrade = 60)
    {
        try
        {
            if (minimumGrade < 0 || minimumGrade > 100)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Minimum grade must be between 0 and 100"
                });

            var students = await _gradeService.GetLowGradeStudentsAsync(minimumGrade);

            return Ok(new ApiResponse<IEnumerable<LowGradeStudentDto>>
            {
                Success = true,
                Message = $"Found {students.Count()} students with grades below {minimumGrade}",
                Data = students
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting low grade students: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving low grade students",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets top performing students by GPA.
    /// </summary>
    /// <param name="count">Number of top students (default: 10)</param>
    /// <returns>Top performing students</returns>
    [HttpGet("top-students")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<TopStudentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopStudents([FromQuery] int count = 10)
    {
        try
        {
            if (count < 1 || count > 100)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Count must be between 1 and 100"
                });

            var topStudents = await _gradeService.GetTopStudentsAsync(count);

            return Ok(new ApiResponse<IEnumerable<TopStudentDto>>
            {
                Success = true,
                Message = $"Retrieved top {topStudents.Count()} students",
                Data = topStudents
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting top students: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving top students",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets average grade for a course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Course average grade</returns>
    [HttpGet("course/{courseId}/average")]
    [ProduceResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseAverageGrade(int courseId)
    {
        try
        {
            var average = await _gradeService.GetCourseAverageGradeAsync(courseId);

            return Ok(new ApiResponse<decimal>
            {
                Success = true,
                Message = "Course average grade retrieved",
                Data = average
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting course average: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving course average",
                Data = ex.Message
            });
        }
    }
}
