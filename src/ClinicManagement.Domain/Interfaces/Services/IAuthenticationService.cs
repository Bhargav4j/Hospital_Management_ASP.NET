namespace ClinicManagement.Domain.Interfaces.Services;

/// <summary>
/// Authentication service interface
/// </summary>
public interface IAuthenticationService
{
    Task<(bool Success, int UserId, string UserType, string Message)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<(bool Success, int UserId, string Message)> RegisterPatientAsync(string name, DateTime birthDate, string email, string password, string phoneNo, string gender, string address, CancellationToken cancellationToken = default);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
