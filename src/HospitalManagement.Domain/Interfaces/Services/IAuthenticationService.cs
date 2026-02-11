namespace HospitalManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for authentication operations
/// </summary>
public interface IAuthenticationService
{
    Task<(bool Success, int UserId, string UserType)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<string> HashPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string password, string hashedPassword);
}
