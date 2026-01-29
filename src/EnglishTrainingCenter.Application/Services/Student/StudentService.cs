using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Student;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnglishTrainingCenter.Application.Services.Student;

/// <summary>
/// Student service implementation
/// </summary>
public class StudentService : IStudentService
{
    private readonly IRepository<Domain.Entities.Student> _studentRepository;
    private readonly IRepository<StudentCourse> _studentCourseRepository;
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IRepository<Grade> _gradeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentService> _logger;

    public StudentService(
        IRepository<Domain.Entities.Student> studentRepository,
        IRepository<StudentCourse> studentCourseRepository,
        IRepository<Payment> paymentRepository,
        IRepository<Grade> gradeRepository,
        IMapper mapper,
        ILogger<StudentService> logger)
    {
        _studentRepository = studentRepository;
        _studentCourseRepository = studentCourseRepository;
        _paymentRepository = paymentRepository;
        _gradeRepository = gradeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<StudentListDto> Students, int TotalCount)> GetAllStudentsAsync(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation($"Retrieving all students - Page {pageNumber}, Size {pageSize}");

            var students = await _studentRepository.GetAllAsync();
            var totalCount = students.Count();

            var pagedStudents = students
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var studentDtos = new List<StudentListDto>();
            foreach (var student in pagedStudents)
            {
                var enrolledCourses = await GetEnrolledCoursesCountAsync(student.Id);
                studentDtos.Add(new StudentListDto
                {
                    Id = student.Id,
                    FullName = student.FullName,
                    Email = student.Email,
                    Phone = student.Phone,
                    EnrollmentDate = student.EnrollmentDate,
                    IsActive = student.IsActive,
                    EnrolledCourses = enrolledCourses
                });
            }

            _logger.LogInformation($"Retrieved {studentDtos.Count} students");
            return (studentDtos, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving students: {ex.Message}");
            throw;
        }
    }

