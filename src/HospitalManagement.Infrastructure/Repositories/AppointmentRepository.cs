using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Appointment entity
/// </summary>
public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AppointmentRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentRepository"/> class
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="logger">Logger instance</param>
    public AppointmentRepository(ApplicationDbContext context, ILogger<AppointmentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all active appointments
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of appointments</returns>
    public async Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all active appointments");

            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.FreeSlot)
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} appointments", appointments.Count);
            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all appointments");
            throw;
        }
    }

    /// <summary>
    /// Gets an appointment by ID
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Appointment entity or null if not found</returns>
    public async Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointment with ID: {AppointmentId}", id);

            var appointment = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.FreeSlot)
                .FirstOrDefaultAsync(a => a.AppointmentID == id && a.IsActive, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment with ID {AppointmentId} not found", id);
            }

            return appointment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointment with ID: {AppointmentId}", id);
            throw;
        }
    }

    /// <summary>
    /// Adds a new appointment
    /// </summary>
    /// <param name="appointment">Appointment entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Added appointment entity</returns>
    public async Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new appointment for patient ID: {PatientId}", appointment.PatientID);

            await _context.Appointments.AddAsync(appointment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added appointment with ID: {AppointmentId}", appointment.AppointmentID);
            return appointment;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding appointment");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding appointment");
            throw;
        }
    }

    /// <summary>
    /// Updates an existing appointment
    /// </summary>
    /// <param name="appointment">Appointment entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating appointment with ID: {AppointmentId}", appointment.AppointmentID);

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated appointment with ID: {AppointmentId}", appointment.AppointmentID);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating appointment with ID: {AppointmentId}",
                appointment.AppointmentID);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating appointment with ID: {AppointmentId}",
                appointment.AppointmentID);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating appointment with ID: {AppointmentId}", appointment.AppointmentID);
            throw;
        }
    }

    /// <summary>
    /// Deletes an appointment (soft delete by setting IsActive to false)
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting appointment with ID: {AppointmentId}", id);

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentID == id, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment with ID {AppointmentId} not found for deletion", id);
                throw new InvalidOperationException($"Appointment with ID {id} not found");
            }

            appointment.IsActive = false;
            appointment.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted appointment with ID: {AppointmentId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting appointment with ID: {AppointmentId}", id);
            throw;
        }
    }

    /// <summary>
    /// Checks if an appointment exists
    /// </summary>
    /// <param name="id">Appointment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if appointment exists, otherwise false</returns>
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking if appointment exists with ID: {AppointmentId}", id);

            var exists = await _context.Appointments
                .AsNoTracking()
                .AnyAsync(a => a.AppointmentID == id && a.IsActive, cancellationToken);

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking appointment existence with ID: {AppointmentId}", id);
            throw;
        }
    }

    /// <summary>
    /// Gets appointments by patient ID
    /// </summary>
    /// <param name="patientId">Patient ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of appointments for the patient</returns>
    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for patient ID: {PatientId}", patientId);

            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.FreeSlot)
                .Where(a => a.PatientID == patientId && a.IsActive)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} appointments for patient ID: {PatientId}",
                appointments.Count, patientId);

            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments for patient ID: {PatientId}", patientId);
            throw;
        }
    }

    /// <summary>
    /// Gets appointments by doctor ID
    /// </summary>
    /// <param name="doctorId">Doctor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of appointments for the doctor</returns>
    public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for doctor ID: {DoctorId}", doctorId);

            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.FreeSlot)
                .Where(a => a.DoctorID == doctorId && a.IsActive)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} appointments for doctor ID: {DoctorId}",
                appointments.Count, doctorId);

            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments for doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }

    /// <summary>
    /// Gets pending appointments by doctor ID
    /// </summary>
    /// <param name="doctorId">Doctor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of pending appointments for the doctor</returns>
    public async Task<IEnumerable<Appointment>> GetPendingByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving pending appointments for doctor ID: {DoctorId}", doctorId);

            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.FreeSlot)
                .Where(a => a.DoctorID == doctorId &&
                           a.Status == "Pending" &&
                           a.IsActive)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} pending appointments for doctor ID: {DoctorId}",
                appointments.Count, doctorId);

            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending appointments for doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }

    /// <summary>
    /// Gets today's appointments by doctor ID
    /// </summary>
    /// <param name="doctorId">Doctor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of today's appointments for the doctor</returns>
    public async Task<IEnumerable<Appointment>> GetTodayByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving today's appointments for doctor ID: {DoctorId}", doctorId);

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.FreeSlot)
                .Where(a => a.DoctorID == doctorId &&
                           a.AppointmentDate >= today &&
                           a.AppointmentDate < tomorrow &&
                           a.IsActive)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} appointments for doctor ID: {DoctorId} today",
                appointments.Count, doctorId);

            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving today's appointments for doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }

    /// <summary>
    /// Gets current appointment by patient ID
    /// </summary>
    /// <param name="patientId">Patient ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Current appointment for the patient or null if not found</returns>
    public async Task<Appointment?> GetCurrentByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving current appointment for patient ID: {PatientId}", patientId);

            var appointment = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.FreeSlot)
                .Where(a => a.PatientID == patientId &&
                           a.Status != "Completed" &&
                           a.IsActive)
                .OrderByDescending(a => a.AppointmentDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (appointment == null)
            {
                _logger.LogInformation("No current appointment found for patient ID: {PatientId}", patientId);
            }

            return appointment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current appointment for patient ID: {PatientId}", patientId);
            throw;
        }
    }

    /// <summary>
    /// Gets treatment history for a patient
    /// </summary>
    /// <param name="patientId">Patient ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of completed appointments with treatment details</returns>
    public async Task<IEnumerable<Appointment>> GetTreatmentHistoryAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving treatment history for patient ID: {PatientId}", patientId);

            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.FreeSlot)
                .Where(a => a.PatientID == patientId &&
                           a.Status == "Completed" &&
                           a.IsActive)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} treatment records for patient ID: {PatientId}",
                appointments.Count, patientId);

            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving treatment history for patient ID: {PatientId}", patientId);
            throw;
        }
    }

    /// <summary>
    /// Gets bill history for a patient
    /// </summary>
    /// <param name="patientId">Patient ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of appointments with billing information</returns>
    public async Task<IEnumerable<Appointment>> GetBillHistoryAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving bill history for patient ID: {PatientId}", patientId);

            var appointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(a => a.FreeSlot)
                .Where(a => a.PatientID == patientId &&
                           a.Status == "Completed" &&
                           a.IsActive)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} billing records for patient ID: {PatientId}",
                appointments.Count, patientId);

            return appointments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bill history for patient ID: {PatientId}", patientId);
            throw;
        }
    }
}
