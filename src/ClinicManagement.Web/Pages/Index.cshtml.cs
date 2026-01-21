using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IUserService userService, ILogger<IndexModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public LoginViewModel LoginModel { get; set; } = new();

    [BindProperty]
    public RegisterViewModel RegisterModel { get; set; } = new();

    public string? LoginErrorMessage { get; set; }
    public string? RegisterErrorMessage { get; set; }

    public void OnGet()
    {
        HttpContext.Session.Clear();
    }

    public async Task<IActionResult> OnPostLoginAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var loginDto = new LoginDto
            {
                Email = LoginModel.Email,
                Password = LoginModel.Password
            };

            var result = await _userService.LoginAsync(loginDto);

            if (result.Success)
            {
                HttpContext.Session.SetInt32("UserId", result.UserId);
                HttpContext.Session.SetInt32("UserType", result.UserType);

                return result.UserType switch
                {
                    1 => RedirectToPage("/Patient/Home"),
                    2 => RedirectToPage("/Doctor/Home"),
                    3 => RedirectToPage("/Admin/Home"),
                    _ => Page()
                };
            }

            LoginErrorMessage = result.Message;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            LoginErrorMessage = "An error occurred. Please try again.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostRegisterAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var createDto = new UserCreateDto
            {
                Name = RegisterModel.Name,
                Email = RegisterModel.Email,
                Password = RegisterModel.Password,
                PhoneNo = RegisterModel.PhoneNo,
                Gender = RegisterModel.Gender,
                Address = RegisterModel.Address,
                BirthDate = RegisterModel.BirthDate,
                UserType = 1
            };

            var user = await _userService.CreateAsync(createDto);

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetInt32("UserType", user.UserType);

            return RedirectToPage("/Patient/Home");
        }
        catch (InvalidOperationException ex)
        {
            RegisterErrorMessage = ex.Message;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            RegisterErrorMessage = "An error occurred. Please try again.";
            return Page();
        }
    }
}

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterViewModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string PhoneNo { get; set; } = string.Empty;

    [Required]
    public string Gender { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Required]
    public DateTime BirthDate { get; set; }
}
