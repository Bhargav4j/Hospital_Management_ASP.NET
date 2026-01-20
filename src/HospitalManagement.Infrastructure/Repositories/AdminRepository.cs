using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<AdminRepository> _logger;

    public AdminRepository(HospitalDbContext context, ILogger<AdminRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Admin>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Admins.Where(a => a.IsActive).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Admin?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Admins.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id && a.IsActive, cancellationToken);
    }

    public async Task<Admin> AddAsync(Admin admin, CancellationToken cancellationToken = default)
    {
        await _context.Admins.AddAsync(admin, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return admin;
    }

    public async Task<Admin> UpdateAsync(Admin admin, CancellationToken cancellationToken = default)
    {
        _context.Admins.Update(admin);
        await _context.SaveChangesAsync(cancellationToken);
        return admin;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Admins.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null) return false;
        entity.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Admins.AnyAsync(a => a.Id == id && a.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Admin>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Admins.Where(a => a.IsActive && (a.Name.Contains(searchTerm) || a.Email.Contains(searchTerm))).AsNoTracking().ToListAsync(cancellationToken);
    }
}
