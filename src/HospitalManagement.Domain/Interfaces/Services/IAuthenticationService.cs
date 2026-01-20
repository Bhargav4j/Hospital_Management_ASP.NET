namespace HospitalManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for authentication and authorization operations.
/// Provides methods for user authentication, token management, and security operations.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates a user with username and password.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>An authentication result containing user information and access token if successful.</returns>
    Task<AuthenticationResultDto?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates an access token.
    /// </summary>
    /// <param name="token">The access token to validate.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the token is valid; otherwise, false.</returns>
    Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes an access token using a refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A new authentication result with refreshed tokens if successful.</returns>
    Task<AuthenticationResultDto?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs out a user by invalidating their tokens.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if logout was successful; otherwise, false.</returns>
    Task<bool> LogoutAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes a user's password.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="currentPassword">The current password.</param>
    /// <param name="newPassword">The new password.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the password was changed successfully; otherwise, false.</returns>
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initiates a password reset request.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the password reset request was initiated successfully; otherwise, false.</returns>
    Task<bool> RequestPasswordResetAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets a user's password using a reset token.
    /// </summary>
    /// <param name="resetToken">The password reset token.</param>
    /// <param name="newPassword">The new password.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the password was reset successfully; otherwise, false.</returns>
    Task<bool> ResetPasswordAsync(string resetToken, string newPassword, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies if a user has a specific role.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="role">The role to check.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the user has the specified role; otherwise, false.</returns>
    Task<bool> HasRoleAsync(int userId, string role, CancellationToken cancellationToken = default);
}

/// <summary>
/// Data Transfer Object for authentication results.
/// </summary>
public record AuthenticationResultDto
{
    public int UserId { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public DateTime TokenExpiresAt { get; init; }
    public DateTime RefreshTokenExpiresAt { get; init; }
}

/// <summary>
/// Data Transfer Object for login credentials.
/// </summary>
public record LoginCredentialsDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

/// <summary>
/// Data Transfer Object for password change requests.
/// </summary>
public record PasswordChangeDto
{
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }
    public required string ConfirmPassword { get; init; }
}

/// <summary>
/// Data Transfer Object for password reset requests.
/// </summary>
public record PasswordResetRequestDto
{
    public required string Email { get; init; }
}

/// <summary>
/// Data Transfer Object for password reset operations.
/// </summary>
public record PasswordResetDto
{
    public required string ResetToken { get; init; }
    public required string NewPassword { get; init; }
    public required string ConfirmPassword { get; init; }
}
