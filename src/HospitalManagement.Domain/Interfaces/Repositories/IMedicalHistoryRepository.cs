using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

public interface IMedicalHistoryRepository
{
    Task<IEnumerable<MedicalHistory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MedicalHistory?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicalHistory>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<MedicalHistory> AddAsync(MedicalHistory medicalHistory, CancellationToken cancellationToken = default);
    Task UpdateAsync(MedicalHistory medicalHistory, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicalHistory>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
