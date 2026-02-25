using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Doctor;

public class ProfileModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly IS3StorageService? _s3Service;
    private readonly ILogger<ProfileModel> _logger;

    public ProfileModel(IDoctorService doctorService, ILogger<ProfileModel> logger, IS3StorageService? s3Service = null)
    {
        _doctorService = doctorService;
        _logger = logger;
        _s3Service = s3Service;
    }

    [BindProperty] public string Name { get; set; } = string.Empty;
    [BindProperty] public string Phone { get; set; } = string.Empty;
    [BindProperty] public string Address { get; set; } = string.Empty;
    [BindProperty] public string Specialization { get; set; } = string.Empty;
    [BindProperty] public string Qualification { get; set; } = string.Empty;
    [BindProperty] public IFormFile? ProfileImage { get; set; }

    public string Email { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int Experience { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var doctorId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(doctorId) || HttpContext.Session.GetString("UserRole") != "Doctor")
            return RedirectToPage("/Login");

        var doctor = await _doctorService.GetDoctorByIdAsync(int.Parse(doctorId));
        if (doctor == null) return RedirectToPage("/Login");

        PopulateFields(doctor);
        await LoadProfileImageAsync(int.Parse(doctorId));
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var doctorId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(doctorId) || HttpContext.Session.GetString("UserRole") != "Doctor")
            return RedirectToPage("/Login");

        var id = int.Parse(doctorId);
        var doctor = await _doctorService.GetDoctorByIdAsync(id);
        if (doctor == null) return RedirectToPage("/Login");

        doctor.Name = Name;
        doctor.Phone = Phone;
        doctor.Address = Address;
        doctor.Specialization = Specialization;
        doctor.Qualification = Qualification;
        await _doctorService.UpdateDoctorAsync(doctor);

        if (ProfileImage != null && ProfileImage.Length > 0 && _s3Service != null)
        {
            await DeleteExistingPhotoAsync(id, "doctors");
            var ext = Path.GetExtension(ProfileImage.FileName);
            using var stream = ProfileImage.OpenReadStream();
            await _s3Service.UploadFileAsync(stream, $"profile{ext}", ProfileImage.ContentType, $"doctors/{id}");
            _logger.LogInformation("Doctor {Id} profile image uploaded", id);
        }

        PopulateFields(doctor);
        SuccessMessage = "Profile updated successfully.";
        await LoadProfileImageAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostDeletePhotoAsync()
    {
        var doctorId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(doctorId) || HttpContext.Session.GetString("UserRole") != "Doctor")
            return RedirectToPage("/Login");

        var id = int.Parse(doctorId);
        await DeleteExistingPhotoAsync(id, "doctors");
        _logger.LogInformation("Doctor {Id} profile image deleted", id);

        var doctor = await _doctorService.GetDoctorByIdAsync(id);
        if (doctor != null) PopulateFields(doctor);
        SuccessMessage = "Profile photo removed.";
        return Page();
    }

    private void PopulateFields(Domain.Entities.Doctor doctor)
    {
        Name = doctor.Name;
        Email = doctor.Email;
        Phone = doctor.Phone;
        Address = doctor.Address;
        Specialization = doctor.Specialization;
        Qualification = doctor.Qualification;
        Experience = doctor.Experience;
    }

    private async Task LoadProfileImageAsync(int id)
    {
        if (_s3Service == null) return;
        var files = await _s3Service.ListFilesAsync($"doctors/{id}/profile");
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
