using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Services;

public class AwsEnvironmentDetector : IAwsEnvironmentDetector
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AwsEnvironmentDetector> _logger;
    private readonly Lazy<bool> _isEcs;
    private readonly Lazy<bool> _isEc2;

    public AwsEnvironmentDetector(IConfiguration configuration, ILogger<AwsEnvironmentDetector> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _isEcs = new Lazy<bool>(DetectEcs);
        _isEc2 = new Lazy<bool>(DetectEc2);
    }

    public string EnvironmentName =>
        IsRunningInEcs() ? "ECS" :
        IsRunningInEc2() ? "EC2" :
        "Local";

    public bool IsRunningInAws() => IsRunningInEcs() || IsRunningInEc2();

    public bool IsRunningInEcs() => _isEcs.Value;

    public bool IsRunningInEc2() => _isEc2.Value;

    private bool DetectEcs()
    {
        var ecsMetadataV4 = Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI_V4");
        var ecsMetadata = Environment.GetEnvironmentVariable("ECS_CONTAINER_METADATA_URI");
        var ecsAgent = Environment.GetEnvironmentVariable("ECS_AGENT_URI");

        var isEcs = !string.IsNullOrEmpty(ecsMetadataV4)
                    || !string.IsNullOrEmpty(ecsMetadata)
                    || !string.IsNullOrEmpty(ecsAgent);

        _logger.LogInformation("ECS detection: {IsEcs} (V4={V4}, V3={V3}, Agent={Agent})",
            isEcs, ecsMetadataV4 ?? "null", ecsMetadata ?? "null", ecsAgent ?? "null");

        return isEcs;
    }

    private bool DetectEc2()
    {
        var awsExecEnv = Environment.GetEnvironmentVariable("AWS_EXECUTION_ENV");
        var awsRegion = Environment.GetEnvironmentVariable("AWS_REGION");
        var awsDefaultRegion = Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION");

        var isEc2 = !string.IsNullOrEmpty(awsExecEnv)
                    || (!string.IsNullOrEmpty(awsRegion) && !string.IsNullOrEmpty(awsDefaultRegion));

        _logger.LogInformation("EC2 detection: {IsEc2} (ExecEnv={ExecEnv}, Region={Region})",
            isEc2, awsExecEnv ?? "null", awsRegion ?? "null");

        return isEc2;
    }
}
