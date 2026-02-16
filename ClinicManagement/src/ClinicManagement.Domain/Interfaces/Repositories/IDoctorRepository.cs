using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

public interface IDoctorRepository : IRepository<Doctor>
{
    Task<Doctor?> GetByEmailAsync(string email);
    Task<Doctor?> ValidateCredentialsAsync(string email, string password);
    Task<IEnumerable<Doctor>> GetByDepartmentAsync(int deptNo);
    Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization);
    Task<IEnumerable<Doctor>> SearchDoctorsAsync(string searchTerm);
}
