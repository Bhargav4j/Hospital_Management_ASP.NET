using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Doctor entity operations.
/// Provides data access methods for managing doctor records in the hospital management system.
/// </summary>
public interface IDoctorRepository
{
    /// <summary>
    /// Retrieves all doctors from the repository.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of all doctors.</returns>
    Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a doctor by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The doctor if found; otherwise, null.</returns>
    Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new doctor to the repository.
    /// </summary>
    /// <param name="doctor">The doctor entity to add.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The added doctor with generated identifier.</returns>
    Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing doctor in the repository.
    /// </summary>
    /// <param name="doctor">The doctor entity with updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated doctor entity.</returns>
    Task<Doctor> UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a doctor from the repository by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the doctor was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a doctor exists in the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the doctor exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for doctors based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against doctor properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of doctors matching the search criteria.</returns>
    Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
