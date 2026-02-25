using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;

namespace ClinicManagement.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICacheService _cacheService;

    private const string PatientAppointmentsCacheKeyPrefix = "appointments:patient:";
    private const string DoctorAppointmentsCacheKeyPrefix = "appointments:doctor:";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public AppointmentService(IAppointmentRepository appointmentRepository, ICacheService cacheService)
    {
        _appointmentRepository = appointmentRepository;
        _cacheService = cacheService;
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
        var result = await _appointmentRepository.AddAsync(appointment);
        await InvalidateAppointmentCacheAsync(appointment.PatientID, appointment.DoctorID);
        return result;
    }

    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        appointment.ModifiedDate = DateTime.UtcNow;
        await _appointmentRepository.UpdateAsync(appointment);
        await InvalidateAppointmentCacheAsync(appointment.PatientID, appointment.DoctorID);
    }

    public async Task DeleteAppointmentAsync(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        await _appointmentRepository.DeleteAsync(id);
        if (appointment != null)
            await InvalidateAppointmentCacheAsync(appointment.PatientID, appointment.DoctorID);
    }

    public async Task<IEnumerable<Appointment>> GetPatientAppointmentsAsync(int patientId)
    {
        var cacheKey = $"{PatientAppointmentsCacheKeyPrefix}{patientId}";
        var cached = await _cacheService.GetAsync<List<Appointment>>(cacheKey);
        if (cached != null)
            return cached;

        var appointments = (await _appointmentRepository.GetByPatientIdAsync(patientId)).ToList();
        await _cacheService.SetAsync(cacheKey, appointments, CacheDuration);
        return appointments;
    }

    public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsAsync(int doctorId)
    {
        var cacheKey = $"{DoctorAppointmentsCacheKeyPrefix}{doctorId}";
        var cached = await _cacheService.GetAsync<List<Appointment>>(cacheKey);
        if (cached != null)
            return cached;

        var appointments = (await _appointmentRepository.GetByDoctorIdAsync(doctorId)).ToList();
        await _cacheService.SetAsync(cacheKey, appointments, CacheDuration);
        return appointments;
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
            await InvalidateAppointmentCacheAsync(appointment.PatientID, appointment.DoctorID);
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
            await InvalidateAppointmentCacheAsync(appointment.PatientID, appointment.DoctorID);
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
            await InvalidateAppointmentCacheAsync(appointment.PatientID, appointment.DoctorID);
        }
    }

    private async Task InvalidateAppointmentCacheAsync(int patientId, int doctorId)
    {
        await _cacheService.RemoveAsync($"{PatientAppointmentsCacheKeyPrefix}{patientId}");
        await _cacheService.RemoveAsync($"{DoctorAppointmentsCacheKeyPrefix}{doctorId}");
    }
}
