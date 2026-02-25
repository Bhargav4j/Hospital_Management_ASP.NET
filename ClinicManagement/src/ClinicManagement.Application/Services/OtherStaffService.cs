using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;

namespace ClinicManagement.Application.Services;

public class OtherStaffService : IOtherStaffService
{
    private readonly IOtherStaffRepository _staffRepository;
    private readonly ICacheService _cacheService;

    private const string AllStaffCacheKey = "staff:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

    public OtherStaffService(IOtherStaffRepository staffRepository, ICacheService cacheService)
    {
        _staffRepository = staffRepository;
        _cacheService = cacheService;
    }

    public async Task<OtherStaff?> GetStaffByIdAsync(int id)
    {
        return await _staffRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<OtherStaff>> GetAllStaffAsync()
    {
        var cached = await _cacheService.GetAsync<List<OtherStaff>>(AllStaffCacheKey);
        if (cached != null)
            return cached;

        var staff = (await _staffRepository.GetAllAsync()).ToList();
        await _cacheService.SetAsync(AllStaffCacheKey, staff, CacheDuration);
        return staff;
    }

    public async Task<OtherStaff> CreateStaffAsync(OtherStaff staff)
    {
        var result = await _staffRepository.AddAsync(staff);
        await _cacheService.RemoveAsync(AllStaffCacheKey);
        return result;
    }

    public async Task UpdateStaffAsync(OtherStaff staff)
    {
        staff.ModifiedDate = DateTime.UtcNow;
        await _staffRepository.UpdateAsync(staff);
        await _cacheService.RemoveAsync(AllStaffCacheKey);
    }

    public async Task DeleteStaffAsync(int id)
    {
        await _staffRepository.DeleteAsync(id);
        await _cacheService.RemoveAsync(AllStaffCacheKey);
    }

    public async Task<IEnumerable<OtherStaff>> GetStaffByDesignationAsync(string designation)
    {
        return await _staffRepository.GetByDesignationAsync(designation);
    }
}
