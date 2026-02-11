using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Patient;

public class HomeModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<HomeModel> _logger;

    public string PatientName { get; set; } = string.Empty;

    public HomeModel(IUserService userService, ILogger<HomeModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToPage("/Account/Login");
        }

        try
        {
            var user = await _userService.GetByIdAsync(userId.Value);
            if (user != null)
            {
                PatientName = user.Name;
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading patient home page");
            return RedirectToPage("/Account/Login");
        }
    }
}
