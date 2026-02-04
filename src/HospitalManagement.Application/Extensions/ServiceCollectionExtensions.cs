using FluentValidation;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Mappings;
using HospitalManagement.Application.Services;
using HospitalManagement.Application.Validators;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HospitalManagement.Application.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to register application layer services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all application layer services, validators, and AutoMapper profiles
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Register FluentValidation validators
        services.AddValidatorsFromAssemblyContaining<PatientCreateDtoValidator>();

        // Register application services
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IOtherStaffService, OtherStaffService>();

        return services;
    }

    /// <summary>
    /// Registers AutoMapper profiles for the application
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        return services;
    }

    /// <summary>
    /// Registers FluentValidation validators for the application
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddFluentValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

    /// <summary>
    /// Registers only the service layer implementations
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IOtherStaffService, OtherStaffService>();

        return services;
    }
}
