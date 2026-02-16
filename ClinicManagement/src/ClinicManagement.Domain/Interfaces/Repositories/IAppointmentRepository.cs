using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Appointment>> GetByStatusAsync(string status);
    Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(int patientId);
    Task<IEnumerable<Appointment>> GetPendingAppointmentsAsync(int doctorId);
    Task<IEnumerable<Appointment>> GetAppointmentHistoryAsync(int patientId);
}
