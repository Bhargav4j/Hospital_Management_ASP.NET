using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Admin;

public class AdminHomeModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<AdminHomeModel> _logger;

    public string AdminName { get; set; } = string.Empty;

    public AdminHomeModel(IUserService userService, ILogger<AdminHomeModel> logger)
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
                AdminName = user.Name;
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin home page");
            return RedirectToPage("/Account/Login");
        }
    }
}
