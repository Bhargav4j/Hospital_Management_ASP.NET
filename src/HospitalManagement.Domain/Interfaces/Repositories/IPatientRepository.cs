using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Patient?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default);
    Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
