using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages;

public class SignUpModel : PageModel
{
    private readonly IPatientRepository _patientRepository;
    private readonly IAuthenticationService _authService;
    private readonly ILogger<SignUpModel> _logger;

    public SignUpModel(
        IPatientRepository patientRepository,
        IAuthenticationService authService,
        ILogger<SignUpModel> logger)
    {
        _patientRepository = patientRepository;
        _authService = authService;
        _logger = logger;
    }

    [BindProperty]
    [Required]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public DateTime BirthDate { get; set; }

    [BindProperty]
    [Required]
    public int Gender { get; set; }

    [BindProperty]
    [Required]
    public string Address { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

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
            var existingPatient = await _patientRepository.GetByEmailAsync(Email);
            if (existingPatient != null)
            {
                ErrorMessage = "Email already exists. Please use a different email.";
                return Page();
            }

            var passwordHash = await _authService.HashPasswordAsync(Password);

            var patient = new Patient
            {
                Name = Name,
                Email = Email,
                PasswordHash = passwordHash,
                PhoneNumber = PhoneNumber,
                BirthDate = BirthDate,
                Gender = (Gender)this.Gender,
                Address = Address,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            await _patientRepository.AddAsync(patient);

            _logger.LogInformation("New patient registered: {Email}", Email);

            SuccessMessage = "Registration successful! Please login.";
            ModelState.Clear();

            return RedirectToPage("/Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during patient registration for {Email}", Email);
            ErrorMessage = "An error occurred during registration. Please try again.";
            return Page();
        }
    }
}
