using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Admin.Doctors;

public class CreateDoctorModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;
    private readonly IS3StorageService? _s3Service;
    private readonly ILogger<CreateDoctorModel> _logger;

    public CreateDoctorModel(
        IDoctorService ds,
        IDepartmentService depts,
        IMapper m,
        ILogger<CreateDoctorModel> logger,
        IS3StorageService? s3Service = null)
    {
        _doctorService = ds;
        _departmentService = depts;
        _mapper = m;
        _logger = logger;
        _s3Service = s3Service;
    }

    [BindProperty, Required] public string Name { get; set; } = string.Empty;
    [BindProperty, Required] public string Email { get; set; } = string.Empty;
    [BindProperty, Required] public string Password { get; set; } = string.Empty;
    [BindProperty] public string Phone { get; set; } = string.Empty;
    [BindProperty] public string Specialization { get; set; } = string.Empty;
    [BindProperty] public int DeptNo { get; set; }
    [BindProperty] public int Experience { get; set; }
    [BindProperty] public decimal ChargesPerVisit { get; set; }
    [BindProperty] public IFormFile? ProfileImage { get; set; }

    public IEnumerable<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");
        Departments = _mapper.Map<IEnumerable<DepartmentDto>>(await _departmentService.GetAllDepartmentsAsync());
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");

        var doctor = await _doctorService.CreateDoctorAsync(new Domain.Entities.Doctor
        {
            Name = Name, Email = Email, Password = Password, Phone = Phone,
            Specialization = Specialization, DeptNo = DeptNo, Experience = Experience,
            ChargesPerVisit = ChargesPerVisit, Address = "", BirthDate = DateTime.Today,
            Gender = "Male", Qualification = "", Salary = 0
        });

        if (ProfileImage != null && ProfileImage.Length > 0 && _s3Service != null)
        {
            var ext = Path.GetExtension(ProfileImage.FileName);
            using var stream = ProfileImage.OpenReadStream();
            var key = await _s3Service.UploadFileAsync(stream, $"profile{ext}", ProfileImage.ContentType, $"doctors/{doctor.DoctorID}");
            _logger.LogInformation("Doctor {Id} profile image uploaded to S3: {Key}", doctor.DoctorID, key);
        }

        return RedirectToPage("/Admin/Doctors/Index");
    }
}
