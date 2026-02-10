using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagement.Application.Extensions;

/// <summary>
/// Extension methods for registering application layer services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        // Register services
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
