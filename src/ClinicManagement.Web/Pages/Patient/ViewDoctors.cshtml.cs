using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Patient;

public class ViewDoctorsModel : PageModel
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ILogger<ViewDoctorsModel> _logger;

    public List<DoctorDto> Doctors { get; set; } = new();

    public ViewDoctorsModel(IDoctorRepository doctorRepository, ILogger<ViewDoctorsModel> logger)
    {
        _doctorRepository = doctorRepository;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToPage("/Login");
        }

        try
        {
            var doctors = await _doctorRepository.GetAllAsync();
            Doctors = doctors.Select(d => new DoctorDto
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,
                Gender = d.Gender,
                Specialization = d.Specialization,
                Qualification = d.Qualification,
                ConsultationFee = d.ConsultationFee,
                IsActive = d.IsActive
            }).ToList();

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading doctors");
            return Page();
        }
    }
}
