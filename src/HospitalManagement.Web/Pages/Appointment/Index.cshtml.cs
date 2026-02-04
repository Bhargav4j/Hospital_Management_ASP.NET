using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Appointment;

public class IndexModel : PageModel
{
    private readonly IAppointmentService _appointmentService;

    public IndexModel(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public IEnumerable<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(userRole))
        {
            TempData["ErrorMessage"] = "Please login to view appointments.";
            return RedirectToPage("/Login");
        }

        Appointments = await _appointmentService.GetAllAppointmentsAsync(cancellationToken);
        return Page();
    }
}
