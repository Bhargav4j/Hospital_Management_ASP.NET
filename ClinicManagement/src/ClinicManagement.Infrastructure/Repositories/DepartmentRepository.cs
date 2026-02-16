using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    public DepartmentRepository(ClinicManagementDbContext context) : base(context)
    {
    }

    public async Task<Department?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(d => d.DeptName == name);
    }
}
