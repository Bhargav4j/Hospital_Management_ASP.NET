using HospitalManagement.Domain.Interfaces.Services;
using HospitalManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagement.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
