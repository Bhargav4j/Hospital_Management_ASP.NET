using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Appointment entity operations.
/// Provides data access methods for managing appointment records in the hospital management system.
/// </summary>
public interface IAppointmentRepository
{
    /// <summary>
    /// Retrieves all appointments from the repository.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of all appointments.</returns>
    Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an appointment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The appointment if found; otherwise, null.</returns>
    Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new appointment to the repository.
    /// </summary>
    /// <param name="appointment">The appointment entity to add.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The added appointment with generated identifier.</returns>
    Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing appointment in the repository.
    /// </summary>
    /// <param name="appointment">The appointment entity with updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated appointment entity.</returns>
    Task<Appointment> UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an appointment from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the appointment was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an appointment exists in the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the appointment exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for appointments based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against appointment properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of appointments matching the search criteria.</returns>
    Task<IEnumerable<Appointment>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
