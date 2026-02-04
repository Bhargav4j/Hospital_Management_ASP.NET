using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Patient;

/// <summary>
/// Page model for patient listing (admin view)
/// </summary>
public class IndexModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class
    /// </summary>
    /// <param name="patientService">Patient service</param>
    /// <param name="logger">Logger instance</param>
    public IndexModel(
        IPatientService patientService,
        ILogger<IndexModel> logger)
    {
        _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the list of patients
    /// </summary>
    public IEnumerable<PatientDto> Patients { get; set; } = new List<PatientDto>();

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
            // Check if user is admin
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                _logger.LogWarning("Unauthorized access attempt to patient list by role: {Role}", userRole);
                TempData["ErrorMessage"] = "You don't have permission to access this page.";
                return RedirectToPage("/Index");
            }

            _logger.LogInformation("Loading patient list");

            // Get all patients
            var allPatients = await _patientService.GetAllPatientsAsync(cancellationToken);

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Patients = allPatients.Where(p =>
                    p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                _logger.LogInformation("Filtered patients by search term: {SearchTerm}, Found: {Count}",
                    SearchTerm, Patients.Count());
            }
            else
            {
                Patients = allPatients;
            }

            _logger.LogInformation("Successfully loaded {Count} patients", Patients.Count());

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading patient list");
            TempData["ErrorMessage"] = "An error occurred while loading patients.";
            Patients = new List<PatientDto>();
            return Page();
        }
    }
}
