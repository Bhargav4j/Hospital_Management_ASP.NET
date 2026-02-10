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
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Doctors.Where(d => d.IsActive).AsNoTracking().ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all doctors");
            throw;
        }
    }

    public async Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor with id {Id}", id);
            throw;
        }
    }

    public async Task<Doctor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.Email == email && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor with email {Email}", email);
            throw;
        }
    }

    public async Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Doctors.AddAsync(doctor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return doctor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding doctor");
            throw;
        }
    }

    public async Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating doctor with id {Id}", doctor.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var doctor = await _context.Doctors.FindAsync(new object[] { id }, cancellationToken);
            if (doctor != null)
            {
                doctor.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting doctor with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Doctors.AnyAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if doctor exists with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Doctors
                .Where(d => d.IsActive && (d.Name.Contains(searchTerm) || d.Specialization.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching doctors with term {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Doctors
                .Where(d => d.IsActive && d.Specialization == specialization)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctors by specialization {Specialization}", specialization);
            throw;
        }
    }
}
