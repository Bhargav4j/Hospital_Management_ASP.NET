namespace HospitalManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for appointment business logic operations.
/// Provides methods for managing appointment data using DTOs for data transfer.
/// </summary>
public interface IAppointmentService
{
    /// <summary>
    /// Retrieves all appointments.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of appointment DTOs.</returns>
    Task<IEnumerable<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an appointment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The appointment DTO if found; otherwise, null.</returns>
    Task<AppointmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new appointment.
    /// </summary>
    /// <param name="dto">The appointment creation DTO containing appointment information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The created appointment DTO with generated identifier.</returns>
    Task<AppointmentDto> CreateAsync(AppointmentCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing appointment.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to update.</param>
    /// <param name="dto">The appointment update DTO containing updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated appointment DTO.</returns>
    Task<AppointmentDto> UpdateAsync(int id, AppointmentUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an appointment.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the appointment was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for appointments based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against appointment properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of appointment DTOs matching the search criteria.</returns>
    Task<IEnumerable<AppointmentDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

/// <summary>
/// Data Transfer Object for appointment information.
/// </summary>
public record AppointmentDto
{
    public int AppointmentId { get; init; }
    public int PatientId { get; init; }
    public int DoctorId { get; init; }
    public DateTime AppointmentDate { get; init; }
    public TimeSpan AppointmentTime { get; init; }
    public required string Status { get; init; }
    public string? Reason { get; init; }
    public string? Notes { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// Data Transfer Object for creating a new appointment.
/// </summary>
public record AppointmentCreateDto
{
    public int PatientId { get; init; }
    public int DoctorId { get; init; }
    public DateTime AppointmentDate { get; init; }
    public TimeSpan AppointmentTime { get; init; }
    public required string Status { get; init; }
    public string? Reason { get; init; }
    public string? Notes { get; init; }
}

/// <summary>
/// Data Transfer Object for updating an existing appointment.
/// </summary>
public record AppointmentUpdateDto
{
    public int PatientId { get; init; }
    public int DoctorId { get; init; }
    public DateTime AppointmentDate { get; init; }
    public TimeSpan AppointmentTime { get; init; }
    public required string Status { get; init; }
    public string? Reason { get; init; }
    public string? Notes { get; init; }
}
