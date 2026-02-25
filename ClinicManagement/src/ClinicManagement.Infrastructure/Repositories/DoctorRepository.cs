using AutoMapper;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using DbModels = ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

public class DoctorRepository : Repository<Doctor, DbModels.doctor>, IDoctorRepository
{
    public DoctorRepository(ClinicManagementDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override async Task<Doctor> AddAsync(Doctor entity)
    {
        var loginEntry = new DbModels.logintable
        {
            email = entity.Email,
            password = entity.Password,
            type = 2
        };
        await _context.Set<DbModels.logintable>().AddAsync(loginEntry);
        await _context.SaveChangesAsync();

        entity.DoctorID = loginEntry.loginid;

        var dbModel = _mapper.Map<DbModels.doctor>(entity);
        dbModel.doctorid = loginEntry.loginid;
        await _dbSet.AddAsync(dbModel);
        await _context.SaveChangesAsync();

        return _mapper.Map<Doctor>(dbModel);
    }

    public override async Task<Doctor?> GetByIdAsync(int id)
    {
        var dbModel = await _dbSet.AsNoTracking()
            .Include(d => d.deptnoNavigation)
            .FirstOrDefaultAsync(d => d.doctorid == id);
        return dbModel != null ? _mapper.Map<Doctor>(dbModel) : null;
    }

    public override async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        var dbModels = await _dbSet.AsNoTracking()
            .Include(d => d.deptnoNavigation)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Doctor>>(dbModels);
    }

    public async Task<Doctor?> GetByEmailAsync(string email)
    {
        var loginEntry = await _context.Set<DbModels.logintable>().AsNoTracking()
            .FirstOrDefaultAsync(l => l.email == email && l.type == 2);
        if (loginEntry == null) return null;

        return await GetByIdAsync(loginEntry.loginid);
    }

    public async Task<Doctor?> ValidateCredentialsAsync(string email, string password)
    {
        var loginEntry = await _context.Set<DbModels.logintable>().AsNoTracking()
            .FirstOrDefaultAsync(l => l.email == email && l.password == password && l.type == 2);
        if (loginEntry == null) return null;

        return await GetByIdAsync(loginEntry.loginid);
    }

    public async Task<IEnumerable<Doctor>> GetByDepartmentAsync(int deptNo)
    {
        var dbModels = await _dbSet.AsNoTracking()
            .Include(d => d.deptnoNavigation)
            .Where(d => d.deptno == deptNo)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Doctor>>(dbModels);
    }

    public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization)
    {
        var dbModels = await _dbSet.AsNoTracking()
            .Include(d => d.deptnoNavigation)
            .Where(d => d.specialization == specialization)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Doctor>>(dbModels);
    }

    public async Task<IEnumerable<Doctor>> SearchDoctorsAsync(string searchTerm)
    {
        var dbModels = await _dbSet.AsNoTracking()
            .Include(d => d.deptnoNavigation)
            .Where(d => d.name.Contains(searchTerm) ||
                       (d.specialization != null && d.specialization.Contains(searchTerm)) ||
                       d.deptnoNavigation.deptname.Contains(searchTerm))
            .ToListAsync();
        return _mapper.Map<IEnumerable<Doctor>>(dbModels);
    }
}
