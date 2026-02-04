using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Doctor;

public class DeleteModel : PageModel
{
    private readonly IDoctorService _doctorService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IDoctorService doctorService, ILogger<DeleteModel> logger)
    {
        _doctorService = doctorService;
        _logger = logger;
    }

    public DoctorDto? Doctor { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        Doctor = await _doctorService.GetDoctorByIdAsync(id, cancellationToken);
        if (Doctor == null) return RedirectToPage("/Doctor/Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _doctorService.DeleteDoctorAsync(id, cancellationToken);
            TempData["SuccessMessage"] = "Doctor deleted successfully!";
            return RedirectToPage("/Doctor/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting doctor");
            TempData["ErrorMessage"] = "Error deleting doctor.";
            return RedirectToPage("/Doctor/Index");
        }
    }
}
