using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

/// <summary>
/// Service interface for Appointment operations
/// </summary>
public interface IAppointmentService
{
    /// <summary>
    /// Gets all appointments asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of appointment DTOs</returns>
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an appointment by ID asynchronously
    /// </summary>
    /// <param name="id">Appointment identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Appointment DTO if found, null otherwise</returns>
    Task<AppointmentDto?> GetAppointmentByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets appointments by patient ID asynchronously
    /// </summary>
    /// <param name="patientId">Patient identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of appointment DTOs</returns>
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets appointments by doctor ID asynchronously
    /// </summary>
    /// <param name="doctorId">Doctor identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of appointment DTOs</returns>
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets appointments by date asynchronously
    /// </summary>
    /// <param name="date">Appointment date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of appointment DTOs</returns>
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateAsync(DateTime date, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets appointments by status asynchronously
    /// </summary>
    /// <param name="status">Appointment status</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of appointment DTOs</returns>
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByStatusAsync(string status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new appointment asynchronously
    /// </summary>
    /// <param name="appointmentCreateDto">Appointment creation data</param>
    /// <param name="createdBy">User creating the record</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created appointment DTO</returns>
    Task<AppointmentDto> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto, string createdBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing appointment asynchronously
    /// </summary>
    /// <param name="appointmentUpdateDto">Appointment update data</param>
    /// <param name="modifiedBy">User modifying the record</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated appointment DTO if successful, null if not found</returns>
    Task<AppointmentDto?> UpdateAppointmentAsync(AppointmentUpdateDto appointmentUpdateDto, string modifiedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an appointment asynchronously
    /// </summary>
    /// <param name="id">Appointment identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteAppointmentAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels an appointment asynchronously
    /// </summary>
    /// <param name="id">Appointment identifier</param>
    /// <param name="modifiedBy">User cancelling the appointment</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if cancelled, false if not found</returns>
    Task<bool> CancelAppointmentAsync(int id, string modifiedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks an appointment as paid asynchronously
    /// </summary>
    /// <param name="id">Appointment identifier</param>
    /// <param name="modifiedBy">User marking payment</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if updated, false if not found</returns>
    Task<bool> MarkAppointmentAsPaidAsync(int id, string modifiedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets upcoming appointments for a patient asynchronously
    /// </summary>
    /// <param name="patientId">Patient identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of upcoming appointment DTOs</returns>
    Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsByPatientAsync(int patientId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets upcoming appointments for a doctor asynchronously
    /// </summary>
    /// <param name="doctorId">Doctor identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of upcoming appointment DTOs</returns>
    Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsByDoctorAsync(int doctorId, CancellationToken cancellationToken = default);
}
