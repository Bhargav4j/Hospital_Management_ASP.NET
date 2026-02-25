namespace ClinicManagement.Domain.Interfaces.Services;

public interface IAwsEnvironmentDetector
{
    bool IsRunningInAws();
    bool IsRunningInEcs();
    bool IsRunningInEc2();
    string EnvironmentName { get; }
}
