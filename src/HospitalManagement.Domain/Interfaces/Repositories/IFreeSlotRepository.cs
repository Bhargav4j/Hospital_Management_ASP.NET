using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Interfaces.Repositories;

public interface IFreeSlotRepository
{
    Task<IEnumerable<FreeSlot>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FreeSlot?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FreeSlot> AddAsync(FreeSlot freeSlot, CancellationToken cancellationToken = default);
    Task UpdateAsync(FreeSlot freeSlot, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FreeSlot>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FreeSlot>> GetAvailableSlotsAsync(int doctorId, int patientId, CancellationToken cancellationToken = default);
}
