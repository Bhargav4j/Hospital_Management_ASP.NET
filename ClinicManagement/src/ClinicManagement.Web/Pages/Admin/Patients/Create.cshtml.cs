using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Admin.Patients;

public class CreatePatientModel : PageModel
{
    private readonly IPatientService _patientService;

    public CreatePatientModel(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [BindProperty, Required]
    public string Name { get; set; } = string.Empty;

    [BindProperty, Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty, Required]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public string Phone { get; set; } = string.Empty;

    [BindProperty]
    public string Address { get; set; } = string.Empty;

    [BindProperty, Required]
    public DateTime BirthDate { get; set; } = DateTime.Today.AddYears(-20);

    [BindProperty, Required]
    public string Gender { get; set; } = string.Empty;

    public IActionResult OnGet()
    {
        return HttpContext.Session.GetString("UserRole") != "Admin" ? RedirectToPage("/Login") : Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");
        if (!ModelState.IsValid) return Page();

        await _patientService.CreatePatientAsync(new Domain.Entities.Patient
        {
            Name = Name, Email = Email, Password = Password, Phone = Phone,
            Address = Address, BirthDate = BirthDate, Gender = Gender
        });
        return RedirectToPage("/Admin/Patients/Index");
    }
}
