namespace HospitalManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for treatment history business logic operations.
/// Provides methods for managing treatment history data using DTOs for data transfer.
/// </summary>
public interface ITreatmentHistoryService
{
    /// <summary>
    /// Retrieves all treatment history records.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of treatment history DTOs.</returns>
    Task<IEnumerable<TreatmentHistoryDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a treatment history record by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the treatment history record.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The treatment history DTO if found; otherwise, null.</returns>
    Task<TreatmentHistoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new treatment history record.
    /// </summary>
    /// <param name="dto">The treatment history creation DTO containing treatment information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The created treatment history DTO with generated identifier.</returns>
    Task<TreatmentHistoryDto> CreateAsync(TreatmentHistoryCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing treatment history record.
    /// </summary>
    /// <param name="id">The unique identifier of the treatment history record to update.</param>
    /// <param name="dto">The treatment history update DTO containing updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated treatment history DTO.</returns>
    Task<TreatmentHistoryDto> UpdateAsync(int id, TreatmentHistoryUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a treatment history record.
    /// </summary>
    /// <param name="id">The unique identifier of the treatment history record to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the treatment history record was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for treatment history records based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against treatment history properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of treatment history DTOs matching the search criteria.</returns>
    Task<IEnumerable<TreatmentHistoryDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

/// <summary>
/// Data Transfer Object for treatment history information.
/// </summary>
public record TreatmentHistoryDto
{
    public int TreatmentId { get; init; }
    public int PatientId { get; init; }
    public int DoctorId { get; init; }
    public DateTime TreatmentDate { get; init; }
    public required string Diagnosis { get; init; }
    public required string Treatment { get; init; }
    public string? Prescription { get; init; }
    public string? LabTests { get; init; }
    public string? FollowUpInstructions { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// Data Transfer Object for creating a new treatment history record.
/// </summary>
public record TreatmentHistoryCreateDto
{
    public int PatientId { get; init; }
    public int DoctorId { get; init; }
    public DateTime TreatmentDate { get; init; }
    public required string Diagnosis { get; init; }
    public required string Treatment { get; init; }
    public string? Prescription { get; init; }
    public string? LabTests { get; init; }
    public string? FollowUpInstructions { get; init; }
}

/// <summary>
/// Data Transfer Object for updating an existing treatment history record.
/// </summary>
public record TreatmentHistoryUpdateDto
{
    public int PatientId { get; init; }
    public int DoctorId { get; init; }
    public DateTime TreatmentDate { get; init; }
    public required string Diagnosis { get; init; }
    public required string Treatment { get; init; }
    public string? Prescription { get; init; }
    public string? LabTests { get; init; }
    public string? FollowUpInstructions { get; init; }
}
