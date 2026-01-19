using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagement.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthService authService, ILogger<AccountController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return View(loginDto);
        }

        try
        {
            var result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(loginDto);
            }

            HttpContext.Session.SetInt32("UserId", result.UserId);
            HttpContext.Session.SetInt32("UserType", result.UserType);
            HttpContext.Session.SetString("Token", result.Token);

            return result.UserType switch
            {
                1 => RedirectToAction("Index", "Admin"),
                2 => RedirectToAction("Index", "Doctor"),
                3 => RedirectToAction("Index", "Patient"),
                _ => RedirectToAction("Login")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            ModelState.AddModelError(string.Empty, "An error occurred during login");
            return View(loginDto);
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createUserDto);
        }

        try
        {
            var userId = await _authService.RegisterUserAsync(createUserDto);

            if (userId == -1)
            {
                ModelState.AddModelError(string.Empty, "Email already exists");
                return View(createUserDto);
            }

            TempData["SuccessMessage"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration");
            ModelState.AddModelError(string.Empty, "An error occurred during registration");
            return View(createUserDto);
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
