using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Admin.Staff;

public class AdminStaffIndexModel : PageModel
{
    public IActionResult OnGet() => HttpContext.Session.GetString("UserRole") != "Admin" ? RedirectToPage("/Login") : Page();
}
