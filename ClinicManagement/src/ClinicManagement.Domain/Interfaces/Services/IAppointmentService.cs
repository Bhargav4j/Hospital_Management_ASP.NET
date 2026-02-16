using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Services;

public interface IAppointmentService
{
    Task<Appointment?> GetAppointmentByIdAsync(int id);
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    Task<Appointment> CreateAppointmentAsync(Appointment appointment);
    Task UpdateAppointmentAsync(Appointment appointment);
    Task DeleteAppointmentAsync(int id);
    Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int patientId);
    Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(int doctorId);
    Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(int patientId);
    Task<IEnumerable<Appointment>> GetPendingAppointmentsAsync(int doctorId);
    Task<IEnumerable<Appointment>> GetAppointmentHistoryAsync(int patientId);
    Task UpdateAppointmentStatusAsync(int appointmentId, string status);
    Task UpdateBillAsync(int appointmentId, decimal billAmount);
    Task MarkAsPaidAsync(int appointmentId);
}
