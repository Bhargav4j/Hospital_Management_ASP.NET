using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Doctor;

public class DoctorHomeModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<DoctorHomeModel> _logger;

    public string DoctorName { get; set; } = string.Empty;

    public DoctorHomeModel(IUserService userService, ILogger<DoctorHomeModel> logger)
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
                DoctorName = user.Name;
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading doctor home page");
            return RedirectToPage("/Account/Login");
        }
    }
}
