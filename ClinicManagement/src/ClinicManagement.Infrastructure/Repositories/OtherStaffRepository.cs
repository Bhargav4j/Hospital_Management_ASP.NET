using AutoMapper;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using DbModels = ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

public class OtherStaffRepository : Repository<OtherStaff, DbModels.otherstaff>, IOtherStaffRepository
{
    public OtherStaffRepository(ClinicManagementDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public async Task<IEnumerable<OtherStaff>> GetByDesignationAsync(string designation)
    {
        var dbModels = await _dbSet.AsNoTracking()
            .Where(s => s.designation == designation)
            .ToListAsync();
        return _mapper.Map<IEnumerable<OtherStaff>>(dbModels);
    }
}
