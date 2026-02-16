using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;

namespace ClinicManagement.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<Doctor?> GetDoctorByIdAsync(int id)
    {
        return await _doctorRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
    {
        return await _doctorRepository.GetAllAsync();
    }

    public async Task<Doctor> CreateDoctorAsync(Doctor doctor)
    {
        doctor.Password = HashPassword(doctor.Password);
        return await _doctorRepository.AddAsync(doctor);
    }

    public async Task UpdateDoctorAsync(Doctor doctor)
    {
        doctor.ModifiedDate = DateTime.UtcNow;
        await _doctorRepository.UpdateAsync(doctor);
    }

    public async Task DeleteDoctorAsync(int id)
    {
        await _doctorRepository.DeleteAsync(id);
    }

    public async Task<Doctor?> ValidateLoginAsync(string email, string password)
    {
        var hashedPassword = HashPassword(password);
        return await _doctorRepository.ValidateCredentialsAsync(email, hashedPassword);
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int deptNo)
    {
        return await _doctorRepository.GetByDepartmentAsync(deptNo);
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsBySpecializationAsync(string specialization)
    {
        return await _doctorRepository.GetBySpecializationAsync(specialization);
    }

    public async Task<IEnumerable<Doctor>> SearchDoctorsAsync(string searchTerm)
    {
        return await _doctorRepository.SearchDoctorsAsync(searchTerm);
    }

    private string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
