using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Doctor;

public class TodayAppointmentsModel : PageModel
{
    private readonly IAppointmentService _appointmentService;

    public TodayAppointmentsModel(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public IEnumerable<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int doctorId))
            return RedirectToPage("/Login");

        var allAppointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId, cancellationToken);
        Appointments = allAppointments.Where(a => a.AppointmentDate.Date == DateTime.Today).OrderBy(a => a.AppointmentDate);

        return Page();
    }
}
