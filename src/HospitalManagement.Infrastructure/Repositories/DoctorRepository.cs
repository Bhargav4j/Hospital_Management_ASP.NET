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
            return await _context.Doctors
                .Include(d => d.User)
                .Where(d => d.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
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
            return await _context.Doctors
                .Include(d => d.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor {DoctorId}", id);
            throw;
        }
    }

    public async Task<Doctor?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Doctors
                .Include(d => d.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == userId && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor by user id {UserId}", userId);
            throw;
        }
    }

    public async Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Doctor created: {DoctorId}", doctor.Id);
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
            _logger.LogInformation("Doctor updated: {DoctorId}", doctor.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating doctor {DoctorId}", doctor.Id);
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
                doctor.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Doctor deleted: {DoctorId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting doctor {DoctorId}", id);
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
            _logger.LogError(ex, "Error checking if doctor exists {DoctorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Where(d => d.IsActive && (d.Name.Contains(searchTerm) || (d.Specialization != null && d.Specialization.Contains(searchTerm))))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching doctors with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
