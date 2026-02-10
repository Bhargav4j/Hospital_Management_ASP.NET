using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<NotificationRepository> _logger;

    public NotificationRepository(HospitalDbContext context, ILogger<NotificationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Notification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Notifications.Where(n => n.IsActive).AsNoTracking().ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all notifications");
            throw;
        }
    }

    public async Task<Notification?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && n.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notification with id {Id}", id);
            throw;
        }
    }

    public async Task<Notification> AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Notifications.AddAsync(notification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return notification;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding notification");
            throw;
        }
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating notification with id {Id}", notification.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var notification = await _context.Notifications.FindAsync(new object[] { id }, cancellationToken);
            if (notification != null)
            {
                notification.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting notification with id {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Notifications.AnyAsync(n => n.Id == id && n.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if notification exists with id {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Notification>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Notifications
                .Where(n => n.PatientId == patientId && n.IsActive)
                .OrderByDescending(n => n.CreatedDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notifications for patient {PatientId}", patientId);
            throw;
        }
    }

    public async Task<IEnumerable<Notification>> GetUnreadByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Notifications
                .Where(n => n.PatientId == patientId && !n.IsRead && n.IsActive)
                .OrderByDescending(n => n.CreatedDate)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unread notifications for patient {PatientId}", patientId);
            throw;
        }
    }
}
