using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for TreatmentHistory entity operations.
/// Provides data access methods for managing treatment history records in the hospital management system.
/// </summary>
public interface ITreatmentHistoryRepository
{
    /// <summary>
    /// Retrieves all treatment history records from the repository.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of all treatment history records.</returns>
    Task<IEnumerable<TreatmentHistory>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a treatment history record by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the treatment history record.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The treatment history record if found; otherwise, null.</returns>
    Task<TreatmentHistory?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new treatment history record to the repository.
    /// </summary>
    /// <param name="treatmentHistory">The treatment history entity to add.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The added treatment history record with generated identifier.</returns>
    Task<TreatmentHistory> AddAsync(TreatmentHistory treatmentHistory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing treatment history record in the repository.
    /// </summary>
    /// <param name="treatmentHistory">The treatment history entity with updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated treatment history entity.</returns>
    Task<TreatmentHistory> UpdateAsync(TreatmentHistory treatmentHistory, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a treatment history record from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the treatment history record to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the treatment history record was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a treatment history record exists in the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the treatment history record.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the treatment history record exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for treatment history records based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against treatment history properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of treatment history records matching the search criteria.</returns>
    Task<IEnumerable<TreatmentHistory>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
