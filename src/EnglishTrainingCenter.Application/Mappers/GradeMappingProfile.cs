using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Grade;
using EnglishTrainingCenter.Domain.Entities;

namespace EnglishTrainingCenter.Application.Mappers;

/// <summary>
/// AutoMapper profile for Grade entity mappings.
/// </summary>
public class GradeMappingProfile : Profile
{
    public GradeMappingProfile()
    {
        // Entity to DTO mappings (read operations)
        CreateMap<Grade, GradeListDto>();

        CreateMap<Grade, GradeDetailDto>()
            .ForMember(dest => dest.StudentName, opt => opt.Ignore())
            .ForMember(dest => dest.CourseName, opt => opt.Ignore());

        // Request to Entity mappings (write operations)
        CreateMap<CreateGradeRequest, Grade>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.GradeDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

        CreateMap<UpdateGradeRequest, Grade>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StudentId, opt => opt.Ignore())
            .ForMember(dest => dest.CourseId, opt => opt.Ignore())
            .ForMember(dest => dest.GradeDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());
    }
}
