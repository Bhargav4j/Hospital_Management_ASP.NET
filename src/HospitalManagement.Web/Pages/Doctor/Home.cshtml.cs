using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Doctor;

/// <summary>
/// Page model for doctor dashboard
/// </summary>
public class HomeModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<HomeModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeModel"/> class
    /// </summary>
    public HomeModel(
        IDoctorService doctorService,
        IAppointmentService appointmentService,
        ILogger<HomeModel> logger)
    {
        _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the doctor information
    /// </summary>
    public DoctorDto? DoctorInfo { get; set; }

    /// <summary>
    /// Gets or sets today's appointments list
    /// </summary>
    public IEnumerable<AppointmentDto> TodayAppointmentsList { get; set; } = new List<AppointmentDto>();

    /// <summary>
    /// Gets or sets total appointments count
    /// </summary>
    public int TotalAppointments { get; set; }

    /// <summary>
    /// Gets or sets pending appointments count
    /// </summary>
    public int PendingAppointments { get; set; }

    /// <summary>
    /// Gets or sets today's appointments count
    /// </summary>
    public int TodayAppointments { get; set; }

    /// <summary>
    /// Handles GET requests
    /// </summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (userRole != "Doctor" || string.IsNullOrEmpty(userIdStr))
            {
                _logger.LogWarning("Unauthorized access attempt to doctor dashboard");
                TempData["ErrorMessage"] = "Please login as a doctor to access this page.";
                return RedirectToPage("/Login");
            }

            if (!int.TryParse(userIdStr, out int doctorId))
            {
                _logger.LogError("Invalid doctor ID in session");
                TempData["ErrorMessage"] = "Invalid session data.";
                return RedirectToPage("/Login");
            }

            _logger.LogInformation("Loading doctor dashboard for doctor ID: {DoctorId}", doctorId);

            DoctorInfo = await _doctorService.GetDoctorByIdAsync(doctorId, cancellationToken);

            if (DoctorInfo == null)
            {
                _logger.LogWarning("Doctor not found: {DoctorId}", doctorId);
                TempData["ErrorMessage"] = "Doctor information not found.";
                return RedirectToPage("/Index");
            }

            var allAppointments = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId, cancellationToken);

            TotalAppointments = allAppointments.Count();
            PendingAppointments = allAppointments.Count(a => a.Status == "Pending");
            TodayAppointmentsList = allAppointments
                .Where(a => a.AppointmentDate.Date == DateTime.Today)
                .OrderBy(a => a.AppointmentDate)
                .Take(5)
                .ToList();
            TodayAppointments = TodayAppointmentsList.Count();

            _logger.LogInformation("Successfully loaded doctor dashboard for doctor ID: {DoctorId}", doctorId);

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading doctor dashboard");
            TempData["ErrorMessage"] = "An error occurred while loading dashboard.";
            return Page();
        }
    }
}
