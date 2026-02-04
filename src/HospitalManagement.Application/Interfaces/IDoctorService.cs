using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

/// <summary>
/// Service interface for Doctor operations
/// </summary>
public interface IDoctorService
{
    /// <summary>
    /// Gets all doctors asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of doctor DTOs</returns>
    Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a doctor by ID asynchronously
    /// </summary>
    /// <param name="id">Doctor identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Doctor DTO if found, null otherwise</returns>
    Task<DoctorDto?> GetDoctorByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a doctor by email asynchronously
    /// </summary>
    /// <param name="email">Doctor email address</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Doctor DTO if found, null otherwise</returns>
    Task<DoctorDto?> GetDoctorByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets doctors by department asynchronously
    /// </summary>
    /// <param name="deptNo">Department number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of doctor DTOs</returns>
    Task<IEnumerable<DoctorDto>> GetDoctorsByDepartmentAsync(int deptNo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets doctors by specialization asynchronously
    /// </summary>
    /// <param name="specialization">Specialization</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of doctor DTOs</returns>
    Task<IEnumerable<DoctorDto>> GetDoctorsBySpecializationAsync(string specialization, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new doctor asynchronously
    /// </summary>
    /// <param name="doctorCreateDto">Doctor creation data</param>
    /// <param name="createdBy">User creating the record</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created doctor DTO</returns>
    Task<DoctorDto> CreateDoctorAsync(DoctorCreateDto doctorCreateDto, string createdBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing doctor asynchronously
    /// </summary>
    /// <param name="doctorUpdateDto">Doctor update data</param>
    /// <param name="modifiedBy">User modifying the record</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated doctor DTO if successful, null if not found</returns>
    Task<DoctorDto?> UpdateDoctorAsync(DoctorUpdateDto doctorUpdateDto, string modifiedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a doctor asynchronously
    /// </summary>
    /// <param name="id">Doctor identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteDoctorAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets active doctors asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of active doctor DTOs</returns>
    Task<IEnumerable<DoctorDto>> GetActiveDoctorsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates doctor reputation index asynchronously
    /// </summary>
    /// <param name="doctorId">Doctor identifier</param>
    /// <param name="newReputation">New reputation index</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if updated, false if not found</returns>
    Task<bool> UpdateDoctorReputationAsync(int doctorId, float newReputation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Increments the number of patients treated by a doctor asynchronously
    /// </summary>
    /// <param name="doctorId">Doctor identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if updated, false if not found</returns>
    Task<bool> IncrementPatientsTreatedAsync(int doctorId, CancellationToken cancellationToken = default);
}