    public async Task<StudentDetailDto?> GetStudentByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Retrieving student with ID: {id}");

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning($"Student not found with ID: {id}");
                return null;
            }

            var enrolledCourses = await GetEnrolledCoursesCountAsync(id);
            var totalPayments = await GetTotalPaymentsAsync(id);
            var averageGPA = await GetAverageGPAAsync(id);

            var studentDto = new StudentDetailDto
            {
                Id = student.Id,
                UserId = student.UserId,
                FullName = student.FullName,
                Email = student.Email,
                Phone = student.Phone,
                DateOfBirth = student.DateOfBirth,
                Address = student.Address,
                Avatar = student.Avatar,
                EnrollmentDate = student.EnrollmentDate,
                IsActive = student.IsActive,
                CreatedDate = student.CreatedDate,
                ModifiedDate = student.ModifiedDate,
                EnrolledCourses = enrolledCourses,
                TotalPayments = totalPayments,
                AverageGPA = averageGPA
            };

            return studentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving student: {ex.Message}");
            throw;
        }
    }

    public async Task<StudentDetailDto> CreateStudentAsync(CreateStudentRequest request)
    {
        try
        {
            _logger.LogInformation($"Creating new student: {request.FullName}");

            if (await EmailExistsAsync(request.Email))
            {
                _logger.LogWarning($"Email already exists: {request.Email}");
                throw new InvalidOperationException("Email already exists");
            }

            var student = new Domain.Entities.Student
            {
                UserId = request.UserId,
                FullName = request.FullName,
                Email = request.Email,
                Phone = request.Phone,
                DateOfBirth = request.DateOfBirth,
                Address = request.Address,
                Avatar = request.Avatar,
                EnrollmentDate = DateTime.UtcNow,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();

            _logger.LogInformation($"Student created successfully with ID: {student.Id}");

            return new StudentDetailDto
            {
                Id = student.Id,
                UserId = student.UserId,
                FullName = student.FullName,
                Email = student.Email,
                Phone = student.Phone,
                DateOfBirth = student.DateOfBirth,
                Address = student.Address,
                Avatar = student.Avatar,
                EnrollmentDate = student.EnrollmentDate,
                IsActive = student.IsActive,
                CreatedDate = student.CreatedDate,
                ModifiedDate = student.ModifiedDate,
                EnrolledCourses = 0,
                TotalPayments = 0,
                AverageGPA = null
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating student: {ex.Message}");
            throw;
        }
    }

    public async Task<StudentDetailDto> UpdateStudentAsync(int id, UpdateStudentRequest request)
    {
        try
        {
            _logger.LogInformation($"Updating student with ID: {id}");

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning($"Student not found with ID: {id}");
                throw new InvalidOperationException($"Student not found");
            }

            if (student.Email != request.Email && await EmailExistsAsync(request.Email))
            {
                _logger.LogWarning($"Email already exists: {request.Email}");
                throw new InvalidOperationException("Email already exists");
            }

            student.FullName = request.FullName;
            student.Email = request.Email;
            student.Phone = request.Phone;
            student.DateOfBirth = request.DateOfBirth;
            student.Address = request.Address;
            student.Avatar = request.Avatar;
            student.IsActive = request.IsActive;
            student.ModifiedDate = DateTime.UtcNow;

            await _studentRepository.UpdateAsync(student);
            await _studentRepository.SaveChangesAsync();

            _logger.LogInformation($"Student updated successfully with ID: {id}");

            var enrolledCourses = await GetEnrolledCoursesCountAsync(id);
            var totalPayments = await GetTotalPaymentsAsync(id);
            var averageGPA = await GetAverageGPAAsync(id);

            return new StudentDetailDto
            {
                Id = student.Id,
                UserId = student.UserId,
                FullName = student.FullName,
                Email = student.Email,
                Phone = student.Phone,
                DateOfBirth = student.DateOfBirth,
                Address = student.Address,
                Avatar = student.Avatar,
                EnrollmentDate = student.EnrollmentDate,
                IsActive = student.IsActive,
                CreatedDate = student.CreatedDate,
                ModifiedDate = student.ModifiedDate,
                EnrolledCourses = enrolledCourses,
                TotalPayments = totalPayments,
                AverageGPA = averageGPA
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating student: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Deleting student with ID: {id}");

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogWarning($"Student not found with ID: {id}");
                return false;
            }

            await _studentRepository.DeleteAsync(student);
            await _studentRepository.SaveChangesAsync();

            _logger.LogInformation($"Student deleted successfully with ID: {id}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting student: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<StudentListDto>> SearchStudentsAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation($"Searching students with term: {searchTerm}");

            var students = await _studentRepository.GetAllAsync();
            var searchLower = searchTerm.ToLower();

            var foundStudents = students
                .Where(s => s.FullName.ToLower().Contains(searchLower) ||
                           s.Email.ToLower().Contains(searchLower) ||
                           (s.Phone != null && s.Phone.Contains(searchTerm)))
                .ToList();

            var studentDtos = new List<StudentListDto>();
            foreach (var student in foundStudents)
            {
                var enrolledCourses = await GetEnrolledCoursesCountAsync(student.Id);
                studentDtos.Add(new StudentListDto
                {
                    Id = student.Id,
                    FullName = student.FullName,
                    Email = student.Email,
                    Phone = student.Phone,
                    EnrollmentDate = student.EnrollmentDate,
                    IsActive = student.IsActive,
                    EnrolledCourses = enrolledCourses
                });
            }

            _logger.LogInformation($"Found {studentDtos.Count} students matching search term");
            return studentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching students: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<StudentListDto>> GetStudentsByStatusAsync(bool isActive)
    {
        try
        {
            _logger.LogInformation($"Retrieving students with status: {(isActive ? "Active" : "Inactive")}");

            var students = await _studentRepository.GetAllAsync();
            var filteredStudents = students.Where(s => s.IsActive == isActive).ToList();

            var studentDtos = new List<StudentListDto>();
            foreach (var student in filteredStudents)
            {
                var enrolledCourses = await GetEnrolledCoursesCountAsync(student.Id);
                studentDtos.Add(new StudentListDto
                {
                    Id = student.Id,
                    FullName = student.FullName,
                    Email = student.Email,
                    Phone = student.Phone,
                    EnrollmentDate = student.EnrollmentDate,
                    IsActive = student.IsActive,
                    EnrolledCourses = enrolledCourses
                });
            }

            return studentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving students by status: {ex.Message}");
            throw;
        }
    }

    public async Task<int> GetTotalStudentCountAsync()
    {
        try
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Count();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting total student count: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        try
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Any(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking if email exists: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<StudentListDto>> GetStudentsInCourseAsync(int courseId)
    {
        try
        {
            _logger.LogInformation($"Retrieving students in course: {courseId}");

            var studentCourses = await _studentCourseRepository.GetAllAsync();
            var studentIds = studentCourses
                .Where(sc => sc.ClassId == courseId)
                .Select(sc => sc.StudentId)
                .Distinct()
                .ToList();

            var allStudents = await _studentRepository.GetAllAsync();
            var courseStudents = allStudents.Where(s => studentIds.Contains(s.Id)).ToList();

            var studentDtos = new List<StudentListDto>();
            foreach (var student in courseStudents)
            {
                var enrolledCourses = await GetEnrolledCoursesCountAsync(student.Id);
                studentDtos.Add(new StudentListDto
                {
                    Id = student.Id,
                    FullName = student.FullName,
                    Email = student.Email,
                    Phone = student.Phone,
                    EnrollmentDate = student.EnrollmentDate,
                    IsActive = student.IsActive,
                    EnrolledCourses = enrolledCourses
                });
            }

            return studentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving students in course: {ex.Message}");
            throw;
        }
    }

    // Private helper methods
    private async Task<int> GetEnrolledCoursesCountAsync(int studentId)
    {
        var studentCourses = await _studentCourseRepository.GetAllAsync();
        return studentCourses.Count(sc => sc.StudentId == studentId && sc.Status == "Enrolled");
    }

    private async Task<decimal> GetTotalPaymentsAsync(int studentId)
    {
        var payments = await _paymentRepository.GetAllAsync();
        return payments
            .Where(p => p.StudentId == studentId && p.Status == "Completed")
            .Sum(p => p.Amount ?? 0);
    }

    private async Task<decimal?> GetAverageGPAAsync(int studentId)
    {
        var grades = await _gradeRepository.GetAllAsync();
        var studentGrades = grades.Where(g => g.StudentId == studentId).ToList();

        if (!studentGrades.Any())
            return null;

        return studentGrades.Average(g => g.GPA ?? 0);
    }
}
