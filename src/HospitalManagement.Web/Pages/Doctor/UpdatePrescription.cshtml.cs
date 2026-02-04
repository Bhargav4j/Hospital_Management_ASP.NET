using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages.Doctor;

public class UpdatePrescriptionModel : PageModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<UpdatePrescriptionModel> _logger;

    public UpdatePrescriptionModel(IAppointmentService appointmentService, ILogger<UpdatePrescriptionModel> logger)
    {
        _appointmentService = appointmentService;
        _logger = logger;
    }

    public AppointmentDto? Appointment { get; set; }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {
        public int AppointmentId { get; set; }
        [Required] public string Prescription { get; set; } = string.Empty;
        public string? Progress { get; set; }
        [Required] public string Status { get; set; } = "Completed";
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        Appointment = await _appointmentService.GetAppointmentByIdAsync(id, cancellationToken);
        if (Appointment == null) return RedirectToPage("/Doctor/Home");

        Input.AppointmentId = Appointment.AppointmentID;
        Input.Prescription = Appointment.Prescription ?? string.Empty;
        Input.Progress = Appointment.Progress;
        Input.Status = Appointment.Status;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            Appointment = await _appointmentService.GetAppointmentByIdAsync(Input.AppointmentId, cancellationToken);
            return Page();
        }

        try
        {
            // Get the existing appointment to preserve fields not in the form
            var existingAppointment = await _appointmentService.GetAppointmentByIdAsync(Input.AppointmentId, cancellationToken);
            if (existingAppointment == null)
            {
                TempData["ErrorMessage"] = "Appointment not found.";
                return RedirectToPage("/Doctor/Home");
            }

            var updateDto = new AppointmentUpdateDto
            {
                AppointmentID = Input.AppointmentId,
                AppointmentDate = existingAppointment.AppointmentDate,
                Status = Input.Status,
                Disease = existingAppointment.Disease,
                Progress = Input.Progress,
                Prescription = Input.Prescription,
                IsPaid = existingAppointment.IsPaid,
                FeedbackGiven = existingAppointment.FeedbackGiven,
                IsActive = existingAppointment.IsActive
            };

            await _appointmentService.UpdateAppointmentAsync(updateDto, "Doctor", cancellationToken);
            TempData["SuccessMessage"] = "Prescription updated successfully!";
            return RedirectToPage("/Doctor/Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating prescription");
            ModelState.AddModelError(string.Empty, "Error updating prescription.");
            Appointment = await _appointmentService.GetAppointmentByIdAsync(Input.AppointmentId, cancellationToken);
            return Page();
        }
    }
}
