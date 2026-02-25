using Amazon;
using Amazon.CloudWatchLogs;
using ClinicManagement.Application.Extensions;
using ClinicManagement.Domain.Interfaces.Services;
using ClinicManagement.Infrastructure.Extensions;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;

var builder = WebApplication.CreateBuilder(args);

// AWS Systems Manager Parameter Store for configuration
var awsRegionStr = builder.Configuration["AWS:Region"];
var ssmEnabled = builder.Configuration.GetValue<bool>("AWS:SystemsManager:Enabled");
if (!string.IsNullOrEmpty(awsRegionStr) && ssmEnabled)
{
    var ssmPath = builder.Configuration["AWS:SystemsManager:Path"] ?? "/clinic-management/";

    builder.Configuration.AddSystemsManager(config =>
    {
        config.Path = ssmPath;
        config.ReloadAfter = TimeSpan.FromMinutes(5);
        config.Optional = true;
    });
}

if (!string.IsNullOrEmpty(awsRegionStr))
{
    builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
}

// Serilog with CloudWatch sink
var loggerConfig = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);

var cloudWatchEnabled = builder.Configuration.GetValue<bool>("AWS:CloudWatch:Enabled");
if (cloudWatchEnabled && !string.IsNullOrEmpty(awsRegionStr))
{
    var awsRegion = RegionEndpoint.GetBySystemName(awsRegionStr);
    var cloudWatchClient = new AmazonCloudWatchLogsClient(awsRegion);
    var logGroupName = builder.Configuration["AWS:CloudWatch:LogGroup"] ?? "/clinic-management/app";

    loggerConfig.WriteTo.AmazonCloudWatch(
        new CloudWatchSinkOptions
        {
            LogGroupName = logGroupName,
            LogStreamNameProvider = new DefaultLogStreamProvider(),
            TextFormatter = new Serilog.Formatting.Json.JsonFormatter(),
            MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information,
            BatchSizeLimit = 100,
            QueueSizeLimit = 10000,
            Period = TimeSpan.FromSeconds(10),
            CreateLogGroup = true,
            RetryAttempts = 5
        },
        cloudWatchClient);
}

Log.Logger = loggerConfig.CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Log environment detection at startup
var envDetector = app.Services.GetRequiredService<IAwsEnvironmentDetector>();
Log.Information("Application starting in {Environment} environment. AWS detected: {IsAws}, ECS: {IsEcs}, EC2: {IsEc2}",
    envDetector.EnvironmentName, envDetector.IsRunningInAws(), envDetector.IsRunningInEcs(), envDetector.IsRunningInEc2());

var cacheService = app.Services.GetRequiredService<ICacheService>();
Log.Information("Cache service implementation: {CacheType}", cacheService.GetType().Name);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
