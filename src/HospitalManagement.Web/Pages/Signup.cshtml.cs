using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages;

/// <summary>
/// Page model for user signup
/// </summary>
public class SignupModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly ILogger<SignupModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignupModel"/> class
    /// </summary>
    /// <param name="patientService">Patient service</param>
    /// <param name="logger">Logger instance</param>
    public SignupModel(
        IPatientService patientService,
        ILogger<SignupModel> logger)
    {
        _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the input model
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    /// <summary>
    /// Input model for signup form
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone
        /// </summary>
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the address
        /// </summary>
        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the birth date
        /// </summary>
        [Required(ErrorMessage = "Birth date is required")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the gender
        /// </summary>
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confirm password
        /// </summary>
        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// Handles GET requests
    /// </summary>
    public void OnGet()
    {
        // Initialize default birth date
        Input.BirthDate = DateTime.Today.AddYears(-25);
    }

    /// <summary>
    /// Handles POST requests for signup
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
            _logger.LogInformation("Signup attempt for email: {Email}", Input.Email);

            // Check if patient already exists
            var existingPatient = await _patientService.GetPatientByEmailAsync(Input.Email, cancellationToken);
            if (existingPatient != null)
            {
                _logger.LogWarning("Signup failed - email already exists: {Email}", Input.Email);
                ModelState.AddModelError(string.Empty, "An account with this email already exists");
                return Page();
            }

            // Create patient DTO
            var patientCreateDto = new PatientCreateDto
            {
                Name = Input.Name,
                Email = Input.Email,
                Phone = Input.Phone,
                Address = Input.Address,
                BirthDate = Input.BirthDate,
                Gender = Input.Gender
            };

            // Create patient
            var createdBy = Input.Email;
            var createdPatient = await _patientService.CreatePatientAsync(patientCreateDto, createdBy, cancellationToken);

            if (createdPatient != null)
            {
                _logger.LogInformation("Patient created successfully: {Email}", Input.Email);
                TempData["SuccessMessage"] = "Account created successfully! Please login to continue.";
                return RedirectToPage("/Login");
            }
            else
            {
                _logger.LogError("Failed to create patient account for email: {Email}", Input.Email);
                ModelState.AddModelError(string.Empty, "Failed to create account. Please try again.");
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during signup for email: {Email}", Input.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during signup. Please try again.");
            return Page();
        }
    }
}
