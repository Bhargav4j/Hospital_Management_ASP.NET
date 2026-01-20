namespace HospitalManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for feedback business logic operations.
/// Provides methods for managing feedback data using DTOs for data transfer.
/// </summary>
public interface IFeedbackService
{
    /// <summary>
    /// Retrieves all feedback records.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of feedback DTOs.</returns>
    Task<IEnumerable<FeedbackDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a feedback record by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback record.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The feedback DTO if found; otherwise, null.</returns>
    Task<FeedbackDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new feedback record.
    /// </summary>
    /// <param name="dto">The feedback creation DTO containing feedback information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The created feedback DTO with generated identifier.</returns>
    Task<FeedbackDto> CreateAsync(FeedbackCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing feedback record.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback record to update.</param>
    /// <param name="dto">The feedback update DTO containing updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated feedback DTO.</returns>
    Task<FeedbackDto> UpdateAsync(int id, FeedbackUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a feedback record.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback record to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the feedback record was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for feedback records based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against feedback properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of feedback DTOs matching the search criteria.</returns>
    Task<IEnumerable<FeedbackDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

/// <summary>
/// Data Transfer Object for feedback information.
/// </summary>
public record FeedbackDto
{
    public int FeedbackId { get; init; }
    public int PatientId { get; init; }
    public int? DoctorId { get; init; }
    public int Rating { get; init; }
    public string? Comments { get; init; }
    public DateTime FeedbackDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// Data Transfer Object for creating a new feedback record.
/// </summary>
public record FeedbackCreateDto
{
    public int PatientId { get; init; }
    public int? DoctorId { get; init; }
    public int Rating { get; init; }
    public string? Comments { get; init; }
    public DateTime FeedbackDate { get; init; }
}

/// <summary>
/// Data Transfer Object for updating an existing feedback record.
/// </summary>
public record FeedbackUpdateDto
{
    public int PatientId { get; init; }
    public int? DoctorId { get; init; }
    public int Rating { get; init; }
    public string? Comments { get; init; }
    public DateTime FeedbackDate { get; init; }
}
