using AutoMapper;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using DbModels = ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

public class DepartmentRepository : Repository<Department, DbModels.department>, IDepartmentRepository
{
    public DepartmentRepository(ClinicManagementDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public async Task<Department?> GetByNameAsync(string name)
    {
        var dbModel = await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(d => d.deptname == name);
        return dbModel != null ? _mapper.Map<Department>(dbModel) : null;
    }
}
