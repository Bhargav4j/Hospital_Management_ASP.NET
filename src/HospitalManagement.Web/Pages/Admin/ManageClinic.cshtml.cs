using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Admin;

public class ManageClinicModel : PageModel
{
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
}
