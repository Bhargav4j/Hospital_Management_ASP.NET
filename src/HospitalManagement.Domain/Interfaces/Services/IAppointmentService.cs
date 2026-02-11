using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AppointmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default);
    Task<AppointmentDto> CreateAsync(AppointmentCreateDto appointmentCreateDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, AppointmentUpdateDto appointmentUpdateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppointmentDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
