namespace HospitalManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for doctor business logic operations.
/// Provides methods for managing doctor data using DTOs for data transfer.
/// </summary>
public interface IDoctorService
{
    /// <summary>
    /// Retrieves all doctors.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of doctor DTOs.</returns>
    Task<IEnumerable<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a doctor by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The doctor DTO if found; otherwise, null.</returns>
    Task<DoctorDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new doctor.
    /// </summary>
    /// <param name="dto">The doctor creation DTO containing doctor information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The created doctor DTO with generated identifier.</returns>
    Task<DoctorDto> CreateAsync(DoctorCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing doctor.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor to update.</param>
    /// <param name="dto">The doctor update DTO containing updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated doctor DTO.</returns>
    Task<DoctorDto> UpdateAsync(int id, DoctorUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a doctor.
    /// </summary>
    /// <param name="id">The unique identifier of the doctor to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the doctor was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for doctors based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against doctor properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of doctor DTOs matching the search criteria.</returns>
    Task<IEnumerable<DoctorDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

/// <summary>
/// Data Transfer Object for doctor information.
/// </summary>
public record DoctorDto
{
    public int DoctorId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Specialization { get; init; }
    public required string Qualification { get; init; }
    public required string ContactNumber { get; init; }
    public required string Email { get; init; }
    public string? Department { get; init; }
    public int YearsOfExperience { get; init; }
    public TimeSpan ConsultationStartTime { get; init; }
    public TimeSpan ConsultationEndTime { get; init; }
    public decimal ConsultationFee { get; init; }
    public bool IsAvailable { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// Data Transfer Object for creating a new doctor.
/// </summary>
public record DoctorCreateDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Specialization { get; init; }
    public required string Qualification { get; init; }
    public required string ContactNumber { get; init; }
    public required string Email { get; init; }
    public string? Department { get; init; }
    public int YearsOfExperience { get; init; }
    public TimeSpan ConsultationStartTime { get; init; }
    public TimeSpan ConsultationEndTime { get; init; }
    public decimal ConsultationFee { get; init; }
    public bool IsAvailable { get; init; }
}

/// <summary>
/// Data Transfer Object for updating an existing doctor.
/// </summary>
public record DoctorUpdateDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Specialization { get; init; }
    public required string Qualification { get; init; }
    public required string ContactNumber { get; init; }
    public required string Email { get; init; }
    public string? Department { get; init; }
    public int YearsOfExperience { get; init; }
    public TimeSpan ConsultationStartTime { get; init; }
    public TimeSpan ConsultationEndTime { get; init; }
    public decimal ConsultationFee { get; init; }
    public bool IsAvailable { get; init; }
}
