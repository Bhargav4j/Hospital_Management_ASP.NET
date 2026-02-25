using Amazon.S3;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Mappings;
using ClinicManagement.Infrastructure.Repositories;
using ClinicManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ClinicManagement.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(DbModelMappingProfile).Assembly);

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ClinicManagementDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
                npgsqlOptions.MigrationsHistoryTable("__efmigrations_history", "public");
            })
            .UseSnakeCaseNamingConvention()
            .EnableSensitiveDataLogging(configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging", false));
        });

        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IOtherStaffRepository, OtherStaffRepository>();

        services.AddSingleton<IAwsEnvironmentDetector, AwsEnvironmentDetector>();

        services.AddAwsServices(configuration);

        return services;
    }

    private static IServiceCollection AddAwsServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // S3
        var s3Enabled = configuration.GetValue<bool>("AWS:S3:Enabled");
        if (s3Enabled)
        {
            services.AddAWSService<IAmazonS3>();
            services.AddSingleton<IS3StorageService, S3StorageService>();
        }

        AddCacheServices(services, configuration);

        return services;
    }

    private static void AddCacheServices(IServiceCollection services, IConfiguration configuration)
    {
        var redisEnabled = configuration.GetValue<bool>("AWS:Redis:Enabled");
        var redisConnectionString = configuration.GetValue<string>("AWS:Redis:ConnectionString");
        var requireAwsEnv = configuration.GetValue<bool>("AWS:FeatureToggle:RequireAwsEnvironment", false);
        var instanceName = configuration.GetValue<string>("AWS:Redis:InstanceName") ?? "hospwithoutdbcontcmp:";

        var shouldUseRedis = redisEnabled && !string.IsNullOrEmpty(redisConnectionString);

        if (shouldUseRedis && requireAwsEnv)
        {
            shouldUseRedis = IsAwsEnvironmentDetected();
        }

        if (shouldUseRedis)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = instanceName;
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RedisCacheService>>();
                try
                {
                    return ConnectionMultiplexer.Connect(redisConnectionString!);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to connect to Redis IConnectionMultiplexer. RemoveByPrefix will be unavailable");
                    return null!;
                }
            });

            services.AddSingleton<ICacheService>(sp =>
            {
                var cache = sp.GetRequiredService<Microsoft.Extensions.Caching.Distributed.IDistributedCache>();
                var logger = sp.GetRequiredService<ILogger<RedisCacheService>>();
                var multiplexer = sp.GetService<IConnectionMultiplexer>();
                return new RedisCacheService(cache, logger, multiplexer, instanceName);
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
            services.AddSingleton<ICacheService, InMemoryCacheService>();
        }
    }

    private static bool IsAwsEnvironmentDetected()
    {
        var ecsMetadataV4 = Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI_V4");
        var ecsMetadata = Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI");
        var ecsAgent = Environment.GetEnvironmentVariable("ECS_AGENT_URI");
        var awsExecEnv = Environment.GetEnvironmentVariable("AWS_EXECUTION_ENV");

        return !string.IsNullOrEmpty(ecsMetadataV4)
               || !string.IsNullOrEmpty(ecsMetadata)
               || !string.IsNullOrEmpty(ecsAgent)
               || !string.IsNullOrEmpty(awsExecEnv);
    }
}
