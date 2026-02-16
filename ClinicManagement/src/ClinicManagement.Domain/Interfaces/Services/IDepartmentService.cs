using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Services;

public interface IDepartmentService
{
    Task<Department?> GetDepartmentByIdAsync(int id);
    Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    Task<Department> CreateDepartmentAsync(Department department);
    Task UpdateDepartmentAsync(Department department);
    Task DeleteDepartmentAsync(int id);
}
