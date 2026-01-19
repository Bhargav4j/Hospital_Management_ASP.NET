using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
    Task<int> CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto);
    Task UpdateAppointmentAsync(UpdateAppointmentDto updateAppointmentDto);
    Task DeleteAppointmentAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientAsync(int patientId);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorAsync(int doctorId);
}
