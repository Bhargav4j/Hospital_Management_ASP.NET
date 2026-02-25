using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Admin.Doctors;

public class EditModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;
    private readonly IS3StorageService? _s3Service;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IDoctorService ds, IDepartmentService depts, IMapper m, ILogger<EditModel> logger, IS3StorageService? s3Service = null)
    {
        _doctorService = ds;
        _departmentService = depts;
        _mapper = m;
        _logger = logger;
        _s3Service = s3Service;
    }

    [BindProperty] public int DoctorID { get; set; }
    [BindProperty, Required] public string Name { get; set; } = string.Empty;
    [BindProperty] public string Phone { get; set; } = string.Empty;
    [BindProperty] public string Specialization { get; set; } = string.Empty;
    [BindProperty] public string Qualification { get; set; } = string.Empty;
    [BindProperty] public int DeptNo { get; set; }
    [BindProperty] public int Experience { get; set; }
    [BindProperty] public decimal ChargesPerVisit { get; set; }
    [BindProperty] public IFormFile? ProfileImage { get; set; }

    public string Email { get; set; } = string.Empty;
    public IEnumerable<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
    public string? ProfileImageUrl { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");

        var doctor = await _doctorService.GetDoctorByIdAsync(id);
        if (doctor == null) return RedirectToPage("/Admin/Doctors/Index");

        PopulateFields(doctor);
        Departments = _mapper.Map<IEnumerable<DepartmentDto>>(await _departmentService.GetAllDepartmentsAsync());
        await LoadProfileImageAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");

        var doctor = await _doctorService.GetDoctorByIdAsync(DoctorID);
        if (doctor == null) return RedirectToPage("/Admin/Doctors/Index");

        doctor.Name = Name;
        doctor.Phone = Phone;
        doctor.Specialization = Specialization;
        doctor.Qualification = Qualification;
        doctor.DeptNo = DeptNo;
        doctor.Experience = Experience;
        doctor.ChargesPerVisit = ChargesPerVisit;
        await _doctorService.UpdateDoctorAsync(doctor);

        if (ProfileImage != null && ProfileImage.Length > 0 && _s3Service != null)
        {
            await DeleteExistingPhotoAsync(DoctorID, "doctors");
            var ext = Path.GetExtension(ProfileImage.FileName);
            using var stream = ProfileImage.OpenReadStream();
            await _s3Service.UploadFileAsync(stream, $"profile{ext}", ProfileImage.ContentType, $"doctors/{DoctorID}");
            _logger.LogInformation("Admin uploaded profile image for doctor {Id}", DoctorID);
        }

        Email = doctor.Email;
        Departments = _mapper.Map<IEnumerable<DepartmentDto>>(await _departmentService.GetAllDepartmentsAsync());
        SuccessMessage = "Doctor updated successfully.";
        await LoadProfileImageAsync(DoctorID);
        return Page();
    }

    public async Task<IActionResult> OnPostDeletePhotoAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");

        await DeleteExistingPhotoAsync(DoctorID, "doctors");
        _logger.LogInformation("Admin deleted profile image for doctor {Id}", DoctorID);

        var doctor = await _doctorService.GetDoctorByIdAsync(DoctorID);
        if (doctor != null) PopulateFields(doctor);
        Departments = _mapper.Map<IEnumerable<DepartmentDto>>(await _departmentService.GetAllDepartmentsAsync());
        SuccessMessage = "Profile photo removed.";
        return Page();
    }

    private void PopulateFields(Domain.Entities.Doctor doctor)
    {
        DoctorID = doctor.DoctorID;
        Name = doctor.Name;
        Email = doctor.Email;
        Phone = doctor.Phone;
        Specialization = doctor.Specialization;
        Qualification = doctor.Qualification;
        DeptNo = doctor.DeptNo;
        Experience = doctor.Experience;
        ChargesPerVisit = doctor.ChargesPerVisit;
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
