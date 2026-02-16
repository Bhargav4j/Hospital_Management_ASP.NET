using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;

namespace ClinicManagement.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(int id)
    {
        return await _appointmentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
    {
        return await _appointmentRepository.GetAllAsync();
    }

    public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
    {
        appointment.Status = "Pending";
        appointment.IsPaid = false;
        appointment.FeedbackGiven = false;
        return await _appointmentRepository.AddAsync(appointment);
    }

    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        appointment.ModifiedDate = DateTime.UtcNow;
        await _appointmentRepository.UpdateAsync(appointment);
    }

    public async Task DeleteAppointmentAsync(int id)
    {
        await _appointmentRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int patientId)
    {
        return await _appointmentRepository.GetByPatientIdAsync(patientId);
    }

    public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(int doctorId)
    {
        return await _appointmentRepository.GetByDoctorIdAsync(doctorId);
    }

    public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(int patientId)
    {
        return await _appointmentRepository.GetUpcomingAppointmentsAsync(patientId);
    }

    public async Task<IEnumerable<Appointment>> GetPendingAppointmentsAsync(int doctorId)
    {
        return await _appointmentRepository.GetPendingAppointmentsAsync(doctorId);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentHistoryAsync(int patientId)
    {
        return await _appointmentRepository.GetAppointmentHistoryAsync(patientId);
    }

    public async Task UpdateAppointmentStatusAsync(int appointmentId, string status)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment != null)
        {
            appointment.Status = status;
            appointment.ModifiedDate = DateTime.UtcNow;
            await _appointmentRepository.UpdateAsync(appointment);
        }
    }

    public async Task UpdateBillAsync(int appointmentId, decimal billAmount)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment != null)
        {
            appointment.BillAmount = billAmount;
            appointment.ModifiedDate = DateTime.UtcNow;
            await _appointmentRepository.UpdateAsync(appointment);
        }
    }

    public async Task MarkAsPaidAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment != null)
        {
            appointment.IsPaid = true;
            appointment.ModifiedDate = DateTime.UtcNow;
            await _appointmentRepository.UpdateAsync(appointment);
        }
    }
}
