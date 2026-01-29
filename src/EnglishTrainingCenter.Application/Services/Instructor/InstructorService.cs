using EnglishTrainingCenter.Application.DTOs.Instructor;
using EnglishTrainingCenter.Common;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace EnglishTrainingCenter.Application.Services.Instructor;

/// <summary>
/// Service implementation for instructor management
/// </summary>
public class InstructorService : IInstructorService
{
    private readonly IRepository<Domain.Entities.Instructor> _instructorRepository;
    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<InstructorCourse> _instructorCourseRepository;
    private readonly IRepository<InstructorPerformance> _performanceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<InstructorService> _logger;

    public InstructorService(
        IRepository<Domain.Entities.Instructor> instructorRepository,
        IRepository<Course> courseRepository,
        IRepository<InstructorCourse> instructorCourseRepository,
        IRepository<InstructorPerformance> performanceRepository,
        IMapper mapper,
        ILogger<InstructorService> logger)
    {
        _instructorRepository = instructorRepository;
        _courseRepository = courseRepository;
        _instructorCourseRepository = instructorCourseRepository;
        _performanceRepository = performanceRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<InstructorListDto>> GetAllInstructorsAsync(int pageNumber, int pageSize)
    {
        try
        {
            _logger.LogInformation($"Retrieving instructors page {pageNumber} with size {pageSize}");

            var instructors = await _instructorRepository.GetAsync();
            var total = instructors.Count();

            var pagedInstructors = instructors
                .OrderBy(x => x.FirstName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<InstructorListDto>>(pagedInstructors);

            return new PagedResult<InstructorListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                Data = dtos
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving instructors: {ex.Message}");
            throw;
        }
    }

    public async Task<InstructorDetailDto> GetInstructorByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Retrieving instructor {id}");

            var instructor = await _instructorRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Instructor with ID {id} not found");

            var assignedCourses = await _instructorCourseRepository.GetAsync(x => x.InstructorId == id);
            var avgRating = await GetAveragePerformanceRatingAsync(id);

            var dto = _mapper.Map<InstructorDetailDto>(instructor);
            dto.AssignedCourseCount = assignedCourses.Count();
            dto.AveragePerformanceRating = avgRating;

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving instructor {id}: {ex.Message}");
            throw;
        }
    }

