using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<AdminRepository> _logger;

    public AdminRepository(ClinicDbContext context, ILogger<AdminRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Admin>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Admins
                .AsNoTracking()
                .Where(a => a.IsActive)
                .OrderBy(a => a.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all admins");
            throw;
        }
    }

    public async Task<Admin?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Admins
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin with ID {AdminId}", id);
            throw;
        }
    }

    public async Task<Admin?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Admins
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email == email && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving admin with email {Email}", email);
            throw;
        }
    }

    public async Task<Admin> AddAsync(Admin admin, CancellationToken cancellationToken = default)
    {
        try
        {
            admin.CreatedDate = DateTime.UtcNow;
            admin.IsActive = true;
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Admin added successfully with ID {AdminId}", admin.Id);
            return admin;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding admin");
            throw;
        }
    }

    public async Task UpdateAsync(Admin admin, CancellationToken cancellationToken = default)
    {
        try
        {
            admin.ModifiedDate = DateTime.UtcNow;
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Admin updated successfully with ID {AdminId}", admin.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating admin with ID {AdminId}", admin.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var admin = await _context.Admins.FindAsync(new object[] { id }, cancellationToken);
            if (admin != null)
            {
                admin.IsActive = false;
                admin.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Admin soft-deleted successfully with ID {AdminId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting admin with ID {AdminId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Admins
                .AnyAsync(a => a.Id == id && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if admin exists with ID {AdminId}", id);
            throw;
        }
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Admins
                .AnyAsync(a => a.Email == email && a.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if email exists {Email}", email);
            throw;
        }
    }
}
