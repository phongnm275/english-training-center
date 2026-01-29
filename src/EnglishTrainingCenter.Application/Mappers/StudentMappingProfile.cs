using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Student;
using EnglishTrainingCenter.Domain.Entities;

namespace EnglishTrainingCenter.Application.Mappers;

/// <summary>
/// AutoMapper profile for Student entity
/// </summary>
public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        // Student to StudentListDto
        CreateMap<Student, StudentListDto>()
            .ForMember(dest => dest.EnrolledCourses, opt => opt.Ignore());

        // Student to StudentDetailDto
        CreateMap<Student, StudentDetailDto>()
            .ForMember(dest => dest.EnrolledCourses, opt => opt.Ignore())
            .ForMember(dest => dest.TotalPayments, opt => opt.Ignore())
            .ForMember(dest => dest.AverageGPA, opt => opt.Ignore());

        // CreateStudentRequest to Student
        CreateMap<CreateStudentRequest, Student>()
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // UpdateStudentRequest to Student
        CreateMap<UpdateStudentRequest, Student>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.EnrollmentDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
    }
}
