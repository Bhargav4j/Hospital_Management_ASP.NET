using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Domain.Interfaces.Services;
using HospitalManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    [BindProperty]
    public LoginViewModel LoginViewModel { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public void OnGet()
    {
        HttpContext.Session.Clear();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var loginDto = new LoginDto
            {
                Email = LoginViewModel.Email,
                Password = LoginViewModel.Password
            };

            var result = await _userService.ValidateLoginAsync(loginDto);

            if (!result.Success)
            {
                ErrorMessage = result.Message;
                return Page();
            }

            HttpContext.Session.SetInt32("UserId", result.UserId);
            HttpContext.Session.SetInt32("UserType", (int)result.UserType);

            return result.UserType switch
            {
                UserType.Patient => RedirectToPage("/Patient/Home"),
                UserType.Doctor => RedirectToPage("/Doctor/Home"),
                UserType.Admin => RedirectToPage("/Admin/Home"),
                _ => RedirectToPage("/Index")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            ErrorMessage = "An error occurred. Please try again.";
            return Page();
        }
    }
}
