using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for TreatmentHistory entity operations
/// </summary>
public interface ITreatmentHistoryRepository
{
    Task<IEnumerable<TreatmentHistory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TreatmentHistory?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TreatmentHistory> AddAsync(TreatmentHistory treatmentHistory, CancellationToken cancellationToken = default);
    Task UpdateAsync(TreatmentHistory treatmentHistory, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TreatmentHistory>> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default);
}
