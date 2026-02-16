using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;

namespace ClinicManagement.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Patient?> GetPatientByIdAsync(int id)
    {
        return await _patientRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        return await _patientRepository.GetAllAsync();
    }

    public async Task<Patient> CreatePatientAsync(Patient patient)
    {
        // Hash password before saving (simple implementation - use proper hashing in production)
        patient.Password = HashPassword(patient.Password);
        return await _patientRepository.AddAsync(patient);
    }

    public async Task UpdatePatientAsync(Patient patient)
    {
        patient.ModifiedDate = DateTime.UtcNow;
        await _patientRepository.UpdateAsync(patient);
    }

    public async Task DeletePatientAsync(int id)
    {
        await _patientRepository.DeleteAsync(id);
    }

    public async Task<Patient?> ValidateLoginAsync(string email, string password)
    {
        var hashedPassword = HashPassword(password);
        return await _patientRepository.ValidateCredentialsAsync(email, hashedPassword);
    }

    public async Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm)
    {
        return await _patientRepository.SearchPatientsAsync(searchTerm);
    }

    private string HashPassword(string password)
    {
        // Simple hash implementation - use BCrypt or similar in production
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
