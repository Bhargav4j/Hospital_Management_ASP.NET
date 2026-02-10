using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<FeedbackRepository> _logger;

    public FeedbackRepository(HospitalDbContext context, ILogger<FeedbackRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Feedback>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Feedbacks.Include(f => f.Patient).Where(f => f.IsActive).AsNoTracking().ToListAsync(cancellationToken);
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
            return await _context.Feedbacks.Include(f => f.Patient).AsNoTracking().FirstOrDefaultAsync(f => f.Id == id && f.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving feedback with id {Id}", id);
            throw;
        }
    }

    public async Task<Feedback> AddAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Feedbacks.AddAsync(feedback, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
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
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating feedback with id {Id}", feedback.Id);
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
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting feedback with id {Id}", id);
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
            _logger.LogError(ex, "Error checking if feedback exists with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Feedback>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Feedbacks
                .Where(f => f.PatientId == patientId && f.IsActive)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving feedbacks for patient {PatientId}", patientId);
            throw;
        }
    }
}
