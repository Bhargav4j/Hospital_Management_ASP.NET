using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Domain.Interfaces.Services;
using HospitalManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Account;

public class SignUpModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<SignUpModel> _logger;

    [BindProperty]
    public SignUpViewModel SignUpViewModel { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public SignUpModel(IUserService userService, ILogger<SignUpModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            if (await _userService.EmailExistsAsync(SignUpViewModel.Email))
            {
                ErrorMessage = "Email already exists. Please choose a different one.";
                return Page();
            }

            var userCreateDto = new UserCreateDto
            {
                Name = SignUpViewModel.Name,
                BirthDate = SignUpViewModel.BirthDate,
                Email = SignUpViewModel.Email,
                Password = SignUpViewModel.Password,
                PhoneNo = SignUpViewModel.PhoneNo,
                Gender = SignUpViewModel.Gender,
                Address = SignUpViewModel.Address,
                UserType = UserType.Patient
            };

            var createdUser = await _userService.CreateAsync(userCreateDto);

            HttpContext.Session.SetInt32("UserId", createdUser.Id);
            HttpContext.Session.SetInt32("UserType", (int)UserType.Patient);

            return RedirectToPage("/Patient/Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sign up");
            ErrorMessage = "An error occurred. Please try again.";
            return Page();
        }
    }
}
