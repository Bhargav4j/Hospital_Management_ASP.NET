using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Feedback entity operations.
/// Provides data access methods for managing feedback records in the hospital management system.
/// </summary>
public interface IFeedbackRepository
{
    /// <summary>
    /// Retrieves all feedback records from the repository.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of all feedback records.</returns>
    Task<IEnumerable<Feedback>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a feedback record by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback record.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The feedback record if found; otherwise, null.</returns>
    Task<Feedback?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new feedback record to the repository.
    /// </summary>
    /// <param name="feedback">The feedback entity to add.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The added feedback record with generated identifier.</returns>
    Task<Feedback> AddAsync(Feedback feedback, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing feedback record in the repository.
    /// </summary>
    /// <param name="feedback">The feedback entity with updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated feedback entity.</returns>
    Task<Feedback> UpdateAsync(Feedback feedback, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a feedback record from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback record to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the feedback record was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a feedback record exists in the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback record.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the feedback record exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for feedback records based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against feedback properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of feedback records matching the search criteria.</returns>
    Task<IEnumerable<Feedback>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
