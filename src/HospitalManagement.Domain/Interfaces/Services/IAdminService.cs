namespace HospitalManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for administrator business logic operations.
/// Provides methods for managing administrator data using DTOs for data transfer.
/// </summary>
public interface IAdminService
{
    /// <summary>
    /// Retrieves all administrators.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of admin DTOs.</returns>
    Task<IEnumerable<AdminDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an administrator by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the administrator.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The admin DTO if found; otherwise, null.</returns>
    Task<AdminDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new administrator.
    /// </summary>
    /// <param name="dto">The admin creation DTO containing administrator information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The created admin DTO with generated identifier.</returns>
    Task<AdminDto> CreateAsync(AdminCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing administrator.
    /// </summary>
    /// <param name="id">The unique identifier of the administrator to update.</param>
    /// <param name="dto">The admin update DTO containing updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated admin DTO.</returns>
    Task<AdminDto> UpdateAsync(int id, AdminUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an administrator.
    /// </summary>
    /// <param name="id">The unique identifier of the administrator to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the administrator was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for administrators based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against administrator properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of admin DTOs matching the search criteria.</returns>
    Task<IEnumerable<AdminDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

/// <summary>
/// Data Transfer Object for administrator information.
/// </summary>
public record AdminDto
{
    public int AdminId { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// Data Transfer Object for creating a new administrator.
/// </summary>
public record AdminCreateDto
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Role { get; init; }
}

/// <summary>
/// Data Transfer Object for updating an existing administrator.
/// </summary>
public record AdminUpdateDto
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
}
