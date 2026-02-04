using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Staff;

public class IndexModel : PageModel
{
    private readonly IOtherStaffService _staffService;

    public IndexModel(IOtherStaffService staffService)
    {
        _staffService = staffService;
    }

    public IEnumerable<OtherStaffDto> StaffList { get; set; } = new List<OtherStaffDto>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "Admin")
        {
            TempData["ErrorMessage"] = "Unauthorized access.";
            return RedirectToPage("/Index");
        }

        StaffList = await _staffService.GetAllStaffAsync(cancellationToken);
        return Page();
    }
}
