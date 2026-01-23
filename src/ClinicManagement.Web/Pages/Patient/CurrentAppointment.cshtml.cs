using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Patient;

public class CurrentAppointmentModel : PageModel
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ILogger<CurrentAppointmentModel> _logger;

    public List<AppointmentDto> Appointments { get; set; } = new();

    public CurrentAppointmentModel(IAppointmentRepository appointmentRepository, ILogger<CurrentAppointmentModel> logger)
    {
        _appointmentRepository = appointmentRepository;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToPage("/Login");
        }

        try
        {
            var appointments = await _appointmentRepository.GetByPatientIdAsync(userId.Value);
            Appointments = appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = a.Patient.Name,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor.Name,
                AppointmentDate = a.AppointmentDate,
                AppointmentTime = a.AppointmentTime,
                Status = a.Status,
                Notes = a.Notes
            }).ToList();

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading appointments for patient {PatientId}", userId);
            return Page();
        }
    }
}
