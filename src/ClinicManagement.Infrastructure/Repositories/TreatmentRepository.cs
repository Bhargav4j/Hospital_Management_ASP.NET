using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

public class TreatmentRepository : ITreatmentRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<TreatmentRepository> _logger;

    public TreatmentRepository(ClinicDbContext context, ILogger<TreatmentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Treatment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Treatments
                .AsNoTracking()
                .Include(t => t.Appointment)
                .Where(t => t.IsActive)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all treatments");
            throw;
        }
    }

    public async Task<Treatment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Treatments
                .AsNoTracking()
                .Include(t => t.Appointment)
                .FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving treatment with ID {TreatmentId}", id);
            throw;
        }
    }

    public async Task<Treatment?> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Treatments
                .AsNoTracking()
                .Include(t => t.Appointment)
                .FirstOrDefaultAsync(t => t.AppointmentId == appointmentId && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving treatment for appointment {AppointmentId}", appointmentId);
            throw;
        }
    }

    public async Task<Treatment> AddAsync(Treatment treatment, CancellationToken cancellationToken = default)
    {
        try
        {
            treatment.CreatedDate = DateTime.UtcNow;
            treatment.IsActive = true;
            _context.Treatments.Add(treatment);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Treatment added successfully with ID {TreatmentId}", treatment.Id);
            return treatment;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding treatment");
            throw;
        }
    }

    public async Task UpdateAsync(Treatment treatment, CancellationToken cancellationToken = default)
    {
        try
        {
            treatment.ModifiedDate = DateTime.UtcNow;
            _context.Treatments.Update(treatment);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Treatment updated successfully with ID {TreatmentId}", treatment.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating treatment with ID {TreatmentId}", treatment.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var treatment = await _context.Treatments.FindAsync(new object[] { id }, cancellationToken);
            if (treatment != null)
            {
                treatment.IsActive = false;
                treatment.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Treatment soft-deleted successfully with ID {TreatmentId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting treatment with ID {TreatmentId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Treatments
                .AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if treatment exists with ID {TreatmentId}", id);
            throw;
        }
    }
}
