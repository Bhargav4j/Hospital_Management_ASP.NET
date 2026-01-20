using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class BillRepository : IBillRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<BillRepository> _logger;

    public BillRepository(HospitalDbContext context, ILogger<BillRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Bills.Where(b => b.IsActive).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Bill?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Bills.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id && b.IsActive, cancellationToken);
    }

    public async Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        await _context.Bills.AddAsync(bill, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return bill;
    }

    public async Task<Bill> UpdateAsync(Bill bill, CancellationToken cancellationToken = default)
    {
        _context.Bills.Update(bill);
        await _context.SaveChangesAsync(cancellationToken);
        return bill;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Bills.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null) return false;
        entity.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Bills.AnyAsync(b => b.Id == id && b.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Bill>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Bills.Where(b => b.IsActive).AsNoTracking().ToListAsync(cancellationToken);
    }
}
