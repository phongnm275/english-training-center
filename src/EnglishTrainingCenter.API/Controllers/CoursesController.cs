using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Course;
using EnglishTrainingCenter.Application.Services.Course;
using EnglishTrainingCenter.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// API controller for course management.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(ICourseService courseService, ILogger<CoursesController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all courses with pagination.
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Records per page (default: 10, max: 50)</param>
    /// <returns>Paginated list of courses</returns>
    [HttpGet]
    [ProduceResponseType(typeof(ApiResponse<PagedResult<CourseListDto>>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllCourses(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 10;

            var result = await _courseService.GetAllCoursesAsync(pageNumber, pageSize);

            return Ok(new ApiResponse<PagedResult<CourseListDto>>
            {
                Success = true,
                Message = $"Retrieved {result.Data.Count} courses",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting courses: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving courses",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets a course by ID.
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Course details</returns>
    [HttpGet("{id}")]
    [ProduceResponseType(typeof(ApiResponse<CourseDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseById(int id)
    {
        try
        {
            var result = await _courseService.GetCourseByIdAsync(id);

            return Ok(new ApiResponse<CourseDetailDto>
            {
                Success = true,
                Message = "Course retrieved successfully",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Course not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting course {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving course",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Creates a new course.
    /// </summary>
    /// <param name="request">Course creation request</param>
    /// <returns>Created course</returns>
    [HttpPost]
    [ProduceResponseType(typeof(ApiResponse<CourseDetailDto>), StatusCodes.Status201Created)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
    {
        try
        {
            var result = await _courseService.CreateCourseAsync(request);

            return CreatedAtAction(nameof(GetCourseById), new { id = result.Id }, 
                new ApiResponse<CourseDetailDto>
                {
                    Success = true,
                    Message = "Course created successfully",
                    Data = result
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid course data",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating course: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error creating course",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Updates an existing course.
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <param name="request">Course update request</param>
    /// <returns>Updated course</returns>
    [HttpPut("{id}")]
    [ProduceResponseType(typeof(ApiResponse<CourseDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseRequest request)
    {
        try
        {
            var result = await _courseService.UpdateCourseAsync(id, request);

            return Ok(new ApiResponse<CourseDetailDto>
            {
                Success = true,
                Message = "Course updated successfully",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Course not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating course {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error updating course",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Deletes a course (Admin only).
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        try
        {
            var result = await _courseService.DeleteCourseAsync(id);

            return Ok(new ApiResponse<bool>
            {
                Success = result,
                Message = result ? "Course deleted successfully" : "Failed to delete course",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = ex.Message,
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting course {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error deleting course",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Searches courses by name or code.
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>Matching courses</returns>
    [HttpGet("search/{searchTerm}")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<CourseListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchCourses(string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Search term is required"
                });

            var result = await _courseService.SearchCoursesAsync(searchTerm);

            return Ok(new ApiResponse<IEnumerable<CourseListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} courses",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching courses: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error searching courses",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets courses by level.
    /// </summary>
    /// <param name="level">Course level (Beginner, Intermediate, Advanced, Professional)</param>
    /// <returns>Courses at specified level</returns>
    [HttpGet("level/{level}")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<CourseListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCoursesByLevel(string level)
    {
        try
        {
            var validLevels = new[] { "Beginner", "Intermediate", "Advanced", "Professional" };
            if (!validLevels.Contains(level, StringComparer.OrdinalIgnoreCase))
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid level. Must be: Beginner, Intermediate, Advanced, or Professional"
                });

            var result = await _courseService.GetCoursesByLevelAsync(level);

            return Ok(new ApiResponse<IEnumerable<CourseListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} courses at {level} level",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting courses by level: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving courses",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets courses by status.
    /// </summary>
    /// <param name="isActive">Is course active (true/false)</param>
    /// <returns>Courses matching status</returns>
    [HttpGet("status/{isActive}")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<CourseListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCoursesByStatus(bool isActive)
    {
        try
        {
            var result = await _courseService.GetCoursesByStatusAsync(isActive);

            return Ok(new ApiResponse<IEnumerable<CourseListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} {(isActive ? "active" : "inactive")} courses",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting courses by status: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving courses",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets courses with available capacity.
    /// </summary>
    /// <returns>Courses with available seats</returns>
    [HttpGet("capacity/available")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<CourseListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCoursesWithCapacity()
    {
        try
        {
            var result = await _courseService.GetCoursesWithCapacityAsync();

            return Ok(new ApiResponse<IEnumerable<CourseListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} courses with available capacity",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting courses with capacity: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving courses",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets total course count.
    /// </summary>
    /// <returns>Total number of courses</returns>
    [HttpGet("count/total")]
    [ProduceResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalCourseCount()
    {
        try
        {
            var count = await _courseService.GetTotalCourseCountAsync();

            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Total course count retrieved",
                Data = count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting course count: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving course count",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets number of students enrolled in a course.
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Number of enrolled students</returns>
    [HttpGet("{id}/enrolled-count")]
    [ProduceResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEnrolledStudentCount(int id)
    {
        try
        {
            // Verify course exists
            await _courseService.GetCourseByIdAsync(id);

            var count = await _courseService.GetEnrolledStudentCountAsync(id);

            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Enrolled student count retrieved",
                Data = count
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Course not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting enrolled count for course {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving enrollment count",
                Data = ex.Message
            });
        }
    }
}
