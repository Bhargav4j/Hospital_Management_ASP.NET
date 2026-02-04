using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages;

public class LoginModel : PageModel
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IAuthenticationService authService, ILogger<LoginModel> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [BindProperty]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

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
            var (isValid, userType, userId) = await _authService.ValidateLoginAsync(Email, Password);

            if (isValid && userType.HasValue && userId.HasValue)
            {
                HttpContext.Session.SetInt32("UserId", userId.Value);
                HttpContext.Session.SetInt32("UserType", (int)userType.Value);
                HttpContext.Session.SetString("UserEmail", Email);

                _logger.LogInformation("User {Email} logged in successfully as {UserType}", Email, userType.Value);

                return userType.Value switch
                {
                    UserType.Patient => RedirectToPage("/Patients/Index"),
                    UserType.Doctor => RedirectToPage("/Doctors/Index"),
                    UserType.Admin => RedirectToPage("/Admin/Index"),
                    UserType.Staff => RedirectToPage("/Admin/Index"),
                    _ => RedirectToPage("/Index")
                };
            }

            ErrorMessage = "Invalid email or password";
            _logger.LogWarning("Failed login attempt for {Email}", Email);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}", Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
