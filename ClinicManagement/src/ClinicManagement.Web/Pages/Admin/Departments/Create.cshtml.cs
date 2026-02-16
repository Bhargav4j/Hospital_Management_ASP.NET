using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Admin.Departments;

public class CreateDepartmentModel : PageModel
{
    private readonly IDepartmentService _service;
    public CreateDepartmentModel(IDepartmentService s) { _service = s; }
    [BindProperty, Required] public string DeptName { get; set; } = string.Empty;
    [BindProperty] public string Description { get; set; } = string.Empty;
    public IActionResult OnGet() => HttpContext.Session.GetString("UserRole") != "Admin" ? RedirectToPage("/Login") : Page();
    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");
        await _service.CreateDepartmentAsync(new Department { DeptName = DeptName, Description = Description });
        return RedirectToPage("/Admin/Departments/Index");
    }
}
