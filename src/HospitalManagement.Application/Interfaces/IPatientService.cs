using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

/// <summary>
/// Service interface for Patient operations
/// </summary>
public interface IPatientService
{
    /// <summary>
    /// Gets all patients asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of patient DTOs</returns>
    Task<IEnumerable<PatientDto>> GetAllPatientsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a patient by ID asynchronously
    /// </summary>
    /// <param name="id">Patient identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Patient DTO if found, null otherwise</returns>
    Task<PatientDto?> GetPatientByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a patient by email asynchronously
    /// </summary>
    /// <param name="email">Patient email address</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Patient DTO if found, null otherwise</returns>
    Task<PatientDto?> GetPatientByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new patient asynchronously
    /// </summary>
    /// <param name="patientCreateDto">Patient creation data</param>
    /// <param name="createdBy">User creating the record</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created patient DTO</returns>
    Task<PatientDto> CreatePatientAsync(PatientCreateDto patientCreateDto, string createdBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing patient asynchronously
    /// </summary>
    /// <param name="patientUpdateDto">Patient update data</param>
    /// <param name="modifiedBy">User modifying the record</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated patient DTO if successful, null if not found</returns>
    Task<PatientDto?> UpdatePatientAsync(PatientUpdateDto patientUpdateDto, string modifiedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a patient asynchronously
    /// </summary>
    /// <param name="id">Patient identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeletePatientAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets active patients asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of active patient DTOs</returns>
    Task<IEnumerable<PatientDto>> GetActivePatientsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches patients by name asynchronously
    /// </summary>
    /// <param name="name">Name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of matching patient DTOs</returns>
    Task<IEnumerable<PatientDto>> SearchPatientsByNameAsync(string name, CancellationToken cancellationToken = default);
}
