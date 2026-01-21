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
        return await _context.Feedbacks.AsNoTracking().Where(f => f.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<Feedback?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks.FirstOrDefaultAsync(f => f.FeedbackID == id && f.IsActive, cancellationToken);
    }

    public async Task<Feedback> AddAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        _context.Feedbacks.Add(feedback);
        await _context.SaveChangesAsync(cancellationToken);
        return feedback;
    }

    public async Task UpdateAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        _context.Feedbacks.Update(feedback);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var feedback = await _context.Feedbacks.FindAsync(new object[] { id }, cancellationToken);
        if (feedback != null)
        {
            feedback.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks.AnyAsync(f => f.FeedbackID == id && f.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Feedback>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks.Where(f => f.PatientID == patientId && f.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Feedback>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks.Where(f => f.DoctorID == doctorId && f.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<Feedback?> GetPendingFeedbackByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks.FirstOrDefaultAsync(f => f.PatientID == patientId && f.IsActive, cancellationToken);
    }
}
