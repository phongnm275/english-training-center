using EnglishTrainingCenter.Application.DTOs.Dashboard;
using AutoMapper;

namespace EnglishTrainingCenter.Application.Mappers;

/// <summary>
/// AutoMapper profile for dashboard DTOs
/// </summary>
public class DashboardMappingProfile : Profile
{
    public DashboardMappingProfile()
    {
        // Dashboard DTOs don't map from entities directly - they're aggregated
        // These are here for consistency and future expansion
    }
}
