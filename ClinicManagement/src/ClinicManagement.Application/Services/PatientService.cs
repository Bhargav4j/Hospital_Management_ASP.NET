using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;

namespace ClinicManagement.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly ICacheService _cacheService;

    private const string AllPatientsCacheKey = "patients:all";
    private const string PatientByIdCacheKeyPrefix = "patients:id:";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

    public PatientService(IPatientRepository patientRepository, ICacheService cacheService)
    {
        _patientRepository = patientRepository;
        _cacheService = cacheService;
    }

    public async Task<Patient?> GetPatientByIdAsync(int id)
    {
        var cacheKey = $"{PatientByIdCacheKeyPrefix}{id}";
        var cached = await _cacheService.GetAsync<Patient>(cacheKey);
        if (cached != null)
            return cached;

        var patient = await _patientRepository.GetByIdAsync(id);
        if (patient != null)
            await _cacheService.SetAsync(cacheKey, patient, CacheDuration);

        return patient;
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        var cached = await _cacheService.GetAsync<List<Patient>>(AllPatientsCacheKey);
        if (cached != null)
            return cached;

        var patients = (await _patientRepository.GetAllAsync()).ToList();
        await _cacheService.SetAsync(AllPatientsCacheKey, patients, CacheDuration);
        return patients;
    }

    public async Task<Patient> CreatePatientAsync(Patient patient)
    {
        var result = await _patientRepository.AddAsync(patient);
        await InvalidatePatientCacheAsync();
        return result;
    }

    public async Task UpdatePatientAsync(Patient patient)
    {
        patient.ModifiedDate = DateTime.UtcNow;
        await _patientRepository.UpdateAsync(patient);
        await InvalidatePatientCacheAsync(patient.PatientID);
    }

    public async Task DeletePatientAsync(int id)
    {
        await _patientRepository.DeleteAsync(id);
        await InvalidatePatientCacheAsync(id);
    }

    public async Task<Patient?> ValidateLoginAsync(string email, string password)
    {
        return await _patientRepository.ValidateCredentialsAsync(email, password);
    }

    public async Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm)
    {
        return await _patientRepository.SearchPatientsAsync(searchTerm);
    }

    private async Task InvalidatePatientCacheAsync(int? patientId = null)
    {
        await _cacheService.RemoveAsync(AllPatientsCacheKey);

        if (patientId.HasValue)
            await _cacheService.RemoveAsync($"{PatientByIdCacheKeyPrefix}{patientId.Value}");
    }
}
