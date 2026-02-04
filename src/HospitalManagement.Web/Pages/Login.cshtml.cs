using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages;

/// <summary>
/// Page model for user login
/// </summary>
public class LoginModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly IDoctorService _doctorService;
    private readonly ILogger<LoginModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginModel"/> class
    /// </summary>
    /// <param name="patientService">Patient service</param>
    /// <param name="doctorService">Doctor service</param>
    /// <param name="logger">Logger instance</param>
    public LoginModel(
        IPatientService patientService,
        IDoctorService doctorService,
        ILogger<LoginModel> logger)
    {
        _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the input model
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    /// <summary>
    /// Input model for login form
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user role
        /// </summary>
        [Required(ErrorMessage = "Please select a role")]
        public string UserRole { get; set; } = string.Empty;
    }

    /// <summary>
    /// Handles GET requests
    /// </summary>
    public void OnGet()
    {
        // Check if user is already logged in
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
        {
            var role = HttpContext.Session.GetString("UserRole");
            RedirectToRoleBasedPage(role);
        }
    }

    /// <summary>
    /// Handles POST requests for login
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Action result</returns>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            _logger.LogInformation("Login attempt for email: {Email} as role: {Role}",
                Input.Email, Input.UserRole);

            bool loginSuccessful = false;
            string userId = string.Empty;
            string userName = string.Empty;

            switch (Input.UserRole)
            {
                case "Patient":
                    var patient = await _patientService.GetPatientByEmailAsync(Input.Email, cancellationToken);
                    if (patient != null && patient.IsActive)
                    {
                        loginSuccessful = true;
                        userId = patient.PatientID.ToString();
                        userName = patient.Name;
                    }
                    break;

                case "Doctor":
                    var doctor = await _doctorService.GetDoctorByEmailAsync(Input.Email, cancellationToken);
                    if (doctor != null && doctor.IsActive)
                    {
                        loginSuccessful = true;
                        userId = doctor.DoctorID.ToString();
                        userName = doctor.Name;
                    }
                    break;

                case "Admin":
                    // For demo purposes, using hardcoded admin credentials
                    // In production, this should be stored in a proper user management system
                    if (Input.Email == "admin@hospital.com" && Input.Password == "admin123")
                    {
                        loginSuccessful = true;
                        userId = "1";
                        userName = "Administrator";
                    }
                    break;

                default:
                    ModelState.AddModelError(string.Empty, "Invalid role selected");
                    return Page();
            }

            if (loginSuccessful)
            {
                // Set session variables
                HttpContext.Session.SetString("UserId", userId);
                HttpContext.Session.SetString("UserName", userName);
                HttpContext.Session.SetString("UserRole", Input.UserRole);
                HttpContext.Session.SetString("UserEmail", Input.Email);

                _logger.LogInformation("User logged in successfully: {Email} as {Role}",
                    Input.Email, Input.UserRole);

                TempData["SuccessMessage"] = $"Welcome back, {userName}!";

                // Redirect based on role
                return RedirectToRoleBasedPage(Input.UserRole);
            }
            else
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", Input.Email);
                ModelState.AddModelError(string.Empty, "Invalid email or password, or account is inactive");
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for email: {Email}", Input.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
            return Page();
        }
    }

    /// <summary>
    /// Redirects to the appropriate page based on user role
    /// </summary>
    /// <param name="role">User role</param>
    /// <returns>Action result</returns>
    private IActionResult RedirectToRoleBasedPage(string? role)
    {
        return role switch
        {
            "Patient" => RedirectToPage("/Patient/Home"),
            "Doctor" => RedirectToPage("/Doctor/Home"),
            "Admin" => RedirectToPage("/Admin/Index"),
            _ => RedirectToPage("/Index")
        };
    }
}
