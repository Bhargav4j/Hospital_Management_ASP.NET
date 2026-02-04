using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages;

/// <summary>
/// Page model for user logout
/// </summary>
public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogoutModel"/> class
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public LogoutModel(ILogger<LogoutModel> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles GET requests for logout
    /// </summary>
    /// <returns>Action result</returns>
    public IActionResult OnGet()
    {
        try
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            _logger.LogInformation("User logging out: UserId={UserId}, Role={Role}", userId, userRole);

            // Clear all session data
            HttpContext.Session.Clear();

            TempData["SuccessMessage"] = "You have been logged out successfully.";

            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during logout");
            return RedirectToPage("/Index");
        }
    }
}
