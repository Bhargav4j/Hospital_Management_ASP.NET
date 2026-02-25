using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Admin.Patients;

public class EditModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly IS3StorageService? _s3Service;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IPatientService patientService, ILogger<EditModel> logger, IS3StorageService? s3Service = null)
    {
        _patientService = patientService;
        _logger = logger;
        _s3Service = s3Service;
    }

    [BindProperty] public int PatientID { get; set; }
    [BindProperty, Required] public string Name { get; set; } = string.Empty;
    [BindProperty] public string Phone { get; set; } = string.Empty;
    [BindProperty] public string Address { get; set; } = string.Empty;
    [BindProperty, Required] public DateTime BirthDate { get; set; }
    [BindProperty, Required] public string Gender { get; set; } = string.Empty;
    [BindProperty] public IFormFile? ProfileImage { get; set; }

    public string Email { get; set; } = string.Empty;
    public string? ProfileImageUrl { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");

        var patient = await _patientService.GetPatientByIdAsync(id);
        if (patient == null) return RedirectToPage("/Admin/Patients/Index");

        PopulateFields(patient);
        await LoadProfileImageAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");
        if (!ModelState.IsValid) { await LoadProfileImageAsync(PatientID); return Page(); }

        var patient = await _patientService.GetPatientByIdAsync(PatientID);
        if (patient == null) return RedirectToPage("/Admin/Patients/Index");

        patient.Name = Name;
        patient.Phone = Phone;
        patient.Address = Address;
        patient.BirthDate = BirthDate;
        patient.Gender = Gender;
        await _patientService.UpdatePatientAsync(patient);

        if (ProfileImage != null && ProfileImage.Length > 0 && _s3Service != null)
        {
            await DeleteExistingPhotoAsync(PatientID, "patients");
            var ext = Path.GetExtension(ProfileImage.FileName);
            using var stream = ProfileImage.OpenReadStream();
            await _s3Service.UploadFileAsync(stream, $"profile{ext}", ProfileImage.ContentType, $"patients/{PatientID}");
            _logger.LogInformation("Admin uploaded profile image for patient {Id}", PatientID);
        }

        Email = patient.Email;
        SuccessMessage = "Patient updated successfully.";
        await LoadProfileImageAsync(PatientID);
        return Page();
    }

    public async Task<IActionResult> OnPostDeletePhotoAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");

        await DeleteExistingPhotoAsync(PatientID, "patients");
        _logger.LogInformation("Admin deleted profile image for patient {Id}", PatientID);

        var patient = await _patientService.GetPatientByIdAsync(PatientID);
        if (patient != null) PopulateFields(patient);
        SuccessMessage = "Profile photo removed.";
        return Page();
    }

    private void PopulateFields(Domain.Entities.Patient patient)
    {
        PatientID = patient.PatientID;
        Name = patient.Name;
        Email = patient.Email;
        Phone = patient.Phone;
        Address = patient.Address;
        BirthDate = patient.BirthDate;
        Gender = patient.Gender;
    }

    private async Task LoadProfileImageAsync(int id)
    {
        if (_s3Service == null) return;
        var files = await _s3Service.ListFilesAsync($"patients/{id}/profile");
        var imageKey = files.FirstOrDefault();
        if (!string.IsNullOrEmpty(imageKey))
            ProfileImageUrl = await _s3Service.GetPreSignedUrlAsync(imageKey, 60);
    }

    private async Task DeleteExistingPhotoAsync(int id, string folder)
    {
        if (_s3Service == null) return;
        var existing = await _s3Service.ListFilesAsync($"{folder}/{id}/profile");
        foreach (var key in existing)
            await _s3Service.DeleteFileAsync(key);
    }
}
