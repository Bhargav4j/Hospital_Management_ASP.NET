using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Clinic entity
/// </summary>
public class ClinicRepository : IClinicRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<ClinicRepository> _logger;

    public ClinicRepository(ClinicDbContext context, ILogger<ClinicRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Clinic>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all clinics");
            return await _context.Clinics
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all clinics");
            throw;
        }
    }

    public async Task<Clinic?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving clinic with ID {ClinicId}", id);
            return await _context.Clinics
                .AsNoTracking()
                .Include(c => c.Doctors)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving clinic with ID {ClinicId}", id);
            throw;
        }
    }

    public async Task<Clinic> AddAsync(Clinic clinic, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new clinic {ClinicName}", clinic.Name);
            clinic.CreatedDate = DateTime.UtcNow;
            clinic.IsActive = true;

            await _context.Clinics.AddAsync(clinic, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added clinic with ID {ClinicId}", clinic.Id);
            return clinic;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding clinic {ClinicName}", clinic.Name);
            throw;
        }
    }

    public async Task UpdateAsync(Clinic clinic, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating clinic with ID {ClinicId}", clinic.Id);
            clinic.ModifiedDate = DateTime.UtcNow;

            _context.Clinics.Update(clinic);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated clinic with ID {ClinicId}", clinic.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating clinic with ID {ClinicId}", clinic.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting clinic with ID {ClinicId}", id);
            var clinic = await _context.Clinics.FindAsync(new object[] { id }, cancellationToken);

            if (clinic != null)
            {
                clinic.IsActive = false;
                clinic.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully deleted clinic with ID {ClinicId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting clinic with ID {ClinicId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Clinics.AnyAsync(c => c.Id == id && c.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of clinic with ID {ClinicId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Clinic>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching clinics with term {SearchTerm}", searchTerm);
            return await _context.Clinics
                .AsNoTracking()
                .Where(c => c.IsActive &&
                    (c.Name.Contains(searchTerm) ||
                     c.Address.Contains(searchTerm)))
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching clinics with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
