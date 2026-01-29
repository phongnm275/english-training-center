using EnglishTrainingCenter.Application.DTOs.Instructor;
using AutoMapper;

namespace EnglishTrainingCenter.Application.Mappers;

/// <summary>
/// AutoMapper profile for instructor entity and DTOs
/// </summary>
public class InstructorMappingProfile : Profile
{
    public InstructorMappingProfile()
    {
        // Instructor to InstructorListDto
        CreateMap<Domain.Entities.Instructor, InstructorListDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        // Instructor to InstructorDetailDto
        CreateMap<Domain.Entities.Instructor, InstructorDetailDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.AssignedCourseCount, opt => opt.Ignore())
            .ForMember(dest => dest.AveragePerformanceRating, opt => opt.Ignore());

        // CreateInstructorRequest to Instructor
        CreateMap<CreateInstructorRequest, Domain.Entities.Instructor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => DateTime.UtcNow));

        // UpdateInstructorRequest to Instructor
        CreateMap<UpdateInstructorRequest, Domain.Entities.Instructor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.JoinDate, opt => opt.Ignore());
    }
}
