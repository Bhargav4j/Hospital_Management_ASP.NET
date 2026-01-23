using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

public interface ITreatmentRepository
{
    Task<IEnumerable<Treatment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Treatment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Treatment?> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default);
    Task<Treatment> AddAsync(Treatment treatment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Treatment treatment, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
