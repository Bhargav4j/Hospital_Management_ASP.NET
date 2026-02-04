using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for FreeSlot entity
/// </summary>
public class FreeSlotRepository : IFreeSlotRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FreeSlotRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FreeSlotRepository"/> class
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="logger">Logger instance</param>
    public FreeSlotRepository(ApplicationDbContext context, ILogger<FreeSlotRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets all active free slots
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of free slots</returns>
    public async Task<IEnumerable<FreeSlot>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all active free slots");

            var freeSlots = await _context.FreeSlots
                .AsNoTracking()
                .Include(f => f.Doctor)
                    .ThenInclude(d => d!.Department)
                .Where(f => f.IsActive)
                .OrderBy(f => f.DoctorID)
                .ThenBy(f => f.StartTime)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} free slots", freeSlots.Count);
            return freeSlots;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all free slots");
            throw;
        }
    }

    /// <summary>
    /// Gets a free slot by ID
    /// </summary>
    /// <param name="id">Free slot ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Free slot entity or null if not found</returns>
    public async Task<FreeSlot?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving free slot with ID: {FreeSlotId}", id);

            var freeSlot = await _context.FreeSlots
                .AsNoTracking()
                .Include(f => f.Doctor)
                    .ThenInclude(d => d!.Department)
                .Include(f => f.Appointments)
                .FirstOrDefaultAsync(f => f.FreeSlotID == id && f.IsActive, cancellationToken);

            if (freeSlot == null)
            {
                _logger.LogWarning("Free slot with ID {FreeSlotId} not found", id);
            }

            return freeSlot;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving free slot with ID: {FreeSlotId}", id);
            throw;
        }
    }

    /// <summary>
    /// Adds a new free slot
    /// </summary>
    /// <param name="freeSlot">Free slot entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Added free slot entity</returns>
    public async Task<FreeSlot> AddAsync(FreeSlot freeSlot, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new free slot for doctor ID: {DoctorId}", freeSlot.DoctorID);

            await _context.FreeSlots.AddAsync(freeSlot, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added free slot with ID: {FreeSlotId}", freeSlot.FreeSlotID);
            return freeSlot;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while adding free slot for doctor ID: {DoctorId}",
                freeSlot.DoctorID);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding free slot for doctor ID: {DoctorId}", freeSlot.DoctorID);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing free slot
    /// </summary>
    /// <param name="freeSlot">Free slot entity to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task UpdateAsync(FreeSlot freeSlot, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating free slot with ID: {FreeSlotId}", freeSlot.FreeSlotID);

            _context.FreeSlots.Update(freeSlot);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated free slot with ID: {FreeSlotId}", freeSlot.FreeSlotID);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while updating free slot with ID: {FreeSlotId}",
                freeSlot.FreeSlotID);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating free slot with ID: {FreeSlotId}",
                freeSlot.FreeSlotID);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating free slot with ID: {FreeSlotId}", freeSlot.FreeSlotID);
            throw;
        }
    }

    /// <summary>
    /// Deletes a free slot (soft delete by setting IsActive to false)
    /// </summary>
    /// <param name="id">Free slot ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting free slot with ID: {FreeSlotId}", id);

            var freeSlot = await _context.FreeSlots
                .FirstOrDefaultAsync(f => f.FreeSlotID == id, cancellationToken);

            if (freeSlot == null)
            {
                _logger.LogWarning("Free slot with ID {FreeSlotId} not found for deletion", id);
                throw new InvalidOperationException($"Free slot with ID {id} not found");
            }

            freeSlot.IsActive = false;
            freeSlot.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted free slot with ID: {FreeSlotId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting free slot with ID: {FreeSlotId}", id);
            throw;
        }
    }

    /// <summary>
    /// Checks if a free slot exists
    /// </summary>
    /// <param name="id">Free slot ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if free slot exists, otherwise false</returns>
    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking if free slot exists with ID: {FreeSlotId}", id);

            var exists = await _context.FreeSlots
                .AsNoTracking()
                .AnyAsync(f => f.FreeSlotID == id && f.IsActive, cancellationToken);

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking free slot existence with ID: {FreeSlotId}", id);
            throw;
        }
    }

    /// <summary>
    /// Gets free slots by doctor ID
    /// </summary>
    /// <param name="doctorId">Doctor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of free slots for the doctor</returns>
    public async Task<IEnumerable<FreeSlot>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving free slots for doctor ID: {DoctorId}", doctorId);

            var freeSlots = await _context.FreeSlots
                .AsNoTracking()
                .Include(f => f.Doctor)
                    .ThenInclude(d => d!.Department)
                .Where(f => f.DoctorID == doctorId && f.IsActive)
                .OrderBy(f => f.StartTime)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} free slots for doctor ID: {DoctorId}",
                freeSlots.Count, doctorId);

            return freeSlots;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving free slots for doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }

    /// <summary>
    /// Gets available slots for a doctor that don't conflict with patient's existing appointments
    /// </summary>
    /// <param name="doctorId">Doctor ID</param>
    /// <param name="patientId">Patient ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of available free slots</returns>
    public async Task<IEnumerable<FreeSlot>> GetAvailableSlotsAsync(int doctorId, int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving available slots for doctor ID: {DoctorId} and patient ID: {PatientId}",
                doctorId, patientId);

            // Get all active slots for the doctor
            var doctorSlots = await _context.FreeSlots
                .AsNoTracking()
                .Include(f => f.Doctor)
                    .ThenInclude(d => d!.Department)
                .Where(f => f.DoctorID == doctorId && f.IsActive)
                .ToListAsync(cancellationToken);

            // Get all active appointments for the patient that are not completed
            var patientAppointments = await _context.Appointments
                .AsNoTracking()
                .Include(a => a.FreeSlot)
                .Where(a => a.PatientID == patientId &&
                           a.Status != "Completed" &&
                           a.Status != "Cancelled" &&
                           a.IsActive)
                .ToListAsync(cancellationToken);

            // Get slot IDs that the patient already has appointments for
            var bookedSlotIds = patientAppointments
                .Select(a => a.FreeSlotID)
                .ToHashSet();

            // Filter out slots that are already booked by the patient
            var availableSlots = doctorSlots
                .Where(slot => !bookedSlotIds.Contains(slot.FreeSlotID))
                .OrderBy(f => f.StartTime)
                .ToList();

            _logger.LogInformation("Found {Count} available slots for doctor ID: {DoctorId} and patient ID: {PatientId}",
                availableSlots.Count, doctorId, patientId);

            return availableSlots;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available slots for doctor ID: {DoctorId} and patient ID: {PatientId}",
                doctorId, patientId);
            throw;
        }
    }
}
