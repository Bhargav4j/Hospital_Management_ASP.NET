using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages;

public class RegisterModel : PageModel
{
    private readonly IAuthenticationService _authenticationService;

    public RegisterModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [BindProperty]
    [Required]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public int UserType { get; set; } = 1;

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var (success, message) = await _authenticationService.RegisterAsync(Name, Email, Password, UserType);

        if (!success)
        {
            ErrorMessage = message;
            return Page();
        }

        SuccessMessage = message;
        return RedirectToPage("/Login");
    }
}
