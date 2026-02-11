using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Treatment entity operations
/// </summary>
public interface ITreatmentRepository
{
    Task<IEnumerable<Treatment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Treatment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Treatment> AddAsync(Treatment treatment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Treatment treatment, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Treatment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Treatment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
}
