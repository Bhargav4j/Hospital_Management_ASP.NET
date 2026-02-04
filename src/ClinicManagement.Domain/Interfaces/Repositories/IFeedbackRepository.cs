using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Feedback entity operations
/// </summary>
public interface IFeedbackRepository
{
    Task<IEnumerable<Feedback>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Feedback?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Feedback>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<Feedback> AddAsync(Feedback feedback, CancellationToken cancellationToken = default);
    Task UpdateAsync(Feedback feedback, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
