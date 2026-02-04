using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Doctor entity
/// </summary>
public class DoctorRepository : IDoctorRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<DoctorRepository> _logger;

    public DoctorRepository(ClinicDbContext context, ILogger<DoctorRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all doctors");
            return await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Clinic)
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
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
            _logger.LogInformation("Retrieving doctor with ID {DoctorId}", id);
            return await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Clinic)
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor with ID {DoctorId}", id);
            throw;
        }
    }

    public async Task<Doctor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctor with email {Email}", email);
            return await _context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Email == email && d.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor with email {Email}", email);
            throw;
        }
    }

    public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctors with specialization {Specialization}", specialization);
            return await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Clinic)
                .Where(d => d.IsActive && d.Specialization == specialization)
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctors with specialization {Specialization}", specialization);
            throw;
        }
    }

    public async Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new doctor {DoctorName}", doctor.Name);
            doctor.CreatedDate = DateTime.UtcNow;
            doctor.IsActive = true;

            await _context.Doctors.AddAsync(doctor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added doctor with ID {DoctorId}", doctor.Id);
            return doctor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding doctor {DoctorName}", doctor.Name);
            throw;
        }
    }

    public async Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating doctor with ID {DoctorId}", doctor.Id);
            doctor.ModifiedDate = DateTime.UtcNow;

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated doctor with ID {DoctorId}", doctor.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating doctor with ID {DoctorId}", doctor.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting doctor with ID {DoctorId}", id);
            var doctor = await _context.Doctors.FindAsync(new object[] { id }, cancellationToken);

            if (doctor != null)
            {
                doctor.IsActive = false;
                doctor.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully deleted doctor with ID {DoctorId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting doctor with ID {DoctorId}", id);
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
            _logger.LogError(ex, "Error checking existence of doctor with ID {DoctorId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching doctors with term {SearchTerm}", searchTerm);
            return await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Clinic)
                .Where(d => d.IsActive &&
                    (d.Name.Contains(searchTerm) ||
                     d.Email.Contains(searchTerm) ||
                     d.Specialization.Contains(searchTerm)))
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching doctors with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
