using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Feedback entity
/// </summary>
public class FeedbackRepository : IFeedbackRepository
{
    private readonly ClinicDbContext _context;
    private readonly ILogger<FeedbackRepository> _logger;

    public FeedbackRepository(ClinicDbContext context, ILogger<FeedbackRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Feedback>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all feedbacks");
            return await _context.Feedbacks
                .AsNoTracking()
                .Include(f => f.Patient)
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
            _logger.LogInformation("Retrieving feedback with ID {FeedbackId}", id);
            return await _context.Feedbacks
                .AsNoTracking()
                .Include(f => f.Patient)
                .FirstOrDefaultAsync(f => f.Id == id && f.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving feedback with ID {FeedbackId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Feedback>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving feedbacks for patient {PatientId}", patientId);
            return await _context.Feedbacks
                .AsNoTracking()
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

    public async Task<Feedback> AddAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Adding new feedback for patient {PatientId}", feedback.PatientId);
            feedback.CreatedDate = DateTime.UtcNow;
            feedback.IsActive = true;

            await _context.Feedbacks.AddAsync(feedback, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added feedback with ID {FeedbackId}", feedback.Id);
            return feedback;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding feedback for patient {PatientId}", feedback.PatientId);
            throw;
        }
    }

    public async Task UpdateAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating feedback with ID {FeedbackId}", feedback.Id);
            feedback.ModifiedDate = DateTime.UtcNow;

            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated feedback with ID {FeedbackId}", feedback.Id);
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
            _logger.LogInformation("Deleting feedback with ID {FeedbackId}", id);
            var feedback = await _context.Feedbacks.FindAsync(new object[] { id }, cancellationToken);

            if (feedback != null)
            {
                feedback.IsActive = false;
                feedback.ModifiedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully deleted feedback with ID {FeedbackId}", id);
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
            return await _context.Feedbacks.AnyAsync(f => f.Id == id && f.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of feedback with ID {FeedbackId}", id);
            throw;
        }
    }
}
