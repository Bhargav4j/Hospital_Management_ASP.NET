using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Repositories;

public interface IOtherStaffRepository : IRepository<OtherStaff>
{
    Task<IEnumerable<OtherStaff>> GetByDesignationAsync(string designation);
}
