using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Patient entity
/// </summary>
public class PatientRepository : IPatientRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PatientRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientRepository"/> class
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="logger">Logger instance</param>
    public PatientRepository(ApplicationDbContext context, ILogger<PatientRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all active patients
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of patients</returns>
    public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all active patients");

            var patients = await _context.Patients
                .AsNoTracking()
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} patients", patients.Count);
            return patients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all patients");
            throw;
        }
    }

    /// <summary>
    /// Gets a patient by ID
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Patient entity or null if not found</returns>
    public async Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving patient with ID: {PatientId}", id);

            var patient = await _context.Patients
                .AsNoTracking()
                .Include(p => p.Appointments)
                .FirstOrDefaultAsync(p => p.PatientID == id && p.IsActive, cancellationToken);

            if (patient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found", id);
            }

            return patient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient with ID: {PatientId}", id);
            throw;
        }
    }

    /// <summary>
    /// Gets a patient by email
    /// </summary>
    /// <param name="email">Patient email</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Patient entity or null if not found</returns>
    public async Task<Patient?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving patient with email: {Email}", email);

            var patient = await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email == email && p.IsActive, cancellationToken);

            if (patient == null)
            {
                _logger.LogWarning("Patient with email {Email} not found", email);
            }

            return patient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient with email: {Email}", email);
            throw;
        }
    }

    /// <summary>
    /// Adds a new patient
    /// </summary>
    /// <param name="patient">Patient entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Added patient entity</returns>
    public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new patient: {PatientName}", patient.Name);

            await _context.Patients.AddAsync(patient, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added patient with ID: {PatientId}", patient.PatientID);
            return patient;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding patient: {PatientName}", patient.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding patient: {PatientName}", patient.Name);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing patient
    /// </summary>
    /// <param name="patient">Patient entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating patient with ID: {PatientId}", patient.PatientID);

            _context.Patients.Update(patient);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated patient with ID: {PatientId}", patient.PatientID);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating patient with ID: {PatientId}", patient.PatientID);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating patient with ID: {PatientId}", patient.PatientID);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient with ID: {PatientId}", patient.PatientID);
            throw;
        }
    }

    /// <summary>
    /// Deletes a patient (soft delete by setting IsActive to false)
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting patient with ID: {PatientId}", id);

            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientID == id, cancellationToken);

            if (patient == null)
            {
                _logger.LogWarning("Patient with ID {PatientId} not found for deletion", id);
                throw new InvalidOperationException($"Patient with ID {id} not found");
            }

            patient.IsActive = false;
            patient.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted patient with ID: {PatientId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient with ID: {PatientId}", id);
            throw;
        }
    }

    /// <summary>
    /// Checks if a patient exists
    /// </summary>
    /// <param name="id">Patient ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if patient exists, otherwise false</returns>
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking if patient exists with ID: {PatientId}", id);

            var exists = await _context.Patients
                .AsNoTracking()
                .AnyAsync(p => p.PatientID == id && p.IsActive, cancellationToken);

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking patient existence with ID: {PatientId}", id);
            throw;
        }
    }

    /// <summary>
    /// Searches for patients by name, email, or phone
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of matching patients</returns>
    public async Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching patients with term: {SearchTerm}", searchTerm);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllAsync(cancellationToken);
            }

            var lowerSearchTerm = searchTerm.ToLower();

            var patients = await _context.Patients
                .AsNoTracking()
                .Where(p => p.IsActive &&
                    (p.Name.ToLower().Contains(lowerSearchTerm) ||
                     p.Email.ToLower().Contains(lowerSearchTerm) ||
                     p.Phone.Contains(searchTerm)))
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} patients matching search term: {SearchTerm}",
                patients.Count, searchTerm);

            return patients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching patients with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    /// <summary>
    /// Validates patient login credentials
    /// </summary>
    /// <param name="email">Patient email</param>
    /// <param name="password">Patient password</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Patient entity if credentials are valid, otherwise null</returns>
    public async Task<Patient?> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating login for patient with email: {Email}", email);

            var patient = await _context.Patients
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email == email &&
                                        p.Password == password &&
                                        p.IsActive,
                                        cancellationToken);

            if (patient == null)
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", email);
            }
            else
            {
                _logger.LogInformation("Successful login for patient: {PatientName}", patient.Name);
            }

            return patient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating login for email: {Email}", email);
            throw;
        }
    }
}
