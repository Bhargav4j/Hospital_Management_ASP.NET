using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages.Doctor;

public class EditModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IDoctorService doctorService, ILogger<EditModel> logger)
    {
        _doctorService = doctorService;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {
        public int DoctorId { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Required][EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Phone { get; set; } = string.Empty;
        [Required] public string Address { get; set; } = string.Empty;
        [Required] public string Specialization { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(id, cancellationToken);
        if (doctor == null) return RedirectToPage("/Doctor/Index");

        Input.DoctorId = doctor.DoctorID;
        Input.Name = doctor.Name;
        Input.Email = doctor.Email;
        Input.Phone = doctor.Phone;
        Input.Address = doctor.Address;
        Input.Specialization = doctor.Specialization;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            // Get the existing doctor to preserve fields not in the form
            var existingDoctor = await _doctorService.GetDoctorByIdAsync(Input.DoctorId, cancellationToken);
            if (existingDoctor == null)
            {
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToPage("/Doctor/Index");
            }

            var updateDto = new DoctorUpdateDto
            {
                DoctorID = Input.DoctorId,
                Name = Input.Name,
                Email = Input.Email,
                Phone = Input.Phone,
                Address = Input.Address,
                BirthDate = existingDoctor.BirthDate,
                Gender = existingDoctor.Gender,
                DeptNo = existingDoctor.DeptNo,
                Experience = existingDoctor.Experience,
                Salary = existingDoctor.Salary,
                ChargesPerVisit = existingDoctor.ChargesPerVisit,
                Specialization = Input.Specialization,
                Qualification = existingDoctor.Qualification,
                Status = existingDoctor.Status,
                IsActive = existingDoctor.IsActive
            };

            await _doctorService.UpdateDoctorAsync(updateDto, "Admin", cancellationToken);
            TempData["SuccessMessage"] = "Doctor updated successfully!";
            return RedirectToPage("/Doctor/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating doctor");
            ModelState.AddModelError(string.Empty, "Error updating doctor.");
            return Page();
        }
    }
}
