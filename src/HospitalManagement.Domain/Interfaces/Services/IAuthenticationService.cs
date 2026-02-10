using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for authentication operations
/// </summary>
public interface IAuthenticationService
{
    Task<(bool Success, UserType UserType, int UserId, string Message)> ValidateLoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<string> HashPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string password, string hashedPassword);
}
