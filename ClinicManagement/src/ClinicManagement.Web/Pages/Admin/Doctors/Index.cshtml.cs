using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Admin.Doctors;

public class AdminDoctorsIndexModel : PageModel
{
    private readonly IDoctorService _service;
    private readonly IMapper _mapper;
    public AdminDoctorsIndexModel(IDoctorService service, IMapper mapper) { _service = service; _mapper = mapper; }
    public IEnumerable<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();
    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");
        Doctors = _mapper.Map<IEnumerable<DoctorDto>>(await _service.GetAllDoctorsAsync());
        return Page();
    }
}
