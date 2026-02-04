using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Doctor;

/// <summary>
/// Page model for doctor listing (admin view)
/// </summary>
public class IndexModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class
    /// </summary>
    /// <param name="doctorService">Doctor service</param>
    /// <param name="logger">Logger instance</param>
    public IndexModel(
        IDoctorService doctorService,
        ILogger<IndexModel> logger)
    {
        _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the list of doctors
    /// </summary>
    public IEnumerable<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();

    /// <summary>
    /// Gets or sets the search term
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

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
            if (userRole != "Admin")
            {
                _logger.LogWarning("Unauthorized access attempt to doctor list");
                TempData["ErrorMessage"] = "You don't have permission to access this page.";
                return RedirectToPage("/Index");
            }

            _logger.LogInformation("Loading doctor list");

            var allDoctors = await _doctorService.GetAllDoctorsAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Doctors = allDoctors.Where(d =>
                    d.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    d.Specialization.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                Doctors = allDoctors;
            }

            _logger.LogInformation("Successfully loaded {Count} doctors", Doctors.Count());

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading doctor list");
            TempData["ErrorMessage"] = "An error occurred while loading doctors.";
            Doctors = new List<DoctorDto>();
            return Page();
        }
    }
}
