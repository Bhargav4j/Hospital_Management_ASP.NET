using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Doctor;

public class UpdateAppointmentModel : PageModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper _mapper;

    public UpdateAppointmentModel(IAppointmentService appointmentService, IMapper mapper)
    {
        _appointmentService = appointmentService;
        _mapper = mapper;
    }

    [BindProperty(SupportsGet = true)]
    public int AppointmentId { get; set; }

    [BindProperty]
    [Required]
    public string Status { get; set; } = string.Empty;

    [BindProperty]
    public string Progress { get; set; } = string.Empty;

    [BindProperty]
    public string Prescription { get; set; } = string.Empty;

    [BindProperty]
    public decimal BillAmount { get; set; }

    public AppointmentDto? Appointment { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Login");
        }

        var appointment = await _appointmentService.GetAppointmentByIdAsync(AppointmentId);
        if (appointment == null)
        {
            return RedirectToPage("/Doctor/Appointments");
        }

        Appointment = _mapper.Map<AppointmentDto>(appointment);
        Status = appointment.Status;
        Progress = appointment.Progress;
        Prescription = appointment.Prescription;
        BillAmount = appointment.BillAmount;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Login");
        }

        if (!ModelState.IsValid)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(AppointmentId);
            Appointment = _mapper.Map<AppointmentDto>(appointment);
            return Page();
        }

        try
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(AppointmentId);
            if (appointment != null)
            {
                appointment.Status = Status;
                appointment.Progress = Progress;
                appointment.Prescription = Prescription;
                appointment.BillAmount = BillAmount;
                
                await _appointmentService.UpdateAppointmentAsync(appointment);
            }

            return RedirectToPage("/Doctor/Appointments");
        }
        catch (Exception ex)
        {
            var appt = await _appointmentService.GetAppointmentByIdAsync(AppointmentId);
            Appointment = _mapper.Map<AppointmentDto>(appt);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the appointment.");
            return Page();
        }
    }
}
