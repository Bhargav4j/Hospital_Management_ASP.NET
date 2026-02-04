using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages.Staff;

public class CreateModel : PageModel
{
    private readonly IOtherStaffService _staffService;

    public CreateModel(IOtherStaffService staffService)
    {
        _staffService = staffService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Phone { get; set; } = string.Empty;
        [Required] public string Address { get; set; } = string.Empty;
        [Required] public string Designation { get; set; } = string.Empty;
        [Required] public string Qualification { get; set; } = string.Empty;
    }

    public IActionResult OnGet()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "Admin")
        {
            TempData["ErrorMessage"] = "Unauthorized access.";
            return RedirectToPage("/Index");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return Page();

        TempData["SuccessMessage"] = "Staff member added successfully!";
        return RedirectToPage("/Staff/Index");
    }
}
