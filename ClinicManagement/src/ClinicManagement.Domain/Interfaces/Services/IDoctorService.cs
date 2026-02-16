using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Services;

public interface IDoctorService
{
    Task<Doctor?> GetDoctorByIdAsync(int id);
    Task<IEnumerable<Doctor>> GetAllDoctorsAsync();
    Task<Doctor> CreateDoctorAsync(Doctor doctor);
    Task UpdateDoctorAsync(Doctor doctor);
    Task DeleteDoctorAsync(int id);
    Task<Doctor?> ValidateLoginAsync(string email, string password);
    Task<IEnumerable<Doctor>> GetDoctorsByDepartmentAsync(int deptNo);
    Task<IEnumerable<Doctor>> GetDoctorsBySpecializationAsync(string specialization);
    Task<IEnumerable<Doctor>> SearchDoctorsAsync(string searchTerm);
}
