using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Grade;
using EnglishTrainingCenter.Common;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnglishTrainingCenter.Application.Services.Grade;

/// <summary>
/// Service implementation for grade management and academic tracking.
/// Handles grade records, GPA calculations, and academic reporting.
/// </summary>
public class GradeService : IGradeService
{
    private readonly IRepository<Domain.Entities.Grade> _gradeRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<StudentCourse> _studentCourseRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GradeService> _logger;

    public GradeService(
        IRepository<Domain.Entities.Grade> gradeRepository,
        IRepository<Student> studentRepository,
        IRepository<Course> courseRepository,
        IRepository<StudentCourse> studentCourseRepository,
        IMapper mapper,
        ILogger<GradeService> logger)
    {
        _gradeRepository = gradeRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _studentCourseRepository = studentCourseRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<GradeListDto>> GetAllGradesAsync(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation($"Getting all grades with pagination (page: {pageNumber}, size: {pageSize})");

            var grades = (await _gradeRepository.GetAllAsync())
                .OrderByDescending(g => g.GradeDate)
                .ToList();

            var totalCount = grades.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var hasNextPage = pageNumber < totalPages;
            var hasPreviousPage = pageNumber > 1;

            var pagedGrades = grades
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<List<GradeListDto>>(pagedGrades);

            _logger.LogInformation($"Successfully retrieved {pagedGrades.Count} grades from page {pageNumber}");

            return new PagedResult<GradeListDto>
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
            _logger.LogError($"Error retrieving grades: {ex.Message}");
            throw;
        }
    }

    public async Task<GradeDetailDto> GetGradeByIdAsync(int gradeId)
    {
        try
        {
            _logger.LogInformation($"Getting grade with ID: {gradeId}");

            var grade = (await _gradeRepository.GetAllAsync())
                .FirstOrDefault(g => g.Id == gradeId);

            if (grade == null)
            {
                _logger.LogWarning($"Grade with ID {gradeId} not found");
                throw new InvalidOperationException($"Grade with ID {gradeId} not found");
            }

            var gradeDto = _mapper.Map<GradeDetailDto>(grade);

            // Load student and course info
            var student = (await _studentRepository.GetAllAsync())
                .FirstOrDefault(s => s.Id == grade.StudentId);
            
            var course = (await _courseRepository.GetAllAsync())
                .FirstOrDefault(c => c.Id == grade.CourseId);

            if (student != null)
                gradeDto.StudentName = student.FullName;

            if (course != null)
                gradeDto.CourseName = course.CourseName;

            _logger.LogInformation($"Successfully retrieved grade {gradeId}");
            return gradeDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving grade {gradeId}: {ex.Message}");
            throw;
        }
    }

    public async Task<GradeDetailDto> CreateGradeAsync(CreateGradeRequest request)
    {
        try
        {
            _logger.LogInformation($"Creating new grade for student {request.StudentId}");

            // Verify student exists
            var student = (await _studentRepository.GetAllAsync())
                .FirstOrDefault(s => s.Id == request.StudentId);

            if (student == null)
            {
                _logger.LogWarning($"Student with ID {request.StudentId} not found");
                throw new InvalidOperationException($"Student with ID {request.StudentId} not found");
            }

            // Verify course exists
            var course = (await _courseRepository.GetAllAsync())
                .FirstOrDefault(c => c.Id == request.CourseId);

            if (course == null)
            {
                _logger.LogWarning($"Course with ID {request.CourseId} not found");
                throw new InvalidOperationException($"Course with ID {request.CourseId} not found");
            }

            // Check if student is enrolled in course
            var enrollment = (await _studentCourseRepository.GetAllAsync())
                .FirstOrDefault(sc => sc.StudentId == request.StudentId && sc.CourseId == request.CourseId);

            if (enrollment == null)
            {
                _logger.LogWarning($"Student {request.StudentId} is not enrolled in course {request.CourseId}");
                throw new InvalidOperationException($"Student is not enrolled in this course");
            }

            var grade = _mapper.Map<Domain.Entities.Grade>(request);
            grade.GradeDate = DateTime.UtcNow;
            grade.CreatedDate = DateTime.UtcNow;

            var createdGrade = await _gradeRepository.AddAsync(grade);
            await _gradeRepository.SaveChangesAsync();

            _logger.LogInformation($"Successfully created grade with ID: {createdGrade.Id}");

            return _mapper.Map<GradeDetailDto>(createdGrade);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating grade: {ex.Message}");
            throw;
        }
    }

