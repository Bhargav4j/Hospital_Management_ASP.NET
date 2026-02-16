using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ClinicManagementDbContext context) : base(context)
    {
    }

    public override async Task<Appointment?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Department)
            .FirstOrDefaultAsync(a => a.AppointmentID == id);
    }

    public override async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Department)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Department)
            .Where(a => a.PatientID == patientId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId)
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Department)
            .Where(a => a.DoctorID == doctorId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(string status)
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Department)
            .Where(a => a.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(int patientId)
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Department)
            .Where(a => a.PatientID == patientId && a.AppointmentDate >= DateTime.Today)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetPendingAppointmentsAsync(int doctorId)
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Department)
            .Where(a => a.DoctorID == doctorId && a.Status == "Pending")
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentHistoryAsync(int patientId)
    {
        return await _dbSet
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .ThenInclude(d => d.Department)
            .Where(a => a.PatientID == patientId && a.Status == "Completed")
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();
    }
}
