using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages;

public class LoginModel : PageModel
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<LoginModel> _logger;

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public LoginModel(IAuthenticationService authService, ILogger<LoginModel> logger)
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
            var result = await _authService.ValidateLoginAsync(Email, Password);

            if (result.Success)
            {
                HttpContext.Session.SetInt32("UserId", result.UserId);
                HttpContext.Session.SetInt32("UserType", (int)result.UserType);
                HttpContext.Session.SetString("UserEmail", Email);

                _logger.LogInformation("User logged in successfully: {Email}", Email);

                // Redirect based on user type
                return result.UserType switch
                {
                    Domain.Enums.UserType.Patient => RedirectToPage("/Patients/Dashboard"),
                    Domain.Enums.UserType.Doctor => RedirectToPage("/Doctors/Dashboard"),
                    Domain.Enums.UserType.Admin => RedirectToPage("/Admin/Dashboard"),
                    _ => RedirectToPage("/Index")
                };
            }
            else
            {
                ErrorMessage = result.Message;
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
