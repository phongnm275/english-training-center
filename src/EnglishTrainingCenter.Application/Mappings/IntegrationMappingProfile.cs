using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Integration;
using EnglishTrainingCenter.Domain.Entities;

namespace EnglishTrainingCenter.Application.Mappings;

/// <summary>
/// AutoMapper Profile for Integration DTOs
/// Configures mappings for all Google Calendar, Payment Gateway, Video Conferencing, OAuth, and Webhook DTOs
/// </summary>
public class IntegrationMappingProfile : Profile
{
    public IntegrationMappingProfile()
    {
        // ============================================================================
        // GOOGLE CALENDAR MAPPINGS
        // ============================================================================

        CreateMap<CreateCalendarEventDto, CalendarEventDto>();
        CreateMap<UpdateCalendarEventDto, CalendarEventDto>();

        // ============================================================================
        // PAYMENT GATEWAY MAPPINGS
        // ============================================================================

        CreateMap<CreatePaymentDto, StripeInitiationDto>()
            .ForMember(dest => dest.PaymentIntentId, opt => opt.MapFrom(src => "pi_" + Guid.NewGuid()))
            .ForMember(dest => dest.ClientSecret, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "requires_payment_method"))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<CreatePaymentDto, PayPalPaymentDto>()
            .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => "txn_" + Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "completed"))
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<CreatePaymentDto, PaymentConfirmationDto>();
        CreateMap<CreatePaymentDto, PaymentStatusDto>();

        // ============================================================================
        // VIDEO CONFERENCING MAPPINGS
        // ============================================================================

        CreateMap<CreateVideoConferenceDto, ZoomMeetingDto>()
            .ForMember(dest => dest.MeetingId, opt => opt.MapFrom(src => "zm_" + Guid.NewGuid()))
            .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => (int)(src.EndTime - src.StartTime).TotalMinutes))
            .ForMember(dest => dest.JoinUrl, opt => opt.MapFrom(src => "https://zoom.us/j/" + Guid.NewGuid()));

        CreateMap<CreateVideoConferenceDto, TeamsMeetingDto>()
            .ForMember(dest => dest.MeetingId, opt => opt.MapFrom(src => "tm_" + Guid.NewGuid()))
            .ForMember(dest => dest.Topic, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => (int)(src.EndTime - src.StartTime).TotalMinutes))
            .ForMember(dest => dest.OnlineMeetingUrl, opt => opt.MapFrom(src => "https://teams.microsoft.com/l/meetup-join/" + Guid.NewGuid()));

        CreateMap<ZoomMeetingDto, ZoomMeetingDetailsDto>();
        CreateMap<TeamsMeetingDto, TeamsMeetingDetailsDto>();

        // ============================================================================
        // OAUTH AUTHENTICATION MAPPINGS
        // ============================================================================

        // OAuth user DTOs are typically created directly from OAuth provider responses
        // Mappings are minimal as external provider data doesn't map to domain entities directly

        // ============================================================================
        // WEBHOOK MAPPINGS
        // ============================================================================

        CreateMap<RegisterWebhookDto, WebhookRegistrationDto>()
            .ForMember(dest => dest.WebhookId, opt => opt.MapFrom(src => "whk_" + Guid.NewGuid()))
            .ForMember(dest => dest.Secret, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<UpdateWebhookDto, WebhookDto>();

        // ============================================================================
        // REVERSE MAPPINGS (Optional)
        // ============================================================================

        // Most integrations are one-way (DTO to external service)
        // Reverse mappings are minimal
    }
}
