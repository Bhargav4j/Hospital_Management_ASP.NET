using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Admin entity operations.
/// Provides data access methods for managing administrator records in the hospital management system.
/// </summary>
public interface IAdminRepository
{
    /// <summary>
    /// Retrieves all administrators from the repository.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of all administrators.</returns>
    Task<IEnumerable<Admin>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an administrator by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the administrator.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The administrator if found; otherwise, null.</returns>
    Task<Admin?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new administrator to the repository.
    /// </summary>
    /// <param name="admin">The admin entity to add.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The added administrator with generated identifier.</returns>
    Task<Admin> AddAsync(Admin admin, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing administrator in the repository.
    /// </summary>
    /// <param name="admin">The admin entity with updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated admin entity.</returns>
    Task<Admin> UpdateAsync(Admin admin, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an administrator from the repository by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the administrator to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the administrator was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an administrator exists in the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the administrator.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the administrator exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for administrators based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against administrator properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of administrators matching the search criteria.</returns>
    Task<IEnumerable<Admin>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
