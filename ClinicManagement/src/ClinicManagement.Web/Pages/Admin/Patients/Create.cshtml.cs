using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Admin.Patients;

public class CreatePatientModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly IS3StorageService? _s3Service;
    private readonly ILogger<CreatePatientModel> _logger;

    public CreatePatientModel(
        IPatientService patientService,
        ILogger<CreatePatientModel> logger,
        IS3StorageService? s3Service = null)
    {
        _patientService = patientService;
        _logger = logger;
        _s3Service = s3Service;
    }

    [BindProperty, Required] public string Name { get; set; } = string.Empty;
    [BindProperty, Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [BindProperty, Required] public string Password { get; set; } = string.Empty;
    [BindProperty] public string Phone { get; set; } = string.Empty;
    [BindProperty] public string Address { get; set; } = string.Empty;
    [BindProperty, Required] public DateTime BirthDate { get; set; } = DateTime.Today.AddYears(-20);
    [BindProperty, Required] public string Gender { get; set; } = string.Empty;
    [BindProperty] public IFormFile? ProfileImage { get; set; }

    public IActionResult OnGet()
    {
        return HttpContext.Session.GetString("UserRole") != "Admin" ? RedirectToPage("/Login") : Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");
        if (!ModelState.IsValid) return Page();

        var patient = await _patientService.CreatePatientAsync(new Domain.Entities.Patient
        {
            Name = Name, Email = Email, Password = Password, Phone = Phone,
            Address = Address, BirthDate = BirthDate, Gender = Gender
        });

        if (ProfileImage != null && ProfileImage.Length > 0 && _s3Service != null)
        {
            var ext = Path.GetExtension(ProfileImage.FileName);
            using var stream = ProfileImage.OpenReadStream();
            var key = await _s3Service.UploadFileAsync(stream, $"profile{ext}", ProfileImage.ContentType, $"patients/{patient.PatientID}");
            _logger.LogInformation("Patient {Id} profile image uploaded to S3: {Key}", patient.PatientID, key);
        }

        return RedirectToPage("/Admin/Patients/Index");
    }
}
