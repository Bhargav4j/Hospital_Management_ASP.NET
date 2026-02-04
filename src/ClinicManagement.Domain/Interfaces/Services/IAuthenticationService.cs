using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for authentication operations
/// </summary>
public interface IAuthenticationService
{
    Task<(bool IsValid, UserType? UserType, int? UserId)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<string> HashPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string password, string passwordHash);
}
