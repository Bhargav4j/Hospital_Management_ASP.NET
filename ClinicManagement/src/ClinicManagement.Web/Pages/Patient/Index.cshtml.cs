using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Patient;

public class PatientIndexModel : PageModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;

    public PatientIndexModel(IAppointmentService appointmentService, IDoctorService doctorService)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
    }

    public int UpcomingCount { get; set; }
    public int CompletedCount { get; set; }
    public int DoctorCount { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Login");
        }

        int patientId = int.Parse(userId);
        
        var upcoming = await _appointmentService.GetUpcomingAppointmentsAsync(patientId);
        UpcomingCount = upcoming.Count();
        
        var history = await _appointmentService.GetAppointmentHistoryAsync(patientId);
        CompletedCount = history.Count();
        
        var doctors = await _doctorService.GetAllDoctorsAsync();
        DoctorCount = doctors.Count();

        return Page();
    }
}
