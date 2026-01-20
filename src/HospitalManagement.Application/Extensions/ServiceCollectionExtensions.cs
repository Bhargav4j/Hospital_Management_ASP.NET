using HospitalManagement.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagement.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Register services would go here
        // services.AddScoped<IPatientService, PatientService>();
        // etc.

        return services;
    }
}
