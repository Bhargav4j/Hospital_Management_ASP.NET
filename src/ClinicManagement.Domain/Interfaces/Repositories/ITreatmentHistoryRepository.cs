using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

public interface ITreatmentHistoryRepository
{
    Task<IEnumerable<TreatmentHistory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TreatmentHistory?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TreatmentHistory> AddAsync(TreatmentHistory treatmentHistory, CancellationToken cancellationToken = default);
    Task UpdateAsync(TreatmentHistory treatmentHistory, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TreatmentHistory>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TreatmentHistory>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
}
