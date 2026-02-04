using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Appointment entity
/// </summary>
public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<AppointmentRepository> _logger;

    public AppointmentRepository(ClinicDbContext context, ILogger<AppointmentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all appointments");
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all appointments");
            throw;
        }
    }

    public async Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointment with ID {AppointmentId}", id);
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointment with ID {AppointmentId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for patient {PatientId}", patientId);
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId && a.IsActive)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments for patient {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for doctor {DoctorId}", doctorId);
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId && a.IsActive)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments for doctor {DoctorId}", doctorId);
            throw;
        }
    }

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments with status {Status}", status);
            return await _context.Appointments
                .AsNoTracking()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => a.Status == status && a.IsActive)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments with status {Status}", status);
            throw;
        }
    }

    public async Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new appointment for patient {PatientId}", appointment.PatientId);
            appointment.CreatedDate = DateTime.UtcNow;
            appointment.IsActive = true;

            await _context.Appointments.AddAsync(appointment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added appointment with ID {AppointmentId}", appointment.Id);
            return appointment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding appointment for patient {PatientId}", appointment.PatientId);
            throw;
        }
    }

    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating appointment with ID {AppointmentId}", appointment.Id);
            appointment.ModifiedDate = DateTime.UtcNow;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated appointment with ID {AppointmentId}", appointment.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating appointment with ID {AppointmentId}", appointment.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting appointment with ID {AppointmentId}", id);
            var appointment = await _context.Appointments.FindAsync(new object[] { id }, cancellationToken);

            if (appointment != null)
            {
                appointment.IsActive = false;
                appointment.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully deleted appointment with ID {AppointmentId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting appointment with ID {AppointmentId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Appointments.AnyAsync(a => a.Id == id && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of appointment with ID {AppointmentId}", id);
            throw;
        }
    }
}
