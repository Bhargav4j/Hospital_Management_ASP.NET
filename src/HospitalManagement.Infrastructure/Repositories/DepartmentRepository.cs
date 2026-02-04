using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Department entity
/// </summary>
public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DepartmentRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentRepository"/> class
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="logger">Logger instance</param>
    public DepartmentRepository(ApplicationDbContext context, ILogger<DepartmentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all active departments
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of departments</returns>
    public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all active departments");

            var departments = await _context.Departments
                .AsNoTracking()
                .Include(d => d.Doctors)
                .Where(d => d.IsActive)
                .OrderBy(d => d.DeptName)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} departments", departments.Count);
            return departments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all departments");
            throw;
        }
    }

    /// <summary>
    /// Gets a department by ID
    /// </summary>
    /// <param name="id">Department ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Department entity or null if not found</returns>
    public async Task<Department?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving department with ID: {DeptNo}", id);

            var department = await _context.Departments
                .AsNoTracking()
                .Include(d => d.Doctors)
                .FirstOrDefaultAsync(d => d.DeptNo == id && d.IsActive, cancellationToken);

            if (department == null)
            {
                _logger.LogWarning("Department with ID {DeptNo} not found", id);
            }

            return department;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department with ID: {DeptNo}", id);
            throw;
        }
    }

    /// <summary>
    /// Adds a new department
    /// </summary>
    /// <param name="department">Department entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Added department entity</returns>
    public async Task<Department> AddAsync(Department department, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new department: {DeptName}", department.DeptName);

            await _context.Departments.AddAsync(department, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added department with ID: {DeptNo}", department.DeptNo);
            return department;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding department: {DeptName}", department.DeptName);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding department: {DeptName}", department.DeptName);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing department
    /// </summary>
    /// <param name="department">Department entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task UpdateAsync(Department department, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating department with ID: {DeptNo}", department.DeptNo);

            _context.Departments.Update(department);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated department with ID: {DeptNo}", department.DeptNo);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating department with ID: {DeptNo}", department.DeptNo);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating department with ID: {DeptNo}", department.DeptNo);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department with ID: {DeptNo}", department.DeptNo);
            throw;
        }
    }

    /// <summary>
    /// Deletes a department (soft delete by setting IsActive to false)
    /// </summary>
    /// <param name="id">Department ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting department with ID: {DeptNo}", id);

            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DeptNo == id, cancellationToken);

            if (department == null)
            {
                _logger.LogWarning("Department with ID {DeptNo} not found for deletion", id);
                throw new InvalidOperationException($"Department with ID {id} not found");
            }

            department.IsActive = false;
            department.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted department with ID: {DeptNo}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department with ID: {DeptNo}", id);
            throw;
        }
    }

    /// <summary>
    /// Checks if a department exists
    /// </summary>
    /// <param name="id">Department ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if department exists, otherwise false</returns>
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking if department exists with ID: {DeptNo}", id);

            var exists = await _context.Departments
                .AsNoTracking()
                .AnyAsync(d => d.DeptNo == id && d.IsActive, cancellationToken);

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking department existence with ID: {DeptNo}", id);
            throw;
        }
    }

    /// <summary>
    /// Searches for departments by name or description
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of matching departments</returns>
    public async Task<IEnumerable<Department>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching departments with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            var lowerSearchTerm = searchTerm.ToLower();

            var departments = await _context.Departments
                .AsNoTracking()
                .Include(d => d.Doctors)
                .Where(d => d.IsActive &&
                    (d.DeptName.ToLower().Contains(lowerSearchTerm) ||
                     (d.Description != null && d.Description.ToLower().Contains(lowerSearchTerm))))
                .OrderBy(d => d.DeptName)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} departments matching search term: {SearchTerm}",
                departments.Count, searchTerm);

            return departments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching departments with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
