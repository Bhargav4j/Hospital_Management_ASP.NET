using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Bill entity operations.
/// Provides data access methods for managing billing records in the hospital management system.
/// </summary>
public interface IBillRepository
{
    /// <summary>
    /// Retrieves all bills from the repository.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of all bills.</returns>
    Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a bill by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bill.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The bill if found; otherwise, null.</returns>
    Task<Bill?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new bill to the repository.
    /// </summary>
    /// <param name="bill">The bill entity to add.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The added bill with generated identifier.</returns>
    Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing bill in the repository.
    /// </summary>
    /// <param name="bill">The bill entity with updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated bill entity.</returns>
    Task<Bill> UpdateAsync(Bill bill, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a bill from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bill to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the bill was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a bill exists in the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the bill.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the bill exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for bills based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against bill properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of bills matching the search criteria.</returns>
    Task<IEnumerable<Bill>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
