using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services;

public interface IClinicService
{
    Task<IEnumerable<ClinicDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClinicDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ClinicDto> CreateAsync(ClinicCreateDto clinicCreateDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, ClinicUpdateDto clinicUpdateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClinicDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
