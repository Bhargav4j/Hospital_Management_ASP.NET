using ClinicManagement.Application.Services;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagement.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        // Register services
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IOtherStaffService, OtherStaffService>();

        return services;
    }
}
