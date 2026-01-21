using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Patient entity
/// </summary>
public class PatientRepository : IPatientRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<PatientRepository> _logger;

    public PatientRepository(HospitalDbContext context, ILogger<PatientRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Patients
                .AsNoTracking()
                .Where(p => p.IsActive)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all patients");
            throw;
        }
    }

    public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PatientID == id && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient with ID {PatientId}", id);
            throw;
        }
    }

    public async Task<Patient?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email == email && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient with email {Email}", email);
            throw;
        }
    }

    public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Patient {PatientId} created successfully", patient.PatientID);
            return patient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding patient");
            throw;
        }
    }

    public async Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Patient {PatientId} updated successfully", patient.PatientID);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient {PatientId}", patient.PatientID);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var patient = await _context.Patients.FindAsync(new object[] { id }, cancellationToken);
            if (patient != null)
            {
                patient.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Patient {PatientId} deleted successfully", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient {PatientId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Patients.AnyAsync(p => p.PatientID == id && p.IsActive, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Patients.AnyAsync(p => p.Email == email && p.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Patient>> SearchAsync(string searchQuery, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Patients
                .AsNoTracking()
                .Where(p => p.IsActive && p.Name.Contains(searchQuery))
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching patients with query {SearchQuery}", searchQuery);
            throw;
        }
    }

    public async Task<(Patient? patient, int userType)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Email == email && p.IsActive, cancellationToken);

            if (patient == null)
            {
                return (null, 0);
            }

            if (patient.Password != password)
            {
                return (null, -1);
            }

            return (patient, 1); // UserType.Patient = 1
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating login for email {Email}", email);
            throw;
        }
    }
}
