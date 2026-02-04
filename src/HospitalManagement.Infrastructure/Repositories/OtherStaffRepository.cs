using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for OtherStaff entity
/// </summary>
public class OtherStaffRepository : IOtherStaffRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OtherStaffRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OtherStaffRepository"/> class
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="logger">Logger instance</param>
    public OtherStaffRepository(ApplicationDbContext context, ILogger<OtherStaffRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all active staff members
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of staff members</returns>
    public async Task<IEnumerable<OtherStaff>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all active staff members");

            var staff = await _context.OtherStaff
                .AsNoTracking()
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} staff members", staff.Count);
            return staff;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all staff members");
            throw;
        }
    }

    /// <summary>
    /// Gets a staff member by ID
    /// </summary>
    /// <param name="id">Staff ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Staff entity or null if not found</returns>
    public async Task<OtherStaff?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving staff member with ID: {StaffId}", id);

            var staff = await _context.OtherStaff
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StaffID == id && s.IsActive, cancellationToken);

            if (staff == null)
            {
                _logger.LogWarning("Staff member with ID {StaffId} not found", id);
            }

            return staff;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff member with ID: {StaffId}", id);
            throw;
        }
    }

    /// <summary>
    /// Adds a new staff member
    /// </summary>
    /// <param name="staff">Staff entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Added staff entity</returns>
    public async Task<OtherStaff> AddAsync(OtherStaff staff, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new staff member: {StaffName}", staff.Name);

            await _context.OtherStaff.AddAsync(staff, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added staff member with ID: {StaffId}", staff.StaffID);
            return staff;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding staff member: {StaffName}", staff.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding staff member: {StaffName}", staff.Name);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing staff member
    /// </summary>
    /// <param name="staff">Staff entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task UpdateAsync(OtherStaff staff, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating staff member with ID: {StaffId}", staff.StaffID);

            _context.OtherStaff.Update(staff);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated staff member with ID: {StaffId}", staff.StaffID);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating staff member with ID: {StaffId}", staff.StaffID);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating staff member with ID: {StaffId}", staff.StaffID);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating staff member with ID: {StaffId}", staff.StaffID);
            throw;
        }
    }

    /// <summary>
    /// Deletes a staff member (soft delete by setting IsActive to false)
    /// </summary>
    /// <param name="id">Staff ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting staff member with ID: {StaffId}", id);

            var staff = await _context.OtherStaff
                .FirstOrDefaultAsync(s => s.StaffID == id, cancellationToken);

            if (staff == null)
            {
                _logger.LogWarning("Staff member with ID {StaffId} not found for deletion", id);
                throw new InvalidOperationException($"Staff member with ID {id} not found");
            }

            staff.IsActive = false;
            staff.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted staff member with ID: {StaffId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting staff member with ID: {StaffId}", id);
            throw;
        }
    }

    /// <summary>
    /// Checks if a staff member exists
    /// </summary>
    /// <param name="id">Staff ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if staff member exists, otherwise false</returns>
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking if staff member exists with ID: {StaffId}", id);

            var exists = await _context.OtherStaff
                .AsNoTracking()
                .AnyAsync(s => s.StaffID == id && s.IsActive, cancellationToken);

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking staff member existence with ID: {StaffId}", id);
            throw;
        }
    }

    /// <summary>
    /// Searches for staff members by name, designation, or phone
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of matching staff members</returns>
    public async Task<IEnumerable<OtherStaff>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching staff members with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            var lowerSearchTerm = searchTerm.ToLower();

            var staff = await _context.OtherStaff
                .AsNoTracking()
                .Where(s => s.IsActive &&
                    (s.Name.ToLower().Contains(lowerSearchTerm) ||
                     s.Designation.ToLower().Contains(lowerSearchTerm) ||
                     s.Phone.Contains(searchTerm)))
                .OrderBy(s => s.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} staff members matching search term: {SearchTerm}",
                staff.Count, searchTerm);

            return staff;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching staff members with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
