using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Admin.Departments;

public class AdminDepartmentsIndexModel : PageModel
{
    private readonly IDepartmentService _service;
    private readonly IMapper _mapper;
    public AdminDepartmentsIndexModel(IDepartmentService s, IMapper m) { _service = s; _mapper = m; }
    public IEnumerable<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToPage("/Login");
        Departments = _mapper.Map<IEnumerable<DepartmentDto>>(await _service.GetAllDepartmentsAsync());
        return Page();
    }
}
