using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Extensions;

/// <summary>
/// Extension methods for configuring infrastructure services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration instance</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        // Add DbContext
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "Database connection string 'DefaultConnection' is not configured");
            }

            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);

                sqlOptions.CommandTimeout(30);
                sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });

            // Enable sensitive data logging in development
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }

            // Add logging
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            if (loggerFactory != null)
            {
                options.UseLoggerFactory(loggerFactory);
            }
        });

        // Register repositories
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IOtherStaffRepository, OtherStaffRepository>();
        services.AddScoped<IFreeSlotRepository, FreeSlotRepository>();

        return services;
    }

    /// <summary>
    /// Adds infrastructure services with custom connection string
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="connectionString">Database connection string</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));
        }

        // Add DbContext
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);

                sqlOptions.CommandTimeout(30);
                sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });

            // Enable sensitive data logging in development
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Development")
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }

            // Add logging
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            if (loggerFactory != null)
            {
                options.UseLoggerFactory(loggerFactory);
            }
        });

        // Register repositories
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IOtherStaffRepository, OtherStaffRepository>();
        services.AddScoped<IFreeSlotRepository, FreeSlotRepository>();

        return services;
    }

    /// <summary>
    /// Adds infrastructure services with advanced configuration options
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="configureOptions">Action to configure DbContext options</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<DbContextOptionsBuilder> configureOptions)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (configureOptions == null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        // Add DbContext with custom options
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "Database connection string 'DefaultConnection' is not configured");
            }

            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);

                sqlOptions.CommandTimeout(30);
                sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });

            // Apply custom configuration
            configureOptions(options);

            // Add logging
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            if (loggerFactory != null)
            {
                options.UseLoggerFactory(loggerFactory);
            }
        });

        // Register repositories
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IOtherStaffRepository, OtherStaffRepository>();
        services.AddScoped<IFreeSlotRepository, FreeSlotRepository>();

        return services;
    }

    /// <summary>
    /// Ensures database is created and applies pending migrations
    /// </summary>
    /// <param name="serviceProvider">Service provider</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public static async Task EnsureDatabaseCreatedAsync(this IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            logger.LogInformation("Checking database state");

            // Check if database exists
            var canConnect = await context.Database.CanConnectAsync();

            if (!canConnect)
            {
                logger.LogInformation("Database does not exist. Creating database...");
                await context.Database.EnsureCreatedAsync();
                logger.LogInformation("Database created successfully");
            }
            else
            {
                // Apply pending migrations
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Applying {Count} pending migrations", pendingMigrations.Count());
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Migrations applied successfully");
                }
                else
                {
                    logger.LogInformation("Database is up to date");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while ensuring database is created");
            throw;
        }
    }

    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    /// <param name="serviceProvider">Service provider</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            logger.LogInformation("Checking if database seeding is required");

            // Check if departments already exist
            var hasDepartments = await context.Departments.AnyAsync();

            if (!hasDepartments)
            {
                logger.LogInformation("Seeding database with initial data");
                await context.SaveChangesAsync();
                logger.LogInformation("Database seeded successfully");
            }
            else
            {
                logger.LogInformation("Database already contains data. Skipping seeding.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while seeding database");
            throw;
        }
    }
}
