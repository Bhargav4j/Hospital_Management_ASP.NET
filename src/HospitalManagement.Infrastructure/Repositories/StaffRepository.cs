using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<StaffRepository> _logger;

    public StaffRepository(HospitalDbContext context, ILogger<StaffRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Staff>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Staff.AsNoTracking().Where(s => s.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<Staff?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Staff.FirstOrDefaultAsync(s => s.StaffID == id && s.IsActive, cancellationToken);
    }

    public async Task<Staff> AddAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        _context.Staff.Add(staff);
        await _context.SaveChangesAsync(cancellationToken);
        return staff;
    }

    public async Task UpdateAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        _context.Staff.Update(staff);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var staff = await _context.Staff.FindAsync(new object[] { id }, cancellationToken);
        if (staff != null)
        {
            staff.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Staff.AnyAsync(s => s.StaffID == id && s.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Staff>> SearchAsync(string searchQuery, CancellationToken cancellationToken = default)
    {
        return await _context.Staff
            .AsNoTracking()
            .Where(s => s.IsActive && s.Name.Contains(searchQuery))
            .ToListAsync(cancellationToken);
    }
}
