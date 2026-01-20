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
        return await _context.Feedbacks.Where(f => f.IsActive).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Feedback?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id && f.IsActive, cancellationToken);
    }

    public async Task<Feedback> AddAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        await _context.Feedbacks.AddAsync(feedback, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return feedback;
    }

    public async Task<Feedback> UpdateAsync(Feedback feedback, CancellationToken cancellationToken = default)
    {
        _context.Feedbacks.Update(feedback);
        await _context.SaveChangesAsync(cancellationToken);
        return feedback;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Feedbacks.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null) return false;
        entity.IsActive = false;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks.AnyAsync(f => f.Id == id && f.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Feedback>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Feedbacks.Where(f => f.IsActive && f.FeedbackText.Contains(searchTerm)).AsNoTracking().ToListAsync(cancellationToken);
    }
}
