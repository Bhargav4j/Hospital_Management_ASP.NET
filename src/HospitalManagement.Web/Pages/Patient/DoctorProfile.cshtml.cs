using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Patient;

/// <summary>
/// Page model for viewing doctor profile details
/// </summary>
public class DoctorProfileModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly ILogger<DoctorProfileModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoctorProfileModel"/> class
    /// </summary>
    /// <param name="doctorService">Doctor service</param>
    /// <param name="logger">Logger instance</param>
    public DoctorProfileModel(
        IDoctorService doctorService,
        ILogger<DoctorProfileModel> logger)
    {
        _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the doctor information
    /// </summary>
    public DoctorDto? Doctor { get; set; }

    /// <summary>
    /// Handles GET requests
    /// </summary>
    /// <param name="id">Doctor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Action result</returns>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if user is logged in as patient
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Patient")
            {
                _logger.LogWarning("Unauthorized access attempt to doctor profile");
                TempData["ErrorMessage"] = "Please login as a patient to view doctor profiles.";
                return RedirectToPage("/Login");
            }

            if (id <= 0)
            {
                _logger.LogWarning("Invalid doctor ID provided: {DoctorId}", id);
                TempData["ErrorMessage"] = "Invalid doctor ID.";
                return RedirectToPage("/Patient/ViewDoctors");
            }

            _logger.LogInformation("Loading doctor profile for doctor ID: {DoctorId}", id);

            // Get doctor information
            Doctor = await _doctorService.GetDoctorByIdAsync(id, cancellationToken);

            if (Doctor == null)
            {
                _logger.LogWarning("Doctor not found: {DoctorId}", id);
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToPage("/Patient/ViewDoctors");
            }

            _logger.LogInformation("Successfully loaded doctor profile for doctor ID: {DoctorId}", id);

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading doctor profile for ID: {DoctorId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading doctor profile.";
            return RedirectToPage("/Patient/ViewDoctors");
        }
    }
}
