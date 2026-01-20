using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<PatientRepository> _logger;

    public PatientRepository(HospitalDbContext context, ILogger<PatientRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Patients
                .Where(p => p.IsActive)
                .AsNoTracking()
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
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient with ID {PatientId}", id);
            throw;
        }
    }

    public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Patients.AddAsync(patient, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return patient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding patient");
            throw;
        }
    }

    public async Task<Patient> UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync(cancellationToken);
            return patient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient with ID {PatientId}", patient.Id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var patient = await _context.Patients.FindAsync(new object[] { id }, cancellationToken);
            if (patient == null) return false;

            patient.IsActive = false;
            patient.ModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient with ID {PatientId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Patients.AnyAsync(p => p.Id == id && p.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Patients
                .Where(p => p.IsActive &&
                    (p.Name.Contains(searchTerm) ||
                     p.Email.Contains(searchTerm) ||
                     p.PhoneNumber.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching patients with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
