using Microsoft.Extensions.DependencyInjection;
using ClinicManagement.Domain.Interfaces.Services;
using ClinicManagement.Application.Services;

namespace ClinicManagement.Application.Extensions;

/// <summary>
/// Extension methods for configuring application services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