    public async Task<InstructorDetailDto> CreateInstructorAsync(CreateInstructorRequest request)
    {
        try
        {
            _logger.LogInformation($"Creating instructor: {request.FirstName} {request.LastName}");

            var existingByEmail = await _instructorRepository.GetAsync(x => x.Email == request.Email);
            if (existingByEmail.Any())
                throw new InvalidOperationException($"Instructor with email {request.Email} already exists");

            var instructor = new Domain.Entities.Instructor
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Specialization = request.Specialization,
                Qualification = request.Qualification,
                YearsOfExperience = request.YearsOfExperience,
                BaseSalary = request.BaseSalary,
                SalaryFrequency = request.SalaryFrequency,
                IsActive = true,
                JoinDate = DateTime.UtcNow
            };

            var created = await _instructorRepository.AddAsync(instructor);
            await _instructorRepository.SaveAsync();

            _logger.LogInformation($"Instructor created with ID {created.Id}");
            return _mapper.Map<InstructorDetailDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating instructor: {ex.Message}");
            throw;
        }
    }

    public async Task<InstructorDetailDto> UpdateInstructorAsync(int id, UpdateInstructorRequest request)
    {
        try
        {
            _logger.LogInformation($"Updating instructor {id}");

            var instructor = await _instructorRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Instructor with ID {id} not found");

            // Check email uniqueness if changed
            if (!instructor.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingByEmail = await _instructorRepository.GetAsync(x => x.Email == request.Email);
                if (existingByEmail.Any())
                    throw new InvalidOperationException($"Instructor with email {request.Email} already exists");
            }

            instructor.FirstName = request.FirstName;
            instructor.LastName = request.LastName;
            instructor.Email = request.Email;
            instructor.PhoneNumber = request.PhoneNumber;
            instructor.Specialization = request.Specialization;
            instructor.Qualification = request.Qualification;
            instructor.YearsOfExperience = request.YearsOfExperience;
            instructor.BaseSalary = request.BaseSalary;
            instructor.SalaryFrequency = request.SalaryFrequency;
            instructor.UpdatedAt = DateTime.UtcNow;

            var updated = await _instructorRepository.UpdateAsync(instructor);
            await _instructorRepository.SaveAsync();

            _logger.LogInformation($"Instructor {id} updated successfully");
            return _mapper.Map<InstructorDetailDto>(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating instructor {id}: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteInstructorAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Deleting instructor {id}");

            var instructor = await _instructorRepository.GetByIdAsync(id)
                ?? throw new InvalidOperationException($"Instructor with ID {id} not found");

            // Remove all course assignments first
            var assignments = await _instructorCourseRepository.GetAsync(x => x.InstructorId == id);
            foreach (var assignment in assignments)
            {
                await _instructorCourseRepository.DeleteAsync(assignment);
            }

            await _instructorRepository.DeleteAsync(instructor);
            await _instructorRepository.SaveAsync();

            _logger.LogInformation($"Instructor {id} deleted successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting instructor {id}: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<InstructorListDto>> SearchInstructorsAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation($"Searching instructors with term: {searchTerm}");

            var searchLower = searchTerm.ToLower();
            var instructors = await _instructorRepository.GetAsync(x =>
                x.FirstName.ToLower().Contains(searchLower) ||
                x.LastName.ToLower().Contains(searchLower) ||
                x.Email.ToLower().Contains(searchLower));

            return _mapper.Map<IEnumerable<InstructorListDto>>(instructors);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching instructors: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<InstructorListDto>> GetInstructorsByQualificationAsync(string qualification)
    {
        try
        {
            _logger.LogInformation($"Retrieving instructors with qualification: {qualification}");

            var instructors = await _instructorRepository.GetAsync(x => x.Qualification == qualification);

            return _mapper.Map<IEnumerable<InstructorListDto>>(instructors);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving instructors by qualification: {ex.Message}");
            throw;
        }
    }

    public async Task<InstructorAssignmentDto> AssignCourseAsync(int instructorId, int courseId)
    {
        try
        {
            _logger.LogInformation($"Assigning course {courseId} to instructor {instructorId}");

            var instructor = await _instructorRepository.GetByIdAsync(instructorId)
                ?? throw new InvalidOperationException($"Instructor with ID {instructorId} not found");

            var course = await _courseRepository.GetByIdAsync(courseId)
                ?? throw new InvalidOperationException($"Course with ID {courseId} not found");

            // Check if assignment already exists
            var existingAssignment = await _instructorCourseRepository.GetAsync(x =>
                x.InstructorId == instructorId && x.CourseId == courseId);

            if (existingAssignment.Any())
                throw new InvalidOperationException("Instructor is already assigned to this course");

            var assignment = new InstructorCourse
            {
                InstructorId = instructorId,
                CourseId = courseId,
                AssignmentDate = DateTime.UtcNow
            };

            var created = await _instructorCourseRepository.AddAsync(assignment);
            await _instructorCourseRepository.SaveAsync();

            _logger.LogInformation($"Course {courseId} assigned to instructor {instructorId}");

            return new InstructorAssignmentDto
            {
                InstructorId = instructorId,
                InstructorName = $"{instructor.FirstName} {instructor.LastName}",
                CourseId = courseId,
                CourseName = course.Name,
                CourseCode = course.Code,
                AssignmentDate = created.AssignmentDate
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error assigning course: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> RemoveCourseAssignmentAsync(int instructorId, int courseId)
    {
        try
        {
            _logger.LogInformation($"Removing course {courseId} assignment from instructor {instructorId}");

            var assignment = (await _instructorCourseRepository.GetAsync(x =>
                x.InstructorId == instructorId && x.CourseId == courseId)).FirstOrDefault()
                ?? throw new InvalidOperationException("Assignment not found");

            await _instructorCourseRepository.DeleteAsync(assignment);
            await _instructorCourseRepository.SaveAsync();

            _logger.LogInformation($"Course {courseId} removed from instructor {instructorId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error removing course assignment: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<InstructorAssignmentDto>> GetInstructorCoursesAsync(int instructorId)
    {
        try
        {
            _logger.LogInformation($"Retrieving courses for instructor {instructorId}");

            var instructor = await _instructorRepository.GetByIdAsync(instructorId)
                ?? throw new InvalidOperationException($"Instructor with ID {instructorId} not found");

            var assignments = await _instructorCourseRepository.GetAsync(x => x.InstructorId == instructorId);

            var result = new List<InstructorAssignmentDto>();
            foreach (var assignment in assignments)
            {
                var course = await _courseRepository.GetByIdAsync(assignment.CourseId);
                result.Add(new InstructorAssignmentDto
                {
                    InstructorId = instructorId,
                    InstructorName = $"{instructor.FirstName} {instructor.LastName}",
                    CourseId = course.Id,
                    CourseName = course.Name,
                    CourseCode = course.Code,
                    AssignmentDate = assignment.AssignmentDate
                });
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving instructor courses: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<InstructorListDto>> GetCourseInstructorsAsync(int courseId)
    {
        try
        {
            _logger.LogInformation($"Retrieving instructors for course {courseId}");

            var course = await _courseRepository.GetByIdAsync(courseId)
                ?? throw new InvalidOperationException($"Course with ID {courseId} not found");

            var assignments = await _instructorCourseRepository.GetAsync(x => x.CourseId == courseId);

            var instructorIds = assignments.Select(x => x.InstructorId).ToList();
            var instructors = await _instructorRepository.GetAsync(x => instructorIds.Contains(x.Id));

            return _mapper.Map<IEnumerable<InstructorListDto>>(instructors);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving course instructors: {ex.Message}");
            throw;
        }
    }

    public async Task<InstructorDetailDto> UpdateSalaryAsync(int instructorId, decimal baseSalary, string salaryFrequency)
    {
        try
        {
            _logger.LogInformation($"Updating salary for instructor {instructorId}");

            var instructor = await _instructorRepository.GetByIdAsync(instructorId)
                ?? throw new InvalidOperationException($"Instructor with ID {instructorId} not found");

            if (baseSalary <= 0)
                throw new InvalidOperationException("Base salary must be greater than zero");

            var validFrequencies = new[] { "Monthly", "Quarterly", "Yearly" };
            if (!validFrequencies.Contains(salaryFrequency))
                throw new InvalidOperationException($"Invalid salary frequency. Must be one of: {string.Join(", ", validFrequencies)}");

            instructor.BaseSalary = baseSalary;
            instructor.SalaryFrequency = salaryFrequency;
            instructor.UpdatedAt = DateTime.UtcNow;

            var updated = await _instructorRepository.UpdateAsync(instructor);
            await _instructorRepository.SaveAsync();

            _logger.LogInformation($"Salary updated for instructor {instructorId}");
            return _mapper.Map<InstructorDetailDto>(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating salary: {ex.Message}");
            throw;
        }
    }

    public async Task<decimal> CalculateSalaryAsync(int instructorId)
    {
        try
        {
            _logger.LogInformation($"Calculating salary for instructor {instructorId}");

            var instructor = await _instructorRepository.GetByIdAsync(instructorId)
                ?? throw new InvalidOperationException($"Instructor with ID {instructorId} not found");

            var assignedCourses = await _instructorCourseRepository.GetAsync(x => x.InstructorId == instructorId);
            var courseCount = assignedCourses.Count();
            var experienceBonus = instructor.YearsOfExperience * 0.02m; // 2% per year of experience

            decimal salary = instructor.BaseSalary;

            // Add 5% per assigned course (up to maximum 50%)
            var courseBonus = Math.Min(courseCount * 0.05m, 0.5m);
            salary = salary * (1 + courseBonus + experienceBonus);

            _logger.LogInformation($"Calculated salary for instructor {instructorId}: {salary}");
            return Math.Round(salary, 2);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating salary: {ex.Message}");
            throw;
        }
    }

    public async Task<InstructorPerformanceDto> AddPerformanceRatingAsync(int instructorId, int rating, string comments)
    {
        try
        {
            _logger.LogInformation($"Adding performance rating for instructor {instructorId}");

            var instructor = await _instructorRepository.GetByIdAsync(instructorId)
                ?? throw new InvalidOperationException($"Instructor with ID {instructorId} not found");

            if (rating < 1 || rating > 5)
                throw new InvalidOperationException("Rating must be between 1 and 5");

            var performance = new InstructorPerformance
            {
                InstructorId = instructorId,
                Rating = rating,
                Comments = comments,
                RatingDate = DateTime.UtcNow
            };

            var created = await _performanceRepository.AddAsync(performance);
            await _performanceRepository.SaveAsync();

            _logger.LogInformation($"Performance rating added for instructor {instructorId}");

            return new InstructorPerformanceDto
            {
                InstructorId = instructorId,
                InstructorName = $"{instructor.FirstName} {instructor.LastName}",
                Rating = created.Rating,
                Comments = created.Comments,
                RatingDate = created.RatingDate
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding performance rating: {ex.Message}");
            throw;
        }
    }

    public async Task<decimal> GetAveragePerformanceRatingAsync(int instructorId)
    {
        try
        {
            var ratings = await _performanceRepository.GetAsync(x => x.InstructorId == instructorId);

            if (!ratings.Any())
                return 0;

            var average = ratings.Average(x => x.Rating);
            return Math.Round(average, 2);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating average performance rating: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<InstructorListDto>> GetInstructorsByStatusAsync(bool isActive)
    {
        try
        {
            _logger.LogInformation($"Retrieving instructors by status: {(isActive ? "Active" : "Inactive")}");

            var instructors = await _instructorRepository.GetAsync(x => x.IsActive == isActive);

            return _mapper.Map<IEnumerable<InstructorListDto>>(instructors);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving instructors by status: {ex.Message}");
            throw;
        }
    }

    public async Task<InstructorDetailDto> UpdateInstructorStatusAsync(int instructorId, bool isActive)
    {
        try
        {
            _logger.LogInformation($"Updating status for instructor {instructorId}");

            var instructor = await _instructorRepository.GetByIdAsync(instructorId)
                ?? throw new InvalidOperationException($"Instructor with ID {instructorId} not found");

            instructor.IsActive = isActive;
            instructor.UpdatedAt = DateTime.UtcNow;

            var updated = await _instructorRepository.UpdateAsync(instructor);
            await _instructorRepository.SaveAsync();

            _logger.LogInformation($"Status updated for instructor {instructorId}");
            return _mapper.Map<InstructorDetailDto>(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating instructor status: {ex.Message}");
            throw;
        }
    }

    public async Task<InstructorStatisticsDto> GetInstructorStatisticsAsync(int instructorId)
    {
        try
        {
            _logger.LogInformation($"Retrieving statistics for instructor {instructorId}");

            var instructor = await _instructorRepository.GetByIdAsync(instructorId)
                ?? throw new InvalidOperationException($"Instructor with ID {instructorId} not found");

            var assignedCourses = await _instructorCourseRepository.GetAsync(x => x.InstructorId == instructorId);
            var avgRating = await GetAveragePerformanceRatingAsync(instructorId);
            var calculatedSalary = await CalculateSalaryAsync(instructorId);

            return new InstructorStatisticsDto
            {
                InstructorId = instructorId,
                InstructorName = $"{instructor.FirstName} {instructor.LastName}",
                TotalCoursesAssigned = assignedCourses.Count(),
                YearsOfExperience = instructor.YearsOfExperience,
                BaseSalary = instructor.BaseSalary,
                CalculatedSalary = calculatedSalary,
                AveragePerformanceRating = avgRating,
                JoinDate = instructor.JoinDate,
                IsActive = instructor.IsActive
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving instructor statistics: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<InstructorPerformanceDto>> GetTopRatedInstructorsAsync(int count)
    {
        try
        {
            _logger.LogInformation($"Retrieving top {count} rated instructors");

            var allInstructors = await _instructorRepository.GetAsync();

            var topInstructors = new List<InstructorPerformanceDto>();
            foreach (var instructor in allInstructors.OrderByDescending(x => x.Id).Take(count))
            {
                var avgRating = await GetAveragePerformanceRatingAsync(instructor.Id);
                topInstructors.Add(new InstructorPerformanceDto
                {
                    InstructorId = instructor.Id,
                    InstructorName = $"{instructor.FirstName} {instructor.LastName}",
                    Rating = (int)Math.Round(avgRating),
                    Comments = $"Average rating: {avgRating}",
                    RatingDate = DateTime.UtcNow
                });
            }

            return topInstructors.OrderByDescending(x => x.Rating).Take(count);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving top-rated instructors: {ex.Message}");
            throw;
        }
    }
}
