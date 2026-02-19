using AutoMapper;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using DbModels = ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

public class AppointmentRepository : Repository<Appointment, DbModels.appointment>, IAppointmentRepository
{
    public AppointmentRepository(ClinicManagementDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    private IQueryable<DbModels.appointment> QueryWithIncludes()
    {
        return _dbSet.AsNoTracking()
            .Include(a => a.patient)
            .Include(a => a.doctor)
                .ThenInclude(d => d!.deptnoNavigation);
    }

    public override async Task<Appointment?> GetByIdAsync(int id)
    {
        var dbModel = await QueryWithIncludes()
            .FirstOrDefaultAsync(a => a.appointid == id);
        return dbModel != null ? _mapper.Map<Appointment>(dbModel) : null;
    }

    public override async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        var dbModels = await QueryWithIncludes().ToListAsync();
        return _mapper.Map<IEnumerable<Appointment>>(dbModels);
    }

    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId)
    {
        var dbModels = await QueryWithIncludes()
            .Where(a => a.patientid == patientId)
            .OrderByDescending(a => a.date)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Appointment>>(dbModels);
    }

    public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId)
    {
        var dbModels = await QueryWithIncludes()
            .Where(a => a.doctorid == doctorId)
            .OrderByDescending(a => a.date)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Appointment>>(dbModels);
    }

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(string status)
    {
        var statusInt = MapStatusToInt(status);
        var dbModels = await QueryWithIncludes()
            .Where(a => a.appointment_status == statusInt)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Appointment>>(dbModels);
    }

    public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(int patientId)
    {
        var dbModels = await QueryWithIncludes()
            .Where(a => a.patientid == patientId && a.date >= DateTime.Today)
            .OrderBy(a => a.date)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Appointment>>(dbModels);
    }

    public async Task<IEnumerable<Appointment>> GetPendingAppointmentsAsync(int doctorId)
    {
        var dbModels = await QueryWithIncludes()
            .Where(a => a.doctorid == doctorId && a.appointment_status == 0)
            .OrderBy(a => a.date)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Appointment>>(dbModels);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentHistoryAsync(int patientId)
    {
        var dbModels = await QueryWithIncludes()
            .Where(a => a.patientid == patientId && a.appointment_status == 2)
            .OrderByDescending(a => a.date)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Appointment>>(dbModels);
    }

    private static int MapStatusToInt(string status)
    {
        return status?.ToLower() switch
        {
            "pending" => 0,
            "approved" => 1,
            "completed" => 2,
            "canceled" or "cancelled" => 3,
            _ => 0
        };
    }
}
