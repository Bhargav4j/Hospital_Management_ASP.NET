using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Doctor;

public class DoctorIndexModel : PageModel
{
    private readonly IAppointmentService _appointmentService;

    public DoctorIndexModel(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public int PendingCount { get; set; }
    public int TodayCount { get; set; }
    public int CompletedTodayCount { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Login");
        }

        int doctorId = int.Parse(userId);
        
        var pending = await _appointmentService.GetPendingAppointmentsAsync(doctorId);
        PendingCount = pending.Count();
        
        var allAppointments = await _appointmentService.GetDoctorAppointmentsAsync(doctorId);
        TodayCount = allAppointments.Count(a => a.AppointmentDate.Date == DateTime.Today);
        CompletedTodayCount = allAppointments.Count(a => a.AppointmentDate.Date == DateTime.Today && a.Status == "Completed");

        return Page();
    }
}
