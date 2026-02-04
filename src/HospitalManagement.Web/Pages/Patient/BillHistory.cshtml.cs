using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Patient;

/// <summary>
/// Page model for viewing patient bill history
/// </summary>
public class BillHistoryModel : PageModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<BillHistoryModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BillHistoryModel"/> class
    /// </summary>
    /// <param name="appointmentService">Appointment service</param>
    /// <param name="logger">Logger instance</param>
    public BillHistoryModel(
        IAppointmentService appointmentService,
        ILogger<BillHistoryModel> logger)
    {
        _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the list of bills (appointments with payment info)
    /// </summary>
    public IEnumerable<AppointmentDto> Bills { get; set; } = new List<AppointmentDto>();

    /// <summary>
    /// Gets or sets total bills count
    /// </summary>
    public int TotalBills { get; set; }

    /// <summary>
    /// Gets or sets paid bills count
    /// </summary>
    public int PaidBills { get; set; }

    /// <summary>
    /// Gets or sets unpaid bills count
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
            var userRole = HttpContext.Session.GetString("UserRole");
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (userRole != "Patient" || string.IsNullOrEmpty(userIdStr))
            {
                _logger.LogWarning("Unauthorized access attempt to bill history");
                TempData["ErrorMessage"] = "Please login as a patient to view bill history.";
                return RedirectToPage("/Login");
            }

            if (!int.TryParse(userIdStr, out int patientId))
            {
                _logger.LogError("Invalid patient ID in session");
                TempData["ErrorMessage"] = "Invalid session data.";
                return RedirectToPage("/Login");
            }

            _logger.LogInformation("Loading bill history for patient ID: {PatientId}", patientId);

            var allAppointments = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId, cancellationToken);
            Bills = allAppointments.Where(a => a.Status == "Completed" || a.Status == "Confirmed")
                .OrderByDescending(a => a.AppointmentDate);

            TotalBills = Bills.Count();
            PaidBills = Bills.Count(b => b.IsPaid);
            UnpaidBills = Bills.Count(b => !b.IsPaid);

            _logger.LogInformation("Successfully loaded bill history for patient ID: {PatientId}", patientId);

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading bill history");
            TempData["ErrorMessage"] = "An error occurred while loading bill history.";
            Bills = new List<AppointmentDto>();
            return Page();
        }
    }
}
