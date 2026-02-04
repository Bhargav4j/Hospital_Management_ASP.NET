using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Staff entity
/// </summary>
public class StaffRepository : IStaffRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<StaffRepository> _logger;

    public StaffRepository(ClinicDbContext context, ILogger<StaffRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Staff>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all staff");
            return await _context.Staff
                .AsNoTracking()
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);
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
            _logger.LogInformation("Retrieving staff with ID {StaffId}", id);
            return await _context.Staff
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff with ID {StaffId}", id);
            throw;
        }
    }

    public async Task<Staff?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving staff with email {Email}", email);
            return await _context.Staff
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Email == email && s.IsActive, cancellationToken);
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
            _logger.LogInformation("Adding new staff {StaffName}", staff.Name);
            staff.CreatedDate = DateTime.UtcNow;
            staff.IsActive = true;

            await _context.Staff.AddAsync(staff, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added staff with ID {StaffId}", staff.Id);
            return staff;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding staff {StaffName}", staff.Name);
            throw;
        }
    }

    public async Task UpdateAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating staff with ID {StaffId}", staff.Id);
            staff.ModifiedDate = DateTime.UtcNow;

            _context.Staff.Update(staff);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated staff with ID {StaffId}", staff.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating staff with ID {StaffId}", staff.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting staff with ID {StaffId}", id);
            var staff = await _context.Staff.FindAsync(new object[] { id }, cancellationToken);

            if (staff != null)
            {
                staff.IsActive = false;
                staff.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully deleted staff with ID {StaffId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting staff with ID {StaffId}", id);
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
            _logger.LogError(ex, "Error checking existence of staff with ID {StaffId}", id);
            throw;
        }
    }
}
