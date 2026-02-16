using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Admin.Patients;

public class AdminPatientsIndexModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly IMapper _mapper;

    public AdminPatientsIndexModel(IPatientService patientService, IMapper mapper)
    {
        _patientService = patientService;
        _mapper = mapper;
    }

    public IEnumerable<PatientDto> Patients { get; set; } = new List<PatientDto>();

    public async Task<IActionResult> OnGetAsync()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "Admin")
        {
            return RedirectToPage("/Login");
        }

        var patients = await _patientService.GetAllPatientsAsync();
        Patients = _mapper.Map<IEnumerable<PatientDto>>(patients);

        return Page();
    }
}
