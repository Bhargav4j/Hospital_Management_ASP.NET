using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages;

public class RegisterModel : PageModel
{
    private readonly IPatientRepository _patientRepository;
    private readonly IAuthenticationService _authService;
    private readonly ILogger<RegisterModel> _logger;

    [BindProperty]
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; } = string.Empty;

    [BindProperty]
    public string? PhoneNumber { get; set; }

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public RegisterModel(
        IPatientRepository patientRepository,
        IAuthenticationService authService,
        ILogger<RegisterModel> logger)
    {
        _patientRepository = patientRepository;
        _authService = authService;
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Check if email already exists
            var existingPatient = await _patientRepository.GetByEmailAsync(Email);
            if (existingPatient != null)
            {
                ErrorMessage = "Email already registered. Please use a different email.";
                return Page();
            }

            // Hash password
            var hashedPassword = await _authService.HashPasswordAsync(Password);

            // Create new patient
            var patient = new Patient
            {
                Name = Name,
                Email = Email,
                Password = hashedPassword,
                Gender = Gender,
                PhoneNumber = PhoneNumber,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            await _patientRepository.AddAsync(patient);

            _logger.LogInformation("New patient registered: {Email}", Email);

            SuccessMessage = "Registration successful! Please login to continue.";
            return RedirectToPage("/Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", Email);
            ErrorMessage = "An error occurred during registration. Please try again.";
            return Page();
        }
    }
}
