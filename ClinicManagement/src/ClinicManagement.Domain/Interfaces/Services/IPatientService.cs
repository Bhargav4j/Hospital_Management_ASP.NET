using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Services;

public interface IPatientService
{
    Task<Patient?> GetPatientByIdAsync(int id);
    Task<IEnumerable<Patient>> GetAllPatientsAsync();
    Task<Patient> CreatePatientAsync(Patient patient);
    Task UpdatePatientAsync(Patient patient);
    Task DeletePatientAsync(int id);
    Task<Patient?> ValidateLoginAsync(string email, string password);
    Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm);
}
