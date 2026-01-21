using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<DoctorRepository> _logger;

    public DoctorRepository(HospitalDbContext context, ILogger<DoctorRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
            .Include(d => d.Department)
            .AsNoTracking()
            .Where(d => d.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
            .Include(d => d.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.DoctorID == id && d.IsActive, cancellationToken);
    }

    public async Task<Doctor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Email == email && d.IsActive, cancellationToken);
    }

    public async Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync(cancellationToken);
        return doctor;
    }

    public async Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var doctor = await _context.Doctors.FindAsync(new object[] { id }, cancellationToken);
        if (doctor != null)
        {
            doctor.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.AnyAsync(d => d.DoctorID == id && d.IsActive, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors.AnyAsync(d => d.Email == email && d.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Doctor>> SearchAsync(string searchQuery, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
            .Include(d => d.Department)
            .AsNoTracking()
            .Where(d => d.IsActive && d.Name.Contains(searchQuery))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Doctor>> GetByDepartmentAsync(int deptNo, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
            .Include(d => d.Department)
            .AsNoTracking()
            .Where(d => d.DeptNo == deptNo && d.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<(Doctor? doctor, int userType)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Email == email && d.IsActive, cancellationToken);

        if (doctor == null)
        {
            return (null, 0);
        }

        if (doctor.Password != password)
        {
            return (null, -1);
        }

        return (doctor, 2); // UserType.Doctor = 2
    }
}
