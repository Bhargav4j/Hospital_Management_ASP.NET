using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;

namespace ClinicManagement.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ICacheService _cacheService;

    private const string AllDepartmentsCacheKey = "departments:all";
    private const string DepartmentByIdCacheKeyPrefix = "departments:id:";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    public DepartmentService(IDepartmentRepository departmentRepository, ICacheService cacheService)
    {
        _departmentRepository = departmentRepository;
        _cacheService = cacheService;
    }

    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        var cacheKey = $"{DepartmentByIdCacheKeyPrefix}{id}";
        var cached = await _cacheService.GetAsync<Department>(cacheKey);
        if (cached != null)
            return cached;

        var department = await _departmentRepository.GetByIdAsync(id);
        if (department != null)
            await _cacheService.SetAsync(cacheKey, department, CacheDuration);

        return department;
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        var cached = await _cacheService.GetAsync<List<Department>>(AllDepartmentsCacheKey);
        if (cached != null)
            return cached;

        var departments = (await _departmentRepository.GetAllAsync()).ToList();
        await _cacheService.SetAsync(AllDepartmentsCacheKey, departments, CacheDuration);
        return departments;
    }

    public async Task<Department> CreateDepartmentAsync(Department department)
    {
        var result = await _departmentRepository.AddAsync(department);
        await InvalidateDepartmentCacheAsync();
        return result;
    }

    public async Task UpdateDepartmentAsync(Department department)
    {
        department.ModifiedDate = DateTime.UtcNow;
        await _departmentRepository.UpdateAsync(department);
        await InvalidateDepartmentCacheAsync(department.DeptNo);
    }

    public async Task DeleteDepartmentAsync(int id)
    {
        await _departmentRepository.DeleteAsync(id);
        await InvalidateDepartmentCacheAsync(id);
    }

    private async Task InvalidateDepartmentCacheAsync(int? deptId = null)
    {
        await _cacheService.RemoveAsync(AllDepartmentsCacheKey);

        if (deptId.HasValue)
            await _cacheService.RemoveAsync($"{DepartmentByIdCacheKeyPrefix}{deptId.Value}");
    }
}
