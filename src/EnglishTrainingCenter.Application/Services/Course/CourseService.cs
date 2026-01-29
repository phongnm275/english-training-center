using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Course;
using EnglishTrainingCenter.Common;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnglishTrainingCenter.Application.Services.Course;

/// <summary>
/// Service implementation for course management operations.
/// Handles CRUD operations, search, filtering, and related business logic.
/// </summary>
public class CourseService : ICourseService
{
    private readonly IRepository<Domain.Entities.Course> _courseRepository;
    private readonly IRepository<StudentCourse> _studentCourseRepository;
    private readonly IRepository<Schedule> _scheduleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CourseService> _logger;

    public CourseService(
        IRepository<Domain.Entities.Course> courseRepository,
        IRepository<StudentCourse> studentCourseRepository,
        IRepository<Schedule> scheduleRepository,
        IMapper mapper,
        ILogger<CourseService> logger)
    {
        _courseRepository = courseRepository;
        _studentCourseRepository = studentCourseRepository;
        _scheduleRepository = scheduleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<CourseListDto>> GetAllCoursesAsync(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation($"Getting all courses with pagination (page: {pageNumber}, size: {pageSize})");

            var courses = (await _courseRepository.GetAllAsync()).OrderBy(c => c.CourseName).ToList();

            var totalCount = courses.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var hasNextPage = pageNumber < totalPages;
            var hasPreviousPage = pageNumber > 1;

            var pagedCourses = courses
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<List<CourseListDto>>(pagedCourses);

            // Populate enrolled student counts
            foreach (var courseDto in dtos)
            {
                courseDto.EnrolledStudents = await GetEnrolledStudentCountAsync(courseDto.Id);
            }

            _logger.LogInformation($"Successfully retrieved {pagedCourses.Count} courses from page {pageNumber}");

            return new PagedResult<CourseListDto>
            {
                Data = dtos,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving courses: {ex.Message}");
            throw;
        }
    }

    public async Task<CourseDetailDto> GetCourseByIdAsync(int courseId)
    {
        try
        {
            _logger.LogInformation($"Getting course with ID: {courseId}");

            var course = (await _courseRepository.GetAllAsync())
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                _logger.LogWarning($"Course with ID {courseId} not found");
                throw new InvalidOperationException($"Course with ID {courseId} not found");
            }

            var courseDto = _mapper.Map<CourseDetailDto>(course);

            // Populate related data
            courseDto.EnrolledStudents = await GetEnrolledStudentCountAsync(courseId);
            courseDto.AvailableCapacity = course.MaxCapacity - courseDto.EnrolledStudents;

            var schedules = (await _scheduleRepository.GetAllAsync())
                .Where(s => s.CourseId == courseId)
                .ToList();
            courseDto.ScheduleCount = schedules.Count;

            _logger.LogInformation($"Successfully retrieved course {courseId}");
            return courseDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving course {courseId}: {ex.Message}");
            throw;
        }
    }

    public async Task<CourseDetailDto> CreateCourseAsync(CreateCourseRequest request)
    {
        try
        {
            _logger.LogInformation($"Creating new course: {request.CourseName}");

            // Check if course code already exists
            if (await CourseCodeExistsAsync(request.CourseCode))
            {
                _logger.LogWarning($"Course code {request.CourseCode} already exists");
                throw new InvalidOperationException($"Course code '{request.CourseCode}' already exists");
            }

            var course = _mapper.Map<Domain.Entities.Course>(request);
            course.CreatedDate = DateTime.UtcNow;
            course.IsActive = true;

            var createdCourse = await _courseRepository.AddAsync(course);
            await _courseRepository.SaveChangesAsync();

            _logger.LogInformation($"Successfully created course with ID: {createdCourse.Id}");

            return _mapper.Map<CourseDetailDto>(createdCourse);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating course: {ex.Message}");
            throw;
        }
    }

