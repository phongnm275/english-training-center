using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EnglishTrainingCenter.Application.DTOs.Student;
using EnglishTrainingCenter.Application.Services.Student;
using EnglishTrainingCenter.Common.Utilities;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// Student management endpoints controller
/// </summary>
[ApiController]
[Route("api/v1/students")]
[ApiVersion("1.0")]
[Authorize]
public class StudentController : BaseController
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IStudentService studentService, ILogger<StudentController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    /// <summary>
    /// Get all students with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default 1)</param>
    /// <param name="pageSize">Page size (default 10)</param>
    /// <returns>List of students</returns>
    /// <response code="200">Students retrieved successfully</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<StudentListDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation($"GetAll students endpoint called - Page {pageNumber}, Size {pageSize}");

        var (students, totalCount) = await _studentService.GetAllStudentsAsync(pageNumber, pageSize);

        var pagedResult = new PagedResult<StudentListDto>
        {
            Items = students,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = (pageNumber * pageSize) < totalCount
        };

        return Ok(ApiResponse<PagedResult<StudentListDto>>.Success(pagedResult, "Students retrieved successfully"));
    }

    /// <summary>
    /// Get student by ID
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <returns>Student details</returns>
    /// <response code="200">Student retrieved successfully</response>
    /// <response code="404">Student not found</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<StudentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<StudentDetailDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        _logger.LogInformation($"GetById student endpoint called with ID: {id}");

        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
        {
            return NotFound(ApiResponse<StudentDetailDto>.Error($"Student with ID {id} not found"));
        }

        return Ok(ApiResponse<StudentDetailDto>.Success(student, "Student retrieved successfully"));
    }

    /// <summary>
    /// Create new student
    /// </summary>
    /// <param name="request">Create student request</param>
    /// <returns>Created student</returns>
    /// <response code="201">Student created successfully</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<StudentDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<StudentDetailDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateStudentRequest request)
    {
        _logger.LogInformation($"Create student endpoint called for: {request.FullName}");

        var student = await _studentService.CreateStudentAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, 
            ApiResponse<StudentDetailDto>.Success(student, "Student created successfully"));
    }

    /// <summary>
    /// Update existing student
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="request">Update student request</param>
    /// <returns>Updated student</returns>
    /// <response code="200">Student updated successfully</response>
    /// <response code="404">Student not found</response>
    /// <response code="400">Invalid request</response>
    /// <response code="401">Unauthorized</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<StudentDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<StudentDetailDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<StudentDetailDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStudentRequest request)
    {
        _logger.LogInformation($"Update student endpoint called with ID: {id}");

        var student = await _studentService.UpdateStudentAsync(id, request);
        return Ok(ApiResponse<StudentDetailDto>.Success(student, "Student updated successfully"));
    }

    /// <summary>
    /// Delete student
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <returns>Deletion result</returns>
    /// <response code="200">Student deleted successfully</response>
    /// <response code="404">Student not found</response>
    /// <response code="401">Unauthorized</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        _logger.LogInformation($"Delete student endpoint called with ID: {id}");

        var result = await _studentService.DeleteStudentAsync(id);
        if (!result)
        {
            return NotFound(ApiResponse<object>.Error($"Student with ID {id} not found"));
        }

        return Ok(ApiResponse<object>.Success(new { message = "Student deleted successfully" }));
    }

    /// <summary>
    /// Search students by name, email, or phone
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>Matching students</returns>
    /// <response code="200">Search results</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("search/{searchTerm}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentListDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Search([FromRoute] string searchTerm)
    {
        _logger.LogInformation($"Search students endpoint called with term: {searchTerm}");

        var students = await _studentService.SearchStudentsAsync(searchTerm);
        return Ok(ApiResponse<IEnumerable<StudentListDto>>.Success(students, "Search results"));
    }

    /// <summary>
    /// Get students by status (active/inactive)
    /// </summary>
    /// <param name="isActive">Active status filter</param>
    /// <returns>Students with specified status</returns>
    /// <response code="200">Students retrieved</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("status/{isActive}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentListDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByStatus([FromRoute] bool isActive)
    {
        _logger.LogInformation($"GetByStatus students endpoint called - Status: {isActive}");

        var students = await _studentService.GetStudentsByStatusAsync(isActive);
        return Ok(ApiResponse<IEnumerable<StudentListDto>>.Success(students, "Students retrieved by status"));
    }

    /// <summary>
    /// Get total number of students
    /// </summary>
    /// <returns>Total student count</returns>
    /// <response code="200">Count retrieved</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("count/total")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetTotalCount()
    {
        _logger.LogInformation("GetTotalCount students endpoint called");

        var count = await _studentService.GetTotalStudentCountAsync();
        return Ok(ApiResponse<int>.Success(count, "Total student count"));
    }

    /// <summary>
    /// Get students enrolled in a specific course
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Students in course</returns>
    /// <response code="200">Students retrieved</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("course/{courseId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentListDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByCourse([FromRoute] int courseId)
    {
        _logger.LogInformation($"GetByCourse students endpoint called with courseId: {courseId}");

        var students = await _studentService.GetStudentsInCourseAsync(courseId);
        return Ok(ApiResponse<IEnumerable<StudentListDto>>.Success(students, "Students in course retrieved"));
    }
}
