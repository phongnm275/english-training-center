using EnglishTrainingCenter.Application.DTOs.Instructor;
using EnglishTrainingCenter.Application.Services.Instructor;
using EnglishTrainingCenter.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// API controller for instructor management and course assignments.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class InstructorsController : ControllerBase
{
    private readonly IInstructorService _instructorService;
    private readonly ILogger<InstructorsController> _logger;

    public InstructorsController(IInstructorService instructorService, ILogger<InstructorsController> logger)
    {
        _instructorService = instructorService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all instructors with pagination.
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Records per page (default: 10, max: 50)</param>
    /// <returns>Paginated list of instructors</returns>
    [HttpGet]
    [ProduceResponseType(typeof(ApiResponse<PagedResult<InstructorListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllInstructors(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 10;

            var result = await _instructorService.GetAllInstructorsAsync(pageNumber, pageSize);

            return Ok(new ApiResponse<PagedResult<InstructorListDto>>
            {
                Success = true,
                Message = $"Retrieved {result.Data.Count} instructors",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting instructors: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving instructors",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets an instructor by ID.
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <returns>Instructor details</returns>
    [HttpGet("{id}")]
    [ProduceResponseType(typeof(ApiResponse<InstructorDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInstructorById(int id)
    {
        try
        {
            var result = await _instructorService.GetInstructorByIdAsync(id);

            return Ok(new ApiResponse<InstructorDetailDto>
            {
                Success = true,
                Message = "Instructor retrieved successfully",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Instructor not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting instructor {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving instructor",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Creates a new instructor.
    /// </summary>
    /// <param name="request">Instructor creation request</param>
    /// <returns>Created instructor</returns>
    [HttpPost]
    [ProduceResponseType(typeof(ApiResponse<InstructorDetailDto>), StatusCodes.Status201Created)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateInstructor([FromBody] CreateInstructorRequest request)
    {
        try
        {
            var result = await _instructorService.CreateInstructorAsync(request);

            return CreatedAtAction(nameof(GetInstructorById), new { id = result.Id },
                new ApiResponse<InstructorDetailDto>
                {
                    Success = true,
                    Message = "Instructor created successfully",
                    Data = result
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid instructor data",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating instructor: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error creating instructor",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Updates an existing instructor.
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <param name="request">Instructor update request</param>
    /// <returns>Updated instructor</returns>
    [HttpPut("{id}")]
    [ProduceResponseType(typeof(ApiResponse<InstructorDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateInstructor(int id, [FromBody] UpdateInstructorRequest request)
    {
        try
        {
            var result = await _instructorService.UpdateInstructorAsync(id, request);

            return Ok(new ApiResponse<InstructorDetailDto>
            {
                Success = true,
                Message = "Instructor updated successfully",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Instructor not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating instructor {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error updating instructor",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Deletes an instructor (Admin only).
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInstructor(int id)
    {
        try
        {
            var result = await _instructorService.DeleteInstructorAsync(id);

            return Ok(new ApiResponse<bool>
            {
                Success = result,
                Message = result ? "Instructor deleted successfully" : "Failed to delete instructor",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Instructor not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting instructor {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error deleting instructor",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Searches instructors by name or email.
    /// </summary>
    /// <param name="searchTerm">Search term (name or email)</param>
    /// <returns>Matching instructors</returns>
    [HttpGet("search")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<InstructorListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchInstructors([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Search term is required"
                });

            var result = await _instructorService.SearchInstructorsAsync(searchTerm);

            return Ok(new ApiResponse<IEnumerable<InstructorListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} instructors matching '{searchTerm}'",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching instructors: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error searching instructors",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets instructors by qualification level.
    /// </summary>
    /// <param name="qualification">Qualification level (High School, Bachelor, Master, PhD)</param>
    /// <returns>Instructors with specified qualification</returns>
    [HttpGet("by-qualification")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<InstructorListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByQualification([FromQuery] string qualification)
    {
        try
        {
            var validQualifications = new[] { "High School", "Bachelor", "Master", "PhD" };
            if (!validQualifications.Contains(qualification))
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Invalid qualification. Must be one of: {string.Join(", ", validQualifications)}"
                });

            var result = await _instructorService.GetInstructorsByQualificationAsync(qualification);

            return Ok(new ApiResponse<IEnumerable<InstructorListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} instructors with {qualification} qualification",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving instructors by qualification: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving instructors",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets instructors by status (Active/Inactive).
    /// </summary>
    /// <param name="isActive">Active status filter</param>
    /// <returns>Filtered instructors</returns>
    [HttpGet("by-status")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<InstructorListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStatus([FromQuery] bool isActive)
    {
        try
        {
            var result = await _instructorService.GetInstructorsByStatusAsync(isActive);

            return Ok(new ApiResponse<IEnumerable<InstructorListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} {(isActive ? "active" : "inactive")} instructors",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving instructors by status: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving instructors",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Updates instructor status (Active/Inactive).
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <param name="isActive">New active status</param>
    /// <returns>Updated instructor</returns>
    [HttpPatch("{id}/status")]
    [ProduceResponseType(typeof(ApiResponse<InstructorDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, [FromQuery] bool isActive)
    {
        try
        {
            var result = await _instructorService.UpdateInstructorStatusAsync(id, isActive);

            return Ok(new ApiResponse<InstructorDetailDto>
            {
                Success = true,
                Message = $"Instructor status updated to {(isActive ? "Active" : "Inactive")}",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Instructor not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating instructor status: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error updating status",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Assigns a course to an instructor.
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <param name="courseId">Course ID to assign</param>
    /// <returns>Assignment details</returns>
    [HttpPost("{instructorId}/courses/{courseId}")]
    [ProduceResponseType(typeof(ApiResponse<InstructorAssignmentDto>), StatusCodes.Status201Created)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignCourse(int instructorId, int courseId)
    {
        try
        {
            var result = await _instructorService.AssignCourseAsync(instructorId, courseId);

            return CreatedAtAction(nameof(GetInstructorCourses), new { instructorId },
                new ApiResponse<InstructorAssignmentDto>
                {
                    Success = true,
                    Message = "Course assigned successfully",
                    Data = result
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Assignment failed",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error assigning course: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error assigning course",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Removes a course assignment from an instructor.
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <param name="courseId">Course ID to unassign</param>
    /// <returns>Success status</returns>
    [HttpDelete("{instructorId}/courses/{courseId}")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveCourseAssignment(int instructorId, int courseId)
    {
        try
        {
            var result = await _instructorService.RemoveCourseAssignmentAsync(instructorId, courseId);

            return Ok(new ApiResponse<bool>
            {
                Success = result,
                Message = result ? "Course assignment removed successfully" : "Failed to remove assignment",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Assignment not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing course assignment: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error removing assignment",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets all courses assigned to an instructor.
    /// </summary>
    /// <param name="instructorId">Instructor ID</param>
    /// <returns>Assigned courses</returns>
    [HttpGet("{instructorId}/courses")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<InstructorAssignmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInstructorCourses(int instructorId)
    {
        try
        {
            var result = await _instructorService.GetInstructorCoursesAsync(instructorId);

            return Ok(new ApiResponse<IEnumerable<InstructorAssignmentDto>>
            {
                Success = true,
                Message = $"Retrieved {result.Count()} assigned courses",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Instructor not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving instructor courses: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving courses",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets all instructors for a specific course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Course instructors</returns>
    [HttpGet("course/{courseId}")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<InstructorListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCourseInstructors(int courseId)
    {
        try
        {
            var result = await _instructorService.GetCourseInstructorsAsync(courseId);

            return Ok(new ApiResponse<IEnumerable<InstructorListDto>>
            {
                Success = true,
                Message = $"Retrieved {result.Count()} instructors for course",
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
            _logger.LogError($"Error retrieving course instructors: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving instructors",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Updates instructor salary information.
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <param name="baseSalary">New base salary</param>
    /// <param name="salaryFrequency">Salary frequency (Monthly/Quarterly/Yearly)</param>
    /// <returns>Updated instructor</returns>
    [HttpPatch("{id}/salary")]
    [ProduceResponseType(typeof(ApiResponse<InstructorDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSalary(int id, [FromQuery] decimal baseSalary, [FromQuery] string salaryFrequency)
    {
        try
        {
            var result = await _instructorService.UpdateSalaryAsync(id, baseSalary, salaryFrequency);

            return Ok(new ApiResponse<InstructorDetailDto>
            {
                Success = true,
                Message = "Salary updated successfully",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid salary data",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating salary: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error updating salary",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Calculates instructor salary based on experience and courses.
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <returns>Calculated salary amount</returns>
    [HttpGet("{id}/calculated-salary")]
    [ProduceResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CalculateSalary(int id)
    {
        try
        {
            var result = await _instructorService.CalculateSalaryAsync(id);

            return Ok(new ApiResponse<decimal>
            {
                Success = true,
                Message = "Salary calculated successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating salary: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error calculating salary",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Adds a performance rating for an instructor.
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <param name="rating">Rating 1-5</param>
    /// <param name="comments">Performance comments</param>
    /// <returns>Performance record</returns>
    [HttpPost("{id}/performance-rating")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<InstructorPerformanceDto>), StatusCodes.Status201Created)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPerformanceRating(int id, [FromQuery] int rating, [FromQuery] string comments)
    {
        try
        {
            if (rating < 1 || rating > 5)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Rating must be between 1 and 5"
                });

            var result = await _instructorService.AddPerformanceRatingAsync(id, rating, comments);

            return CreatedAtAction(nameof(GetAveragePerformanceRating), new { id },
                new ApiResponse<InstructorPerformanceDto>
                {
                    Success = true,
                    Message = "Performance rating added successfully",
                    Data = result
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid rating data",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding performance rating: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error adding rating",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets average performance rating for an instructor.
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <returns>Average performance rating</returns>
    [HttpGet("{id}/average-rating")]
    [ProduceResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAveragePerformanceRating(int id)
    {
        try
        {
            var result = await _instructorService.GetAveragePerformanceRatingAsync(id);

            return Ok(new ApiResponse<decimal>
            {
                Success = true,
                Message = "Average rating retrieved",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting average rating: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving rating",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets instructor statistics and analytics.
    /// </summary>
    /// <param name="id">Instructor ID</param>
    /// <returns>Instructor statistics</returns>
    [HttpGet("{id}/statistics")]
    [ProduceResponseType(typeof(ApiResponse<InstructorStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatistics(int id)
    {
        try
        {
            var result = await _instructorService.GetInstructorStatisticsAsync(id);

            return Ok(new ApiResponse<InstructorStatisticsDto>
            {
                Success = true,
                Message = "Statistics retrieved",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Instructor not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting statistics: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving statistics",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets top-rated instructors.
    /// </summary>
    /// <param name="count">Number of top instructors (default: 10)</param>
    /// <returns>Top-rated instructors</returns>
    [HttpGet("top-rated")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<InstructorPerformanceDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopRated([FromQuery] int count = 10)
    {
        try
        {
            if (count < 1 || count > 100)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Count must be between 1 and 100"
                });

            var result = await _instructorService.GetTopRatedInstructorsAsync(count);

            return Ok(new ApiResponse<IEnumerable<InstructorPerformanceDto>>
            {
                Success = true,
                Message = $"Retrieved top {result.Count()} rated instructors",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting top-rated instructors: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving top-rated instructors",
                Data = ex.Message
            });
        }
    }
}
