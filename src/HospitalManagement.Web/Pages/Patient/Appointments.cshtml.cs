using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Patient;

/// <summary>
/// Page model for viewing patient appointments
/// </summary>
public class AppointmentsModel : PageModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentsModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentsModel"/> class
    /// </summary>
    /// <param name="appointmentService">Appointment service</param>
    /// <param name="logger">Logger instance</param>
    public AppointmentsModel(
        IAppointmentService appointmentService,
        ILogger<AppointmentsModel> logger)
    {
        _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the list of appointments
    /// </summary>
    public IEnumerable<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

    /// <summary>
    /// Gets or sets the filter
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public string Filter { get; set; } = "all";

    /// <summary>
    /// Gets or sets the all appointments count
    /// </summary>
    public int AllCount { get; set; }

    /// <summary>
    /// Gets or sets the upcoming appointments count
    /// </summary>
    public int UpcomingCount { get; set; }

    /// <summary>
    /// Gets or sets the pending appointments count
    /// </summary>
    public int PendingCount { get; set; }

    /// <summary>
    /// Gets or sets the completed appointments count
    /// </summary>
    public int CompletedCount { get; set; }

    /// <summary>
    /// Handles GET requests
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Action result</returns>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if user is logged in as patient
            var userRole = HttpContext.Session.GetString("UserRole");
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (userRole != "Patient" || string.IsNullOrEmpty(userIdStr))
            {
                _logger.LogWarning("Unauthorized access attempt to patient appointments");
                TempData["ErrorMessage"] = "Please login as a patient to view appointments.";
                return RedirectToPage("/Login");
            }

            if (!int.TryParse(userIdStr, out int patientId))
            {
                _logger.LogError("Invalid patient ID in session: {UserId}", userIdStr);
                TempData["ErrorMessage"] = "Invalid session data.";
                return RedirectToPage("/Login");
            }

            _logger.LogInformation("Loading appointments for patient ID: {PatientId}", patientId);

            // Get all appointments for the patient
            var allAppointments = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId, cancellationToken);
            var appointmentsList = allAppointments.ToList();

            // Calculate counts
            AllCount = appointmentsList.Count;
            UpcomingCount = appointmentsList.Count(a => a.AppointmentDate >= DateTime.Today && a.Status != "Cancelled");
            PendingCount = appointmentsList.Count(a => a.Status == "Pending");
            CompletedCount = appointmentsList.Count(a => a.Status == "Completed");

            // Apply filter
            Appointments = Filter switch
            {
                "upcoming" => appointmentsList.Where(a => a.AppointmentDate >= DateTime.Today && a.Status != "Cancelled")
                    .OrderBy(a => a.AppointmentDate),
                "pending" => appointmentsList.Where(a => a.Status == "Pending")
                    .OrderBy(a => a.AppointmentDate),
                "completed" => appointmentsList.Where(a => a.Status == "Completed")
                    .OrderByDescending(a => a.AppointmentDate),
                _ => appointmentsList.OrderByDescending(a => a.AppointmentDate)
            };

            _logger.LogInformation("Successfully loaded {Count} appointments for patient ID: {PatientId}",
                Appointments.Count(), patientId);

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading patient appointments");
            TempData["ErrorMessage"] = "An error occurred while loading appointments.";
            Appointments = new List<AppointmentDto>();
            return Page();
        }
    }
}
