using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

public interface IDepartmentRepository : IRepository<Department>
{
    Task<Department?> GetByNameAsync(string name);
}
