using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Patient;

public class ViewDoctorsModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;

    public ViewDoctorsModel(IDoctorService doctorService, IMapper mapper)
    {
        _doctorService = doctorService;
        _mapper = mapper;
    }

    public IEnumerable<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();
    
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Login");
        }

        if (!string.IsNullOrEmpty(SearchTerm))
        {
            var doctors = await _doctorService.SearchDoctorsAsync(SearchTerm);
            Doctors = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
        else
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();
            Doctors = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }

        return Page();
    }
}
