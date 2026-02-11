using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services;

public interface IBillService
{
    Task<IEnumerable<BillDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BillDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BillDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<BillDto> CreateAsync(BillCreateDto billCreateDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, BillUpdateDto billUpdateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BillDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
