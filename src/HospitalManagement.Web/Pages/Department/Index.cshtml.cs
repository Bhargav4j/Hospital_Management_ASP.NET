using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Department;

public class IndexModel : PageModel
{
    private readonly IDepartmentService _departmentService;

    public IndexModel(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    public IEnumerable<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        Departments = await _departmentService.GetAllDepartmentsAsync(cancellationToken);
        return Page();
    }
}
