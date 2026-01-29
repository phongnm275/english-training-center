using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Payment;
using EnglishTrainingCenter.Domain.Entities;

namespace EnglishTrainingCenter.Application.Mappers;

/// <summary>
/// AutoMapper profile for Payment entity mappings.
/// </summary>
public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        // Entity to DTO mappings (read operations)
        CreateMap<Payment, PaymentListDto>();

        CreateMap<Payment, PaymentDetailDto>()
            .ForMember(dest => dest.StudentName, opt => opt.Ignore())
            .ForMember(dest => dest.CourseName, opt => opt.Ignore());

        // Request to Entity mappings (write operations)
        CreateMap<CreatePaymentRequest, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PaymentDate, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceNumber, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

        CreateMap<UpdatePaymentRequest, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StudentId, opt => opt.Ignore())
            .ForMember(dest => dest.CourseId, opt => opt.Ignore())
            .ForMember(dest => dest.Amount, opt => opt.Ignore())
            .ForMember(dest => dest.PaymentDate, opt => opt.Ignore())
            .ForMember(dest => dest.InvoiceNumber, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());
    }
}
