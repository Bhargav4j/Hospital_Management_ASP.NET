using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HospitalManagement.Domain.Interfaces.Services;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages;

public class LoginModel : PageModel
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<LoginModel> _logger;

    [BindProperty]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    [DataType(DataType.Password)]
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
            var (success, userId, userType) = await _authService.ValidateLoginAsync(Email, Password);

            if (success)
            {
                HttpContext.Session.SetInt32("UserId", userId);
                HttpContext.Session.SetString("UserType", userType);

                _logger.LogInformation("User {UserId} logged in as {UserType}", userId, userType);

                if (userType == "Patient")
                {
                    return RedirectToPage("/Patients/Dashboard");
                }
                else if (userType == "Doctor")
                {
                    return RedirectToPage("/Doctors/Dashboard");
                }
            }

            ErrorMessage = "Invalid email or password";
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
