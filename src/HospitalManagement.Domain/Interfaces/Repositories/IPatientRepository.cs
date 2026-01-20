using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Patient entity operations.
/// Provides data access methods for managing patient records in the hospital management system.
/// </summary>
public interface IPatientRepository
{
    /// <summary>
    /// Retrieves all patients from the repository.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of all patients.</returns>
    Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a patient by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The patient if found; otherwise, null.</returns>
    Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new patient to the repository.
    /// </summary>
    /// <param name="patient">The patient entity to add.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The added patient with generated identifier.</returns>
    Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing patient in the repository.
    /// </summary>
    /// <param name="patient">The patient entity with updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated patient entity.</returns>
    Task<Patient> UpdateAsync(Patient patient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a patient from the repository by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the patient to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the patient was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a patient exists in the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the patient exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for patients based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against patient properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of patients matching the search criteria.</returns>
    Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
