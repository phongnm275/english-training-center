using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation;

namespace EnglishTrainingCenter.Application;

/// <summary>
/// Dependency injection extensions for Application layer
/// </summary>
public static class ApplicationDependencyInjection
{
    /// <summary>
    /// Add application services to DI container
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(typeof(ApplicationDependencyInjection));

        // Add FluentValidation
        services.AddValidatorsFromAssemblyContaining(typeof(ApplicationDependencyInjection));

        return services;
    }
}
