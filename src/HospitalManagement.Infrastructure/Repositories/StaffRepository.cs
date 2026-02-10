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
        try
        {
            return await _context.Staff.Where(s => s.IsActive).AsNoTracking().ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all staff");
            throw;
        }
    }

    public async Task<Staff?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id && s.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff with id {Id}", id);
            throw;
        }
    }

    public async Task<Staff?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.Email == email && s.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff with email {Email}", email);
            throw;
        }
    }

    public async Task<Staff> AddAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Staff.AddAsync(staff, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return staff;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding staff");
            throw;
        }
    }

    public async Task UpdateAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Staff.Update(staff);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating staff with id {Id}", staff.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var staff = await _context.Staff.FindAsync(new object[] { id }, cancellationToken);
            if (staff != null)
            {
                staff.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting staff with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Staff.AnyAsync(s => s.Id == id && s.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if staff exists with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Staff>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Staff
                .Where(s => s.IsActive && (s.Name.Contains(searchTerm) || s.Email.Contains(searchTerm)))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching staff with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