    public async Task<CourseDetailDto> UpdateCourseAsync(int courseId, UpdateCourseRequest request)
    {
        try
        {
            _logger.LogInformation($"Updating course with ID: {courseId}");

            var course = (await _courseRepository.GetAllAsync())
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                _logger.LogWarning($"Course with ID {courseId} not found");
                throw new InvalidOperationException($"Course with ID {courseId} not found");
            }

            // Check if new course code conflicts (excluding current course)
            if (request.CourseCode != course.CourseCode && await CourseCodeExistsAsync(request.CourseCode, courseId))
            {
                _logger.LogWarning($"Course code {request.CourseCode} already exists");
                throw new InvalidOperationException($"Course code '{request.CourseCode}' already exists");
            }

            _mapper.Map(request, course);
            course.ModifiedDate = DateTime.UtcNow;

            _courseRepository.Update(course);
            await _courseRepository.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated course {courseId}");

            return _mapper.Map<CourseDetailDto>(course);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating course {courseId}: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteCourseAsync(int courseId)
    {
        try
        {
            _logger.LogInformation($"Deleting course with ID: {courseId}");

            var course = (await _courseRepository.GetAllAsync())
                .FirstOrDefault(c => c.Id == courseId);

            if (course == null)
            {
                _logger.LogWarning($"Course with ID {courseId} not found");
                throw new InvalidOperationException($"Course with ID {courseId} not found");
            }

            // Check if course has enrolled students
            var enrolledCount = await GetEnrolledStudentCountAsync(courseId);
            if (enrolledCount > 0)
            {
                _logger.LogWarning($"Cannot delete course {courseId} with {enrolledCount} enrolled students");
                throw new InvalidOperationException($"Cannot delete course with {enrolledCount} enrolled students. Remove students first.");
            }

            _courseRepository.Delete(course);
            await _courseRepository.SaveChangesAsync();

            _logger.LogInformation($"Successfully deleted course {courseId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting course {courseId}: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<CourseListDto>> SearchCoursesAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation($"Searching courses with term: {searchTerm}");

            var courses = (await _courseRepository.GetAllAsync())
                .Where(c => c.CourseName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                           c.CourseCode.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var dtos = _mapper.Map<List<CourseListDto>>(courses);

            foreach (var courseDto in dtos)
            {
                courseDto.EnrolledStudents = await GetEnrolledStudentCountAsync(courseDto.Id);
            }

            _logger.LogInformation($"Found {dtos.Count} courses matching '{searchTerm}'");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching courses: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<CourseListDto>> GetCoursesByLevelAsync(string level)
    {
        try
        {
            _logger.LogInformation($"Getting courses by level: {level}");

            var courses = (await _courseRepository.GetAllAsync())
                .Where(c => c.Level.Equals(level, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var dtos = _mapper.Map<List<CourseListDto>>(courses);

            foreach (var courseDto in dtos)
            {
                courseDto.EnrolledStudents = await GetEnrolledStudentCountAsync(courseDto.Id);
            }

            _logger.LogInformation($"Found {dtos.Count} courses at level '{level}'");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving courses by level: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<CourseListDto>> GetCoursesByStatusAsync(bool isActive)
    {
        try
        {
            _logger.LogInformation($"Getting courses by status: {(isActive ? "Active" : "Inactive")}");

            var courses = (await _courseRepository.GetAllAsync())
                .Where(c => c.IsActive == isActive)
                .ToList();

            var dtos = _mapper.Map<List<CourseListDto>>(courses);

            foreach (var courseDto in dtos)
            {
                courseDto.EnrolledStudents = await GetEnrolledStudentCountAsync(courseDto.Id);
            }

            _logger.LogInformation($"Found {dtos.Count} {(isActive ? "active" : "inactive")} courses");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving courses by status: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<CourseListDto>> GetCoursesWithCapacityAsync()
    {
        try
        {
            _logger.LogInformation("Getting courses with available capacity");

            var courses = (await _courseRepository.GetAllAsync())
                .Where(c => c.IsActive)
                .ToList();

            var studentCourses = (await _studentCourseRepository.GetAllAsync()).ToList();

            var coursesWithCapacity = new List<CourseListDto>();

            foreach (var course in courses)
            {
                var enrolledCount = studentCourses.Count(sc => sc.CourseId == course.Id);
                if (enrolledCount < course.MaxCapacity)
                {
                    var dto = _mapper.Map<CourseListDto>(course);
                    dto.EnrolledStudents = enrolledCount;
                    coursesWithCapacity.Add(dto);
                }
            }

            _logger.LogInformation($"Found {coursesWithCapacity.Count} courses with available capacity");
            return coursesWithCapacity;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving courses with capacity: {ex.Message}");
            throw;
        }
    }

    public async Task<int> GetTotalCourseCountAsync()
    {
        try
        {
            _logger.LogInformation("Getting total course count");
            var count = (await _courseRepository.GetAllAsync()).Count();
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting total course count: {ex.Message}");
            throw;
        }
    }

    public async Task<int> GetEnrolledStudentCountAsync(int courseId)
    {
        try
        {
            var studentCourses = (await _studentCourseRepository.GetAllAsync())
                .Where(sc => sc.CourseId == courseId)
                .ToList();

            return studentCourses.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting enrolled student count for course {courseId}: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> CourseCodeExistsAsync(string courseCode, int? excludeCourseId = null)
    {
        try
        {
            var courses = await _courseRepository.GetAllAsync();
            var exists = courses.Any(c => 
                c.CourseCode.Equals(courseCode, StringComparison.OrdinalIgnoreCase) &&
                (excludeCourseId == null || c.Id != excludeCourseId.Value));

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking if course code exists: {ex.Message}");
            throw;
        }
    }
}
