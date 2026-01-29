using Microsoft.Extensions.DependencyInjection;
using EnglishTrainingCenter.Infrastructure.Data;
using EnglishTrainingCenter.Domain.Interfaces;
using EnglishTrainingCenter.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishTrainingCenter.Infrastructure;

/// <summary>
/// Dependency injection extensions for Infrastructure layer
/// </summary>
public static class InfrastructureDependencyInjection
{
    /// <summary>
    /// Add infrastructure services to DI container
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString)
    {
        // Add DbContext
        services.AddDbContext<ETCContext>(options =>
            options.UseSqlServer(connectionString, sqlServerOptions =>
                sqlServerOptions.MigrationsAssembly(typeof(ETCContext).Assembly.FullName)));

        // Add generic repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}
