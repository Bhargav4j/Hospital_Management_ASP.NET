using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages;

public class LoginModel : PageModel
{
    private readonly IAuthenticationService _authenticationService;

    public LoginModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [BindProperty]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var (success, userId, userType, message) = await _authenticationService.LoginAsync(Email, Password);

        if (!success)
        {
            ErrorMessage = message;
            return Page();
        }

        HttpContext.Session.SetInt32("UserId", userId);
        HttpContext.Session.SetInt32("UserType", userType);

        return userType switch
        {
            1 => RedirectToPage("/Patient/Index"),
            2 => RedirectToPage("/Doctor/Index"),
            3 => RedirectToPage("/Admin/Index"),
            _ => RedirectToPage("/Index")
        };
    }
}
