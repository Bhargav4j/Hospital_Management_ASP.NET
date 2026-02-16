using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

public class OtherStaffRepository : Repository<OtherStaff>, IOtherStaffRepository
{
    public OtherStaffRepository(ClinicManagementDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<OtherStaff>> GetByDesignationAsync(string designation)
    {
        return await _dbSet
            .Where(s => s.Designation == designation)
            .ToListAsync();
    }
}
