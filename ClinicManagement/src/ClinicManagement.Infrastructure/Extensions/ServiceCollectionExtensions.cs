using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Register DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ClinicManagementDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Register repositories
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IOtherStaffRepository, OtherStaffRepository>();

        return services;
    }
}
