using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<FeedbackRepository> _logger;

    public FeedbackRepository(ClinicDbContext context, ILogger<FeedbackRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Feedback>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Feedbacks
                .AsNoTracking()
                .Include(f => f.Patient)
                .Include(f => f.Doctor)
                .Where(f => f.IsActive)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all feedbacks");
            throw;
        }
    }

    public async Task<Feedback?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Feedbacks
                .AsNoTracking()
                .Include(f => f.Patient)
                .Include(f => f.Doctor)
                .FirstOrDefaultAsync(f => f.Id == id && f.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving feedback with ID {FeedbackId}", id);
            throw;
        }
    }

    public async Task<Feedback> AddAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        try
        {
            feedback.CreatedDate = DateTime.UtcNow;
            feedback.IsActive = true;
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Feedback added successfully with ID {FeedbackId}", feedback.Id);
            return feedback;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding feedback");
            throw;
        }
    }

    public async Task UpdateAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        try
        {
            feedback.ModifiedDate = DateTime.UtcNow;
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Feedback updated successfully with ID {FeedbackId}", feedback.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating feedback with ID {FeedbackId}", feedback.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var feedback = await _context.Feedbacks.FindAsync(new object[] { id }, cancellationToken);
            if (feedback != null)
            {
                feedback.IsActive = false;
                feedback.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Feedback soft-deleted successfully with ID {FeedbackId}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting feedback with ID {FeedbackId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Feedbacks
                .AnyAsync(f => f.Id == id && f.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if feedback exists with ID {FeedbackId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Feedback>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Feedbacks
                .AsNoTracking()
                .Include(f => f.Patient)
                .Include(f => f.Doctor)
                .Where(f => f.DoctorId == doctorId && f.IsActive)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving feedbacks for doctor {DoctorId}", doctorId);
            throw;
        }
    }

    public async Task<IEnumerable<Feedback>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Feedbacks
                .AsNoTracking()
                .Include(f => f.Patient)
                .Include(f => f.Doctor)
                .Where(f => f.PatientId == patientId && f.IsActive)
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving feedbacks for patient {PatientId}", patientId);
            throw;
        }
    }
}
