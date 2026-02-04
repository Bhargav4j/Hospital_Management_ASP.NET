using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Patient;

/// <summary>
/// Page model for viewing patient treatment history
/// </summary>
public class TreatmentHistoryModel : PageModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<TreatmentHistoryModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TreatmentHistoryModel"/> class
    /// </summary>
    /// <param name="appointmentService">Appointment service</param>
    /// <param name="logger">Logger instance</param>
    public TreatmentHistoryModel(
        IAppointmentService appointmentService,
        ILogger<TreatmentHistoryModel> logger)
    {
        _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the list of treatments
    /// </summary>
    public IEnumerable<AppointmentDto> Treatments { get; set; } = new List<AppointmentDto>();

    /// <summary>
    /// Handles GET requests
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Action result</returns>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (userRole != "Patient" || string.IsNullOrEmpty(userIdStr))
            {
                _logger.LogWarning("Unauthorized access attempt to treatment history");
                TempData["ErrorMessage"] = "Please login as a patient to view treatment history.";
                return RedirectToPage("/Login");
            }

            if (!int.TryParse(userIdStr, out int patientId))
            {
                _logger.LogError("Invalid patient ID in session");
                TempData["ErrorMessage"] = "Invalid session data.";
                return RedirectToPage("/Login");
            }

            _logger.LogInformation("Loading treatment history for patient ID: {PatientId}", patientId);

            var allAppointments = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId, cancellationToken);

            Treatments = allAppointments
                .Where(a => a.Status == "Completed" && (!string.IsNullOrEmpty(a.Prescription) || !string.IsNullOrEmpty(a.Progress)))
                .OrderByDescending(a => a.AppointmentDate);

            _logger.LogInformation("Successfully loaded treatment history for patient ID: {PatientId}", patientId);

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading treatment history");
            TempData["ErrorMessage"] = "An error occurred while loading treatment history.";
            Treatments = new List<AppointmentDto>();
            return Page();
        }
    }
}