    public async Task<GradeDetailDto> UpdateGradeAsync(int gradeId, UpdateGradeRequest request)
    {
        try
        {
            _logger.LogInformation($"Updating grade with ID: {gradeId}");

            var grade = (await _gradeRepository.GetAllAsync())
                .FirstOrDefault(g => g.Id == gradeId);

            if (grade == null)
            {
                _logger.LogWarning($"Grade with ID {gradeId} not found");
                throw new InvalidOperationException($"Grade with ID {gradeId} not found");
            }

            _mapper.Map(request, grade);
            grade.ModifiedDate = DateTime.UtcNow;

            _gradeRepository.Update(grade);
            await _gradeRepository.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated grade {gradeId}");

            return _mapper.Map<GradeDetailDto>(grade);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating grade {gradeId}: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteGradeAsync(int gradeId)
    {
        try
        {
            _logger.LogInformation($"Deleting grade with ID: {gradeId}");

            var grade = (await _gradeRepository.GetAllAsync())
                .FirstOrDefault(g => g.Id == gradeId);

            if (grade == null)
            {
                _logger.LogWarning($"Grade with ID {gradeId} not found");
                throw new InvalidOperationException($"Grade with ID {gradeId} not found");
            }

            _gradeRepository.Delete(grade);
            await _gradeRepository.SaveChangesAsync();

            _logger.LogInformation($"Successfully deleted grade {gradeId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting grade {gradeId}: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<GradeListDto>> GetGradesByStudentAsync(int studentId)
    {
        try
        {
            _logger.LogInformation($"Getting grades for student: {studentId}");

            var grades = (await _gradeRepository.GetAllAsync())
                .Where(g => g.StudentId == studentId)
                .OrderByDescending(g => g.GradeDate)
                .ToList();

            var dtos = _mapper.Map<List<GradeListDto>>(grades);

            _logger.LogInformation($"Found {dtos.Count} grades for student {studentId}");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving student grades: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<GradeListDto>> GetGradesByCourseAsync(int courseId)
    {
        try
        {
            _logger.LogInformation($"Getting grades for course: {courseId}");

            var grades = (await _gradeRepository.GetAllAsync())
                .Where(g => g.CourseId == courseId)
                .OrderBy(g => g.StudentId)
                .ToList();

            var dtos = _mapper.Map<List<GradeListDto>>(grades);

            _logger.LogInformation($"Found {dtos.Count} grades for course {courseId}");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving course grades: {ex.Message}");
            throw;
        }
    }

    public async Task<decimal> CalculateStudentGPAAsync(int studentId)
    {
        try
        {
            _logger.LogInformation($"Calculating GPA for student: {studentId}");

            var grades = (await _gradeRepository.GetAllAsync())
                .Where(g => g.StudentId == studentId)
                .ToList();

            if (grades.Count == 0)
                return 0;

            // Convert letter grades to GPA points (4.0 scale)
            decimal totalPoints = 0;
            foreach (var grade in grades)
            {
                totalPoints += GradeToGPAPoints(grade.Grade);
            }

            var gpa = totalPoints / grades.Count;
            _logger.LogInformation($"Calculated GPA for student {studentId}: {gpa:F2}");

            return Math.Round(gpa, 2);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating GPA: {ex.Message}");
            throw;
        }
    }

    public async Task<GradeDistributionDto> GetCourseGradeDistributionAsync(int courseId)
    {
        try
        {
            _logger.LogInformation($"Getting grade distribution for course: {courseId}");

            var grades = (await _gradeRepository.GetAllAsync())
                .Where(g => g.CourseId == courseId)
                .ToList();

            var distribution = new GradeDistributionDto
            {
                CourseId = courseId,
                TotalGrades = grades.Count,
                ACount = grades.Count(g => g.Grade == "A"),
                BCount = grades.Count(g => g.Grade == "B"),
                CCount = grades.Count(g => g.Grade == "C"),
                DCount = grades.Count(g => g.Grade == "D"),
                FCount = grades.Count(g => g.Grade == "F"),
                AverageGrade = grades.Any() ? Math.Round(grades.Average(g => GradeToPercentage(g.Grade)), 2) : 0
            };

            _logger.LogInformation($"Retrieved grade distribution for course {courseId}");
            return distribution;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving grade distribution: {ex.Message}");
            throw;
        }
    }

    public async Task<AcademicReportCardDto> GetStudentReportCardAsync(int studentId)
    {
        try
        {
            _logger.LogInformation($"Generating report card for student: {studentId}");

            var student = (await _studentRepository.GetAllAsync())
                .FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                _logger.LogWarning($"Student with ID {studentId} not found");
                throw new InvalidOperationException($"Student with ID {studentId} not found");
            }

            var grades = (await _gradeRepository.GetAllAsync())
                .Where(g => g.StudentId == studentId)
                .ToList();

            var gpa = await CalculateStudentGPAAsync(studentId);

            var courses = await Task.WhenAll(grades
                .Select(g => _courseRepository.GetAllAsync()
                    .ContinueWith(t => t.Result.FirstOrDefault(c => c.Id == g.CourseId)))
                .ToArray());

            var courseGrades = new List<CourseGradeDto>();
            foreach (var grade in grades)
            {
                var course = courses.FirstOrDefault(c => c?.Id == grade.CourseId);
                if (course != null)
                {
                    courseGrades.Add(new CourseGradeDto
                    {
                        CourseId = grade.CourseId,
                        CourseName = course.CourseName,
                        CourseCode = course.CourseCode,
                        Grade = grade.Grade,
                        GradeDate = grade.GradeDate
                    });
                }
            }

            var reportCard = new AcademicReportCardDto
            {
                StudentId = studentId,
                StudentName = student.FullName,
                StudentEmail = student.Email,
                GPA = gpa,
                AcademicStatus = GetAcademicStatus(gpa),
                TotalCoursesCompleted = courseGrades.Count,
                CourseGrades = courseGrades,
                GeneratedDate = DateTime.UtcNow
            };

            _logger.LogInformation($"Generated report card for student {studentId}");
            return reportCard;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating report card: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<GradeListDto>> GetStudentCourseGradesAsync(int studentId, int courseId)
    {
        try
        {
            _logger.LogInformation($"Getting grades for student {studentId} in course {courseId}");

            var grades = (await _gradeRepository.GetAllAsync())
                .Where(g => g.StudentId == studentId && g.CourseId == courseId)
                .OrderByDescending(g => g.GradeDate)
                .ToList();

            var dtos = _mapper.Map<List<GradeListDto>>(grades);

            _logger.LogInformation($"Found {dtos.Count} grades for student {studentId} in course {courseId}");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving student course grades: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<LowGradeStudentDto>> GetLowGradeStudentsAsync(decimal minimumGrade = 60)
    {
        try
        {
            _logger.LogInformation($"Getting students with grades below {minimumGrade}");

            var grades = (await _gradeRepository.GetAllAsync()).ToList();
            var lowGrades = new Dictionary<int, List<Domain.Entities.Grade>>();

            foreach (var grade in grades)
            {
                var percentage = GradeToPercentage(grade.Grade);
                if (percentage < minimumGrade)
                {
                    if (!lowGrades.ContainsKey(grade.StudentId))
                        lowGrades[grade.StudentId] = new List<Domain.Entities.Grade>();

                    lowGrades[grade.StudentId].Add(grade);
                }
            }

            var students = await _studentRepository.GetAllAsync();
            var courses = await _courseRepository.GetAllAsync();

            var result = new List<LowGradeStudentDto>();
            foreach (var studentId in lowGrades.Keys)
            {
                var student = students.FirstOrDefault(s => s.Id == studentId);
                if (student != null)
                {
                    var lowStudentGrades = lowGrades[studentId].Select(g =>
                    {
                        var course = courses.FirstOrDefault(c => c.Id == g.CourseId);
                        return new StudentLowGradeDto
                        {
                            CourseId = g.CourseId,
                            CourseName = course?.CourseName ?? "Unknown",
                            Grade = g.Grade,
                            GradePercentage = GradeToPercentage(g.Grade),
                            GradeDate = g.GradeDate
                        };
                    }).ToList();

                    result.Add(new LowGradeStudentDto
                    {
                        StudentId = studentId,
                        StudentName = student.FullName,
                        LowGrades = lowStudentGrades
                    });
                }
            }

            _logger.LogInformation($"Found {result.Count} students with grades below {minimumGrade}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving low grade students: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<TopStudentDto>> GetTopStudentsAsync(int count = 10)
    {
        try
        {
            _logger.LogInformation($"Getting top {count} students by GPA");

            var students = await _studentRepository.GetAllAsync();
            var topStudents = new List<TopStudentDto>();

            foreach (var student in students)
            {
                var gpa = await CalculateStudentGPAAsync(student.Id);
                topStudents.Add(new TopStudentDto
                {
                    StudentId = student.Id,
                    StudentName = student.FullName,
                    GPA = gpa
                });
            }

            var result = topStudents
                .OrderByDescending(s => s.GPA)
                .Take(count)
                .ToList();

            _logger.LogInformation($"Retrieved top {result.Count} students by GPA");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving top students: {ex.Message}");
            throw;
        }
    }

    public async Task<decimal> GetCourseAverageGradeAsync(int courseId)
    {
        try
        {
            _logger.LogInformation($"Calculating average grade for course: {courseId}");

            var grades = (await _gradeRepository.GetAllAsync())
                .Where(g => g.CourseId == courseId)
                .ToList();

            if (grades.Count == 0)
                return 0;

            var average = Math.Round(grades.Average(g => GradeToPercentage(g.Grade)), 2);
            return average;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating course average: {ex.Message}");
            throw;
        }
    }

    // Helper methods
    private decimal GradeToGPAPoints(string grade)
    {
        return grade.ToUpper() switch
        {
            "A" => 4.0m,
            "B" => 3.0m,
            "C" => 2.0m,
            "D" => 1.0m,
            "F" => 0.0m,
            _ => 0.0m
        };
    }

    private decimal GradeToPercentage(string grade)
    {
        return grade.ToUpper() switch
        {
            "A" => 90,
            "B" => 80,
            "C" => 70,
            "D" => 60,
            "F" => 0,
            _ => 0
        };
    }

    private string GetAcademicStatus(decimal gpa)
    {
        return gpa switch
        {
            >= 3.5m => "Excellent",
            >= 3.0m => "Good",
            >= 2.5m => "Satisfactory",
            >= 2.0m => "Fair",
            > 0 => "Needs Improvement",
            _ => "No Grades"
        };
    }
}
