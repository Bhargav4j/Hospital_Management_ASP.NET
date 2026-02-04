using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Patient;

/// <summary>
/// Page model for patient dashboard
/// </summary>
public class HomeModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<HomeModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeModel"/> class
    /// </summary>
    /// <param name="patientService">Patient service</param>
    /// <param name="appointmentService">Appointment service</param>
    /// <param name="logger">Logger instance</param>
    public HomeModel(
        IPatientService patientService,
        IAppointmentService appointmentService,
        ILogger<HomeModel> logger)
    {
        _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the patient information
    /// </summary>
    public PatientDto? PatientInfo { get; set; }

    /// <summary>
    /// Gets or sets the upcoming appointments
    /// </summary>
    public IEnumerable<AppointmentDto> UpcomingAppointments { get; set; } = new List<AppointmentDto>();

    /// <summary>
    /// Gets or sets the total appointments count
    /// </summary>
    public int TotalAppointments { get; set; }

    /// <summary>
    /// Gets or sets the pending appointments count
    /// </summary>
    public int PendingAppointments { get; set; }

    /// <summary>
    /// Gets or sets the completed appointments count
    /// </summary>
    public int CompletedAppointments { get; set; }

    /// <summary>
    /// Gets or sets the unpaid bills count
    /// </summary>
    public int UnpaidBills { get; set; }

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
                _logger.LogWarning("Unauthorized access attempt to patient dashboard");
                TempData["ErrorMessage"] = "Please login as a patient to access this page.";
                return RedirectToPage("/Login");
            }

            if (!int.TryParse(userIdStr, out int patientId))
            {
                _logger.LogError("Invalid patient ID in session: {UserId}", userIdStr);
                TempData["ErrorMessage"] = "Invalid session data.";
                return RedirectToPage("/Login");
            }

            _logger.LogInformation("Loading patient dashboard for patient ID: {PatientId}", patientId);

            // Get patient information
            PatientInfo = await _patientService.GetPatientByIdAsync(patientId, cancellationToken);

            if (PatientInfo == null)
            {
                _logger.LogWarning("Patient not found: {PatientId}", patientId);
                TempData["ErrorMessage"] = "Patient information not found.";
                return RedirectToPage("/Index");
            }

            // Get patient appointments
            var allAppointments = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId, cancellationToken);

            // Calculate statistics
            TotalAppointments = allAppointments.Count();
            PendingAppointments = allAppointments.Count(a => a.Status == "Pending");
            CompletedAppointments = allAppointments.Count(a => a.Status == "Completed");
            UnpaidBills = allAppointments.Count(a => !a.IsPaid);

            // Get upcoming appointments (next 5)
            UpcomingAppointments = allAppointments
                .Where(a => a.AppointmentDate >= DateTime.Today && a.Status != "Cancelled")
                .OrderBy(a => a.AppointmentDate)
                .Take(5)
                .ToList();

            _logger.LogInformation("Successfully loaded patient dashboard for patient ID: {PatientId}", patientId);

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading patient dashboard");
            TempData["ErrorMessage"] = "An error occurred while loading dashboard.";
            return Page();
        }
    }
}
