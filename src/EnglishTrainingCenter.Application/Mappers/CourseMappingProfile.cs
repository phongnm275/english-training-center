using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Course;
using EnglishTrainingCenter.Domain.Entities;

namespace EnglishTrainingCenter.Application.Mappers;

/// <summary>
/// AutoMapper profile for Course entity mappings.
/// </summary>
public class CourseMappingProfile : Profile
{
    public CourseMappingProfile()
    {
        // Entity to DTO mappings (read operations)
        CreateMap<Course, CourseListDto>()
            .ForMember(dest => dest.EnrolledStudents, opt => opt.Ignore());

        CreateMap<Course, CourseDetailDto>()
            .ForMember(dest => dest.EnrolledStudents, opt => opt.Ignore())
            .ForMember(dest => dest.AvailableCapacity, opt => opt.Ignore())
            .ForMember(dest => dest.ScheduleCount, opt => opt.Ignore());

        // Request to Entity mappings (write operations)
        CreateMap<CreateCourseRequest, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());

        CreateMap<UpdateCourseRequest, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());
    }
}
