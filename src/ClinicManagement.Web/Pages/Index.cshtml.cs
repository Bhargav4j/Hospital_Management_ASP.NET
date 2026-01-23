using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClinicManagement.Domain.Interfaces.Services;

namespace ClinicManagement.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IAuthenticationService _authService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IAuthenticationService authService, ILogger<IndexModel> logger)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }

    public void OnGet()
    {
        HttpContext.Session.Clear();
    }

    public async Task<IActionResult> OnPostLoginAsync(string loginEmail, string loginPassword)
    {
        try
        {
            var result = await _authService.ValidateLoginAsync(loginEmail, loginPassword);

            if (result.Success)
            {
                HttpContext.Session.SetInt32("UserId", result.UserId);
                HttpContext.Session.SetString("UserType", result.UserType);

                return result.UserType switch
                {
                    "Patient" => RedirectToPage("/Patient/PatientHome"),
                    "Doctor" => RedirectToPage("/Doctor/DoctorHome"),
                    "Admin" => RedirectToPage("/Admin/AdminHome"),
                    _ => Page()
                };
            }

            Message = result.Message;
            IsSuccess = false;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            Message = "An error occurred during login. Please try again.";
            IsSuccess = false;
            return Page();
        }
    }

    public async Task<IActionResult> OnPostSignUpAsync(
        string sName,
        string sBirthDate,
        string sEmail,
        string sPassword,
        string Phone,
        string Gender,
        string Address)
    {
        try
        {
            if (!DateTime.TryParse(sBirthDate, out var birthDate))
            {
                Message = "Invalid birth date format.";
                IsSuccess = false;
                return Page();
            }

            var result = await _authService.RegisterPatientAsync(
                sName,
                birthDate,
                sEmail,
                sPassword,
                Phone,
                Gender,
                Address);

            if (result.Success)
            {
                HttpContext.Session.SetInt32("UserId", result.UserId);
                HttpContext.Session.SetString("UserType", "Patient");
                return RedirectToPage("/Patient/PatientHome");
            }

            Message = result.Message;
            IsSuccess = false;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sign up");
            Message = "An error occurred during registration. Please try again.";
            IsSuccess = false;
            return Page();
        }
    }
}
