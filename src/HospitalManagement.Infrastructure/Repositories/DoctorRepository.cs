using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Doctor entity
/// </summary>
public class DoctorRepository : IDoctorRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DoctorRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoctorRepository"/> class
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="logger">Logger instance</param>
    public DoctorRepository(ApplicationDbContext context, ILogger<DoctorRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all active doctors
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of doctors</returns>
    public async Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all active doctors");

            var doctors = await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} doctors", doctors.Count);
            return doctors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all doctors");
            throw;
        }
    }

    /// <summary>
    /// Gets a doctor by ID
    /// </summary>
    /// <param name="id">Doctor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Doctor entity or null if not found</returns>
    public async Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctor with ID: {DoctorId}", id);

            var doctor = await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.DoctorID == id && d.IsActive, cancellationToken);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor with ID {DoctorId} not found", id);
            }

            return doctor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor with ID: {DoctorId}", id);
            throw;
        }
    }

    /// <summary>
    /// Gets a doctor by email
    /// </summary>
    /// <param name="email">Doctor email</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Doctor entity or null if not found</returns>
    public async Task<Doctor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctor with email: {Email}", email);

            var doctor = await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Email == email && d.IsActive, cancellationToken);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor with email {Email} not found", email);
            }

            return doctor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor with email: {Email}", email);
            throw;
        }
    }

    /// <summary>
    /// Adds a new doctor
    /// </summary>
    /// <param name="doctor">Doctor entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Added doctor entity</returns>
    public async Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new doctor: {DoctorName}", doctor.Name);

            await _context.Doctors.AddAsync(doctor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added doctor with ID: {DoctorId}", doctor.DoctorID);
            return doctor;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding doctor: {DoctorName}", doctor.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding doctor: {DoctorName}", doctor.Name);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing doctor
    /// </summary>
    /// <param name="doctor">Doctor entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating doctor with ID: {DoctorId}", doctor.DoctorID);

            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated doctor with ID: {DoctorId}", doctor.DoctorID);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating doctor with ID: {DoctorId}", doctor.DoctorID);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating doctor with ID: {DoctorId}", doctor.DoctorID);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating doctor with ID: {DoctorId}", doctor.DoctorID);
            throw;
        }
    }

    /// <summary>
    /// Deletes a doctor (soft delete by setting IsActive to false)
    /// </summary>
    /// <param name="id">Doctor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting doctor with ID: {DoctorId}", id);

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorID == id, cancellationToken);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor with ID {DoctorId} not found for deletion", id);
                throw new InvalidOperationException($"Doctor with ID {id} not found");
            }

            doctor.IsActive = false;
            doctor.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted doctor with ID: {DoctorId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting doctor with ID: {DoctorId}", id);
            throw;
        }
    }

    /// <summary>
    /// Checks if a doctor exists
    /// </summary>
    /// <param name="id">Doctor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if doctor exists, otherwise false</returns>
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking if doctor exists with ID: {DoctorId}", id);

            var exists = await _context.Doctors
                .AsNoTracking()
                .AnyAsync(d => d.DoctorID == id && d.IsActive, cancellationToken);

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking doctor existence with ID: {DoctorId}", id);
            throw;
        }
    }

    /// <summary>
    /// Checks if an email exists
    /// </summary>
    /// <param name="email">Doctor email</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if email exists, otherwise false</returns>
    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking if email exists: {Email}", email);

            var exists = await _context.Doctors
                .AsNoTracking()
                .AnyAsync(d => d.Email == email && d.IsActive, cancellationToken);

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking email existence: {Email}", email);
            throw;
        }
    }

    /// <summary>
    /// Searches for doctors by name, email, or specialization
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of matching doctors</returns>
    public async Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching doctors with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            var lowerSearchTerm = searchTerm.ToLower();

            var doctors = await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .Where(d => d.IsActive &&
                    (d.Name.ToLower().Contains(lowerSearchTerm) ||
                     d.Email.ToLower().Contains(lowerSearchTerm) ||
                     d.Specialization.ToLower().Contains(lowerSearchTerm) ||
                     d.Phone.Contains(searchTerm)))
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} doctors matching search term: {SearchTerm}",
                doctors.Count, searchTerm);

            return doctors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching doctors with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    /// <summary>
    /// Gets doctors by department
    /// </summary>
    /// <param name="deptNo">Department number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of doctors in the department</returns>
    public async Task<IEnumerable<Doctor>> GetByDepartmentAsync(int deptNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctors for department: {DeptNo}", deptNo);

            var doctors = await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .Where(d => d.DeptNo == deptNo && d.IsActive)
                .OrderBy(d => d.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} doctors for department: {DeptNo}",
                doctors.Count, deptNo);

            return doctors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctors for department: {DeptNo}", deptNo);
            throw;
        }
    }

    /// <summary>
    /// Validates doctor login credentials
    /// </summary>
    /// <param name="email">Doctor email</param>
    /// <param name="password">Doctor password</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Doctor entity if credentials are valid, otherwise null</returns>
    public async Task<Doctor?> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating login for doctor with email: {Email}", email);

            var doctor = await _context.Doctors
                .AsNoTracking()
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Email == email &&
                                        d.Password == password &&
                                        d.IsActive,
                                        cancellationToken);

            if (doctor == null)
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", email);
            }
            else
            {
                _logger.LogInformation("Successful login for doctor: {DoctorName}", doctor.Name);
            }

            return doctor;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating login for email: {Email}", email);
            throw;
        }
    }
}
