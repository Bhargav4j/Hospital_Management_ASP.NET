using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagement.Web.Pages.Patient;

/// <summary>
/// Page model for viewing available doctors
/// </summary>
public class ViewDoctorsModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<ViewDoctorsModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewDoctorsModel"/> class
    /// </summary>
    /// <param name="doctorService">Doctor service</param>
    /// <param name="departmentService">Department service</param>
    /// <param name="logger">Logger instance</param>
    public ViewDoctorsModel(
        IDoctorService doctorService,
        IDepartmentService departmentService,
        ILogger<ViewDoctorsModel> logger)
    {
        _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the list of doctors
    /// </summary>
    public IEnumerable<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();

    /// <summary>
    /// Gets or sets the department list for dropdown
    /// </summary>
    public SelectList DepartmentList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    /// <summary>
    /// Gets or sets the search term
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Gets or sets the department filter
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public int? DepartmentId { get; set; }

    /// <summary>
    /// Gets or sets the sort by field
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public string SortBy { get; set; } = "name";

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
            if (userRole != "Patient")
            {
                _logger.LogWarning("Unauthorized access attempt to view doctors");
                TempData["ErrorMessage"] = "Please login as a patient to view doctors.";
                return RedirectToPage("/Login");
            }

            _logger.LogInformation("Loading doctors list");

            // Load departments for filter
            var departments = await _departmentService.GetAllDepartmentsAsync(cancellationToken);
            DepartmentList = new SelectList(departments, "DeptNo", "DeptName", DepartmentId);

            // Get all active doctors
            var allDoctors = await _doctorService.GetAllDoctorsAsync(cancellationToken);
            var activeDoctors = allDoctors.Where(d => d.IsActive).ToList();

            // Apply filters
            var filteredDoctors = activeDoctors.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                filteredDoctors = filteredDoctors.Where(d =>
                    d.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    d.Specialization.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (DepartmentId.HasValue && DepartmentId.Value > 0)
            {
                filteredDoctors = filteredDoctors.Where(d => d.DeptNo == DepartmentId.Value);
            }

            // Apply sorting
            filteredDoctors = SortBy switch
            {
                "experience" => filteredDoctors.OrderByDescending(d => d.Experience),
                "reputation" => filteredDoctors.OrderByDescending(d => d.ReputationIndex),
                "charges" => filteredDoctors.OrderBy(d => d.ChargesPerVisit),
                _ => filteredDoctors.OrderBy(d => d.Name)
            };

            Doctors = filteredDoctors.ToList();

            _logger.LogInformation("Successfully loaded {Count} doctors", Doctors.Count());

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading doctors list");
            TempData["ErrorMessage"] = "An error occurred while loading doctors.";
            Doctors = new List<DoctorDto>();
            return Page();
        }
    }
}
