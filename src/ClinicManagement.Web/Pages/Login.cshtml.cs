using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Services;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages;

public class LoginPageModel : PageModel
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<LoginPageModel> _logger;

    [BindProperty]
    public LoginViewModel LoginInput { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public LoginPageModel(IAuthenticationService authService, ILogger<LoginPageModel> logger)
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
            var loginDto = new LoginDto
            {
                Email = LoginInput.Email,
                Password = LoginInput.Password
            };

            var result = await _authService.LoginAsync(loginDto);

            if (result.Success)
            {
                HttpContext.Session.SetInt32("UserId", result.UserId);
                HttpContext.Session.SetInt32("UserType", (int)result.UserType);

                _logger.LogInformation("User logged in: {Email}, Type: {UserType}", LoginInput.Email, result.UserType);

                return result.UserType switch
                {
                    UserType.Patient => RedirectToPage("/Patient/PatientHome"),
                    UserType.Doctor => RedirectToPage("/Doctor/DoctorHome"),
                    UserType.Admin => RedirectToPage("/Admin/AdminHome"),
                    _ => Page()
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
            _logger.LogError(ex, "Login error for email {Email}", LoginInput.Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
