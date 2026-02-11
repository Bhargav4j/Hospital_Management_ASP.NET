using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

public interface IClinicRepository
{
    Task<IEnumerable<Clinic>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Clinic?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Clinic> AddAsync(Clinic clinic, CancellationToken cancellationToken = default);
    Task UpdateAsync(Clinic clinic, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Clinic>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
