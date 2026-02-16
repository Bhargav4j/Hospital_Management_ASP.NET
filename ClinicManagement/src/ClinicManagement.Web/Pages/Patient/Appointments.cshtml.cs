using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Patient;

public class AppointmentsModel : PageModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper _mapper;

    public AppointmentsModel(IAppointmentService appointmentService, IMapper mapper)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
    }

    public IEnumerable<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Login");
        }

        int patientId = int.Parse(userId);
        var appointments = await _appointmentService.GetPatientAppointmentsAsync(patientId);
        Appointments = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);

        return Page();
    }
}
