namespace HospitalManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for patient business logic operations.
/// Provides methods for managing patient data using DTOs for data transfer.
/// </summary>
public interface IPatientService
{
    /// <summary>
    /// Retrieves all patients.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of patient DTOs.</returns>
    Task<IEnumerable<PatientDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a patient by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the patient.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The patient DTO if found; otherwise, null.</returns>
    Task<PatientDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new patient.
    /// </summary>
    /// <param name="dto">The patient creation DTO containing patient information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The created patient DTO with generated identifier.</returns>
    Task<PatientDto> CreateAsync(PatientCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing patient.
    /// </summary>
    /// <param name="id">The unique identifier of the patient to update.</param>
    /// <param name="dto">The patient update DTO containing updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated patient DTO.</returns>
    Task<PatientDto> UpdateAsync(int id, PatientUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a patient.
    /// </summary>
    /// <param name="id">The unique identifier of the patient to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the patient was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for patients based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against patient properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of patient DTOs matching the search criteria.</returns>
    Task<IEnumerable<PatientDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

/// <summary>
/// Data Transfer Object for patient information.
/// </summary>
public record PatientDto
{
    public int PatientId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public required string Gender { get; init; }
    public required string ContactNumber { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
    public string? MedicalHistory { get; init; }
    public string? EmergencyContact { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// Data Transfer Object for creating a new patient.
/// </summary>
public record PatientCreateDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public required string Gender { get; init; }
    public required string ContactNumber { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
    public string? MedicalHistory { get; init; }
    public string? EmergencyContact { get; init; }
}

/// <summary>
/// Data Transfer Object for updating an existing patient.
/// </summary>
public record PatientUpdateDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public required string Gender { get; init; }
    public required string ContactNumber { get; init; }
    public required string Email { get; init; }
    public required string Address { get; init; }
    public string? MedicalHistory { get; init; }
    public string? EmergencyContact { get; init; }
}
