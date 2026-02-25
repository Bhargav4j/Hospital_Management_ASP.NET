using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Patient;

public class ProfileModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly IS3StorageService? _s3Service;
    private readonly ILogger<ProfileModel> _logger;

    public ProfileModel(IPatientService patientService, ILogger<ProfileModel> logger, IS3StorageService? s3Service = null)
    {
        _patientService = patientService;
        _logger = logger;
        _s3Service = s3Service;
    }

    [BindProperty] public string Name { get; set; } = string.Empty;
    [BindProperty] public string Phone { get; set; } = string.Empty;
    [BindProperty] public string Address { get; set; } = string.Empty;
    [BindProperty] public DateTime BirthDate { get; set; }
    [BindProperty] public string Gender { get; set; } = string.Empty;
    [BindProperty] public IFormFile? ProfileImage { get; set; }

    public string? ProfileImageUrl { get; set; }
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public string Email { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        var patientId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(patientId) || HttpContext.Session.GetString("UserRole") != "Patient")
            return RedirectToPage("/Login");

        var patient = await _patientService.GetPatientByIdAsync(int.Parse(patientId));
        if (patient == null) return RedirectToPage("/Login");

        Name = patient.Name;
        Email = patient.Email;
        Phone = patient.Phone;
        Address = patient.Address;
        BirthDate = patient.BirthDate;
        Gender = patient.Gender;

        await LoadProfileImageAsync(int.Parse(patientId));
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var patientId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(patientId) || HttpContext.Session.GetString("UserRole") != "Patient")
            return RedirectToPage("/Login");

        var id = int.Parse(patientId);
        var patient = await _patientService.GetPatientByIdAsync(id);
        if (patient == null) return RedirectToPage("/Login");

        patient.Name = Name;
        patient.Phone = Phone;
        patient.Address = Address;
        patient.BirthDate = BirthDate;
        patient.Gender = Gender;
        await _patientService.UpdatePatientAsync(patient);

        if (ProfileImage != null && ProfileImage.Length > 0 && _s3Service != null)
        {
            await DeleteExistingPhotoAsync(id, "patients");
            var ext = Path.GetExtension(ProfileImage.FileName);
            var key = $"patients/{id}/profile{ext}";
            using var stream = ProfileImage.OpenReadStream();
            await _s3Service.UploadFileAsync(stream, $"profile{ext}", ProfileImage.ContentType, $"patients/{id}");
            _logger.LogInformation("Patient {Id} profile image uploaded: {Key}", id, key);
        }

        Email = patient.Email;
        SuccessMessage = "Profile updated successfully.";
        await LoadProfileImageAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostDeletePhotoAsync()
    {
        var patientId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(patientId) || HttpContext.Session.GetString("UserRole") != "Patient")
            return RedirectToPage("/Login");

        var id = int.Parse(patientId);
        await DeleteExistingPhotoAsync(id, "patients");
        _logger.LogInformation("Patient {Id} profile image deleted", id);

        var patient = await _patientService.GetPatientByIdAsync(id);
        if (patient != null)
        {
            Name = patient.Name;
            Email = patient.Email;
            Phone = patient.Phone;
            Address = patient.Address;
            BirthDate = patient.BirthDate;
            Gender = patient.Gender;
        }
        SuccessMessage = "Profile photo removed.";
        return Page();
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
