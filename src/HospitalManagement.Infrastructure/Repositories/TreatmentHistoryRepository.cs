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
        return await _context.TreatmentHistories.Where(t => t.IsActive).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<TreatmentHistory?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TreatmentHistories.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id && t.IsActive, cancellationToken);
    }

    public async Task<TreatmentHistory> AddAsync(TreatmentHistory treatmentHistory, CancellationToken cancellationToken = default)
    {
        await _context.TreatmentHistories.AddAsync(treatmentHistory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return treatmentHistory;
    }

    public async Task<TreatmentHistory> UpdateAsync(TreatmentHistory treatmentHistory, CancellationToken cancellationToken = default)
    {
        _context.TreatmentHistories.Update(treatmentHistory);
        await _context.SaveChangesAsync(cancellationToken);
        return treatmentHistory;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.TreatmentHistories.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null) return false;
        entity.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TreatmentHistories.AnyAsync(t => t.Id == id && t.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<TreatmentHistory>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.TreatmentHistories.Where(t => t.IsActive && t.Diagnosis.Contains(searchTerm)).AsNoTracking().ToListAsync(cancellationToken);
    }
}
