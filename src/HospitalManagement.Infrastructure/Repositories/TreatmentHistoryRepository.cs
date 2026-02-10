using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class TreatmentHistoryRepository : ITreatmentHistoryRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<TreatmentHistoryRepository> _logger;

    public TreatmentHistoryRepository(HospitalDbContext context, ILogger<TreatmentHistoryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TreatmentHistory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TreatmentHistories.Where(t => t.IsActive).AsNoTracking().ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all treatment histories");
            throw;
        }
    }

    public async Task<TreatmentHistory?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TreatmentHistories.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving treatment history with id {Id}", id);
            throw;
        }
    }

    public async Task<TreatmentHistory> AddAsync(TreatmentHistory treatmentHistory, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.TreatmentHistories.AddAsync(treatmentHistory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return treatmentHistory;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding treatment history");
            throw;
        }
    }

    public async Task UpdateAsync(TreatmentHistory treatmentHistory, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.TreatmentHistories.Update(treatmentHistory);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating treatment history with id {Id}", treatmentHistory.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var treatmentHistory = await _context.TreatmentHistories.FindAsync(new object[] { id }, cancellationToken);
            if (treatmentHistory != null)
            {
                treatmentHistory.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting treatment history with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TreatmentHistories.AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if treatment history exists with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TreatmentHistory>> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.TreatmentHistories
                .Where(t => t.AppointmentId == appointmentId && t.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving treatment histories for appointment {AppointmentId}", appointmentId);
            throw;
        }
    }
}
