using AutoMapper;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Application.DTOs;

namespace EnglishTrainingCenter.Application.Mappers;

/// <summary>
/// AutoMapper profile for Student entity and DTOs
/// </summary>
public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        // Student to StudentDto
        CreateMap<Student, StudentDto>()
            .ReverseMap();

        // Student to CreateStudentRequest
        CreateMap<CreateStudentRequest, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

        // Student to UpdateStudentRequest
        CreateMap<UpdateStudentRequest, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.StudentCourses, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore())
            .ForMember(dest => dest.Grades, opt => opt.Ignore());
    }
}

/// <summary>
/// AutoMapper profile for Course and Class entities
/// </summary>
public class CourseMappingProfile : Profile
{
    public CourseMappingProfile()
    {
        CreateMap<Course, CourseDto>()
            .ReverseMap();

        CreateMap<Class, ClassDto>()
            .ReverseMap();
    }
}

/// <summary>
/// AutoMapper profile for User entity
/// </summary>
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ReverseMap();
    }
}
