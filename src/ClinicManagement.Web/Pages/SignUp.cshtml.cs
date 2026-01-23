using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Services;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages;

public class SignUpPageModel : PageModel
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<SignUpPageModel> _logger;

    [BindProperty]
    public SignUpViewModel SignUpInput { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public SignUpPageModel(IAuthenticationService authService, ILogger<SignUpPageModel> logger)
    {
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
            var patientDto = new PatientCreateDto
            {
                Name = SignUpInput.Name,
                Email = SignUpInput.Email,
                Password = SignUpInput.Password,
                PhoneNumber = SignUpInput.PhoneNumber,
                DateOfBirth = SignUpInput.DateOfBirth,
                Gender = SignUpInput.Gender,
                Address = SignUpInput.Address
            };

            var result = await _authService.RegisterPatientAsync(patientDto);

            if (result.Success)
            {
                HttpContext.Session.SetInt32("UserId", result.UserId);
                HttpContext.Session.SetInt32("UserType", (int)UserType.Patient);

                _logger.LogInformation("New patient registered: {Email}", SignUpInput.Email);

                return RedirectToPage("/Patient/PatientHome");
            }
            else
            {
                ErrorMessage = result.Message;
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sign up error for email {Email}", SignUpInput.Email);
            ErrorMessage = "An error occurred during registration. Please try again.";
            return Page();
        }
    }
}
