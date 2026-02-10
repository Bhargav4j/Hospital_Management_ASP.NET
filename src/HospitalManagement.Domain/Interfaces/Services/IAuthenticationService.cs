namespace HospitalManagement.Domain.Interfaces.Services;

public interface IAuthenticationService
{
    Task<(bool Success, int UserId, int UserType, string Message)> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<(bool Success, string Message)> RegisterAsync(string name, string email, string password, int userType, CancellationToken cancellationToken = default);
}
