using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Doctor;

public class PendingAppointmentsModel : PageModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<PendingAppointmentsModel> _logger;

    public PendingAppointmentsModel(IAppointmentService appointmentService, ILogger<PendingAppointmentsModel> logger)
    {
        _appointmentService = appointmentService;
        _logger = logger;
    }

    public IEnumerable<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int doctorId))
            return RedirectToPage("/Login");

        var allAppointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId, cancellationToken);
        Appointments = allAppointments.Where(a => a.Status == "Pending").OrderBy(a => a.AppointmentDate);

        return Page();
    }
}
