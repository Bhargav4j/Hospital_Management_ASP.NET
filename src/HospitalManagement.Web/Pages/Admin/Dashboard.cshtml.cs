using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Admin;

public class DashboardModel : PageModel
{
    private readonly ILogger<DashboardModel> _logger;

    public DashboardModel(ILogger<DashboardModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToPage("/Login");
        }

        return Page();
    }
}
