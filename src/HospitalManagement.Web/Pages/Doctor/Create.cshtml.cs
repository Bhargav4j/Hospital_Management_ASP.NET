using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages.Doctor;

/// <summary>
/// Page model for creating a new doctor
/// </summary>
public class CreateModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(
        IDoctorService doctorService,
        IDepartmentService departmentService,
        ILogger<CreateModel> logger)
    {
        _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public SelectList DepartmentList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    public class InputModel
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required][EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Phone { get; set; } = string.Empty;
        [Required] public string Address { get; set; } = string.Empty;
        [Required] public DateTime BirthDate { get; set; }
        [Required] public string Gender { get; set; } = string.Empty;
        [Required] public int DeptNo { get; set; }
        [Required] public int Experience { get; set; }
        [Required] public decimal Salary { get; set; }
        [Required] public decimal ChargesPerVisit { get; set; }
        [Required] public string Specialization { get; set; } = string.Empty;
        [Required] public string Qualification { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                TempData["ErrorMessage"] = "Unauthorized access.";
                return RedirectToPage("/Index");
            }

            var departments = await _departmentService.GetAllDepartmentsAsync(cancellationToken);
            DepartmentList = new SelectList(departments, "DeptNo", "DeptName");

            Input.BirthDate = DateTime.Today.AddYears(-30);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create doctor page");
            TempData["ErrorMessage"] = "An error occurred.";
            return RedirectToPage("/Doctor/Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var departments = await _departmentService.GetAllDepartmentsAsync(cancellationToken);
            DepartmentList = new SelectList(departments, "DeptNo", "DeptName");
            return Page();
        }

        try
        {
            var doctorCreateDto = new DoctorCreateDto
            {
                Name = Input.Name,
                Email = Input.Email,
                Password = "Doctor@123",
                Phone = Input.Phone,
                Address = Input.Address,
                BirthDate = Input.BirthDate,
                Gender = Input.Gender,
                DeptNo = Input.DeptNo,
                Experience = Input.Experience,
                Salary = Input.Salary,
                ChargesPerVisit = Input.ChargesPerVisit,
                Specialization = Input.Specialization,
                Qualification = Input.Qualification
            };

            var createdDoctor = await _doctorService.CreateDoctorAsync(doctorCreateDto, "Admin", cancellationToken);

            if (createdDoctor != null)
            {
                TempData["SuccessMessage"] = "Doctor created successfully!";
                return RedirectToPage("/Doctor/Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to create doctor.");
            var departments = await _departmentService.GetAllDepartmentsAsync(cancellationToken);
            DepartmentList = new SelectList(departments, "DeptNo", "DeptName");
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating doctor");
            ModelState.AddModelError(string.Empty, "An error occurred while creating doctor.");
            var departments = await _departmentService.GetAllDepartmentsAsync(cancellationToken);
            DepartmentList = new SelectList(departments, "DeptNo", "DeptName");
            return Page();
        }
    }
}
