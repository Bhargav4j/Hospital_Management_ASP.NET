using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagement.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServiceCollectionExtensions));

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
