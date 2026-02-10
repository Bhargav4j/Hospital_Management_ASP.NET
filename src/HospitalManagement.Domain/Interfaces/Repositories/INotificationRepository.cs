using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Notification entity operations
/// </summary>
public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Notification?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Notification> AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetUnreadByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
}
