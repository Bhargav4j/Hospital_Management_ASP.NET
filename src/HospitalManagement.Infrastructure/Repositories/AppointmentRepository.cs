using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<AppointmentRepository> _logger;

    public AppointmentRepository(HospitalDbContext context, ILogger<AppointmentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.TimeSlot)
            .AsNoTracking()
            .Where(a => a.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.TimeSlot)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AppointmentID == id && a.IsActive, cancellationToken);
    }

    public async Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);
        return appointment;
    }

    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments.FindAsync(new object[] { id }, cancellationToken);
        if (appointment != null)
        {
            appointment.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments.AnyAsync(a => a.AppointmentID == id && a.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.TimeSlot)
            .AsNoTracking()
            .Where(a => a.PatientID == patientId && a.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.TimeSlot)
            .AsNoTracking()
            .Where(a => a.DoctorID == doctorId && a.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetPendingByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.TimeSlot)
            .AsNoTracking()
            .Where(a => a.DoctorID == doctorId && a.Status == AppointmentStatus.Pending && a.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Appointment>> GetTodaysByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.TimeSlot)
            .AsNoTracking()
            .Where(a => a.DoctorID == doctorId && a.AppointmentDate.Date == today && a.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> GetCurrentAppointmentByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var today = DateTime.Today;
        return await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.TimeSlot)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.PatientID == patientId && a.AppointmentDate.Date == today && a.Status == AppointmentStatus.Approved && a.IsActive, cancellationToken);
    }

    public async Task ApproveAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments.FindAsync(new object[] { appointmentId }, cancellationToken);
        if (appointment != null)
        {
            appointment.Status = AppointmentStatus.Approved;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<Appointment>> GetFreeSlotsByDoctorIdAsync(int doctorId, int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments
            .Include(a => a.TimeSlot)
            .AsNoTracking()
            .Where(a => a.DoctorID == doctorId && a.IsActive)
            .ToListAsync(cancellationToken);
    }
}
