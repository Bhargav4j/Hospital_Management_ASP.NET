using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages;

public class LoginModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly IDoctorService _doctorService;

    public LoginModel(IPatientService patientService, IDoctorService doctorService)
    {
        _patientService = patientService;
        _doctorService = doctorService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "User type is required")]
    public string UserType { get; set; } = string.Empty;

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
            if (UserType == "Patient")
            {
                var patient = await _patientService.ValidateLoginAsync(Email, Password);
                if (patient != null)
                {
                    HttpContext.Session.SetString("UserId", patient.PatientID.ToString());
                    HttpContext.Session.SetString("UserName", patient.Name);
                    HttpContext.Session.SetString("UserRole", "Patient");
                    return RedirectToPage("/Patient/Index");
                }
            }
            else if (UserType == "Doctor")
            {
                var doctor = await _doctorService.ValidateLoginAsync(Email, Password);
                if (doctor != null)
                {
                    HttpContext.Session.SetString("UserId", doctor.DoctorID.ToString());
                    HttpContext.Session.SetString("UserName", doctor.Name);
                    HttpContext.Session.SetString("UserRole", "Doctor");
                    return RedirectToPage("/Doctor/Index");
                }
            }
            else if (UserType == "Admin")
            {
                // Simple admin check - in production, use proper admin authentication
                if (Email == "admin@clinic.com" && Password == "admin123")
                {
                    HttpContext.Session.SetString("UserId", "1");
                    HttpContext.Session.SetString("UserName", "Administrator");
                    HttpContext.Session.SetString("UserRole", "Admin");
                    return RedirectToPage("/Admin/Index");
                }
            }

            ErrorMessage = "Invalid email or password";
            return Page();
        }
        catch (Exception ex)
        {
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
