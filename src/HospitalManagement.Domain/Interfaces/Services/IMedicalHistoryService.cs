using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services;

public interface IMedicalHistoryService
{
    Task<IEnumerable<MedicalHistoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<MedicalHistoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicalHistoryDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<MedicalHistoryDto> CreateAsync(MedicalHistoryCreateDto medicalHistoryCreateDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, MedicalHistoryUpdateDto medicalHistoryUpdateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MedicalHistoryDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
