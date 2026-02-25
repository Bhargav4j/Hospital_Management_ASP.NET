using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;

namespace ClinicManagement.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ICacheService _cacheService;

    private const string AllDoctorsCacheKey = "doctors:all";
    private const string DoctorByIdCacheKeyPrefix = "doctors:id:";
    private const string DoctorsByDeptCacheKeyPrefix = "doctors:dept:";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

    public DoctorService(IDoctorRepository doctorRepository, ICacheService cacheService)
    {
        _doctorRepository = doctorRepository;
        _cacheService = cacheService;
    }

    public async Task<Doctor?> GetDoctorByIdAsync(int id)
    {
        var cacheKey = $"{DoctorByIdCacheKeyPrefix}{id}";
        var cached = await _cacheService.GetAsync<Doctor>(cacheKey);
        if (cached != null)
            return cached;

        var doctor = await _doctorRepository.GetByIdAsync(id);
        if (doctor != null)
            await _cacheService.SetAsync(cacheKey, doctor, CacheDuration);

        return doctor;
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
    {
        var cached = await _cacheService.GetAsync<List<Doctor>>(AllDoctorsCacheKey);
        if (cached != null)
            return cached;

        var doctors = (await _doctorRepository.GetAllAsync()).ToList();
        await _cacheService.SetAsync(AllDoctorsCacheKey, doctors, CacheDuration);
        return doctors;
    }

    public async Task<Doctor> CreateDoctorAsync(Doctor doctor)
    {
        var result = await _doctorRepository.AddAsync(doctor);
        await InvalidateDoctorCacheAsync();
        return result;
    }

    public async Task UpdateDoctorAsync(Doctor doctor)
    {
        doctor.ModifiedDate = DateTime.UtcNow;
        await _doctorRepository.UpdateAsync(doctor);
        await InvalidateDoctorCacheAsync(doctor.DoctorID);
    }

    public async Task DeleteDoctorAsync(int id)
    {
        await _doctorRepository.DeleteAsync(id);
        await InvalidateDoctorCacheAsync(id);
    }

    public async Task<Doctor?> ValidateLoginAsync(string email, string password)
    {
        return await _doctorRepository.ValidateCredentialsAsync(email, password);
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int deptNo)
    {
        var cacheKey = $"{DoctorsByDeptCacheKeyPrefix}{deptNo}";
        var cached = await _cacheService.GetAsync<List<Doctor>>(cacheKey);
        if (cached != null)
            return cached;

        var doctors = (await _doctorRepository.GetByDepartmentAsync(deptNo)).ToList();
        await _cacheService.SetAsync(cacheKey, doctors, CacheDuration);
        return doctors;
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsBySpecializationAsync(string specialization)
    {
        return await _doctorRepository.GetBySpecializationAsync(specialization);
    }

    public async Task<IEnumerable<Doctor>> SearchDoctorsAsync(string searchTerm)
    {
        return await _doctorRepository.SearchDoctorsAsync(searchTerm);
    }

    private async Task InvalidateDoctorCacheAsync(int? doctorId = null)
    {
        await _cacheService.RemoveAsync(AllDoctorsCacheKey);
        await _cacheService.RemoveByPrefixAsync(DoctorsByDeptCacheKeyPrefix);

        if (doctorId.HasValue)
            await _cacheService.RemoveAsync($"{DoctorByIdCacheKeyPrefix}{doctorId.Value}");
    }
}
