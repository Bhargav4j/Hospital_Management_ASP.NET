using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByEmailAsync(string email);
    Task<Patient?> ValidateCredentialsAsync(string email, string password);
    Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm);
}
