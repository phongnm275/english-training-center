using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Notifications;

namespace EnglishTrainingCenter.Application.Mappings;

/// <summary>
/// AutoMapper profile for notification DTOs
/// </summary>
public class NotificationMappingProfile : Profile
{
    public NotificationMappingProfile()
    {
        // Base notification mappings
        CreateMap<NotificationDto, NotificationDto>()
            .ReverseMap();

        CreateMap<EmailNotificationDto, EmailNotificationDto>()
            .ReverseMap();

        CreateMap<SmsNotificationDto, SmsNotificationDto>()
            .ReverseMap();

        CreateMap<PushNotificationDto, PushNotificationDto>()
            .ReverseMap();

        // Template mappings
        CreateMap<NotificationTemplateDto, NotificationTemplateDto>()
            .ReverseMap();

        CreateMap<CreateTemplateRequestDto, NotificationTemplateDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Active"))
            .ForMember(dest => dest.IsSystem, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ReverseMap();

        CreateMap<UpdateTemplateRequestDto, NotificationTemplateDto>()
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ReverseMap();

        // Preference mappings
        CreateMap<NotificationPreferenceDto, NotificationPreferenceDto>()
            .ReverseMap();

        // Schedule mappings
        CreateMap<ScheduleNotificationRequestDto, ScheduleNotificationRequestDto>()
            .ReverseMap();

        CreateMap<ScheduledNotificationDto, ScheduledNotificationDto>()
            .ReverseMap();

        // Status mappings
        CreateMap<NotificationStatusDto, NotificationStatusDto>()
            .ReverseMap();

        CreateMap<StatusHistoryDto, StatusHistoryDto>()
            .ReverseMap();

        // Analytics mappings
        CreateMap<NotificationStatisticsDto, NotificationStatisticsDto>()
            .ReverseMap();

        CreateMap<TemplateMetricsDto, TemplateMetricsDto>()
            .ReverseMap();

        // System maintenance mappings
        CreateMap<SystemMaintenanceDto, SystemMaintenanceDto>()
            .ReverseMap();

        // Send notification request mappings
        CreateMap<SendNotificationRequestDto, SendNotificationRequestDto>()
            .ReverseMap();
    }
}
