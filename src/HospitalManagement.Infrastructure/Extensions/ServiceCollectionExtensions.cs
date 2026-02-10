using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagement.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering infrastructure layer services
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<HospitalDbContext>(options =>
        {
            var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            string connectionString;
            if (!string.IsNullOrEmpty(dbServer) && !string.IsNullOrEmpty(dbName) &&
                !string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
            {
                connectionString = $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;";
            }
            else
            {
                connectionString = configuration.GetConnectionString("DefaultConnection") ??
                    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });

        // Register repositories
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IBillRepository, BillRepository>();
        services.AddScoped<ITreatmentHistoryRepository, TreatmentHistoryRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();

        return services;
    }
}
