using AutoMapper;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using DbModels = ClinicManagement.Infrastructure.Data.Scaffolded.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

public class PatientRepository : Repository<Patient, DbModels.patient>, IPatientRepository
{
    public PatientRepository(ClinicManagementDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override async Task<Patient?> GetByIdAsync(int id)
    {
        var dbModel = await _dbSet.AsNoTracking()
            .Include(p => p.patientNavigation)
            .FirstOrDefaultAsync(p => p.patientid == id);
        return dbModel != null ? _mapper.Map<Patient>(dbModel) : null;
    }

    public override async Task<IEnumerable<Patient>> GetAllAsync()
    {
        var dbModels = await _dbSet.AsNoTracking()
            .Include(p => p.patientNavigation)
            .ToListAsync();
        return _mapper.Map<IEnumerable<Patient>>(dbModels);
    }

    public async Task<Patient?> GetByEmailAsync(string email)
    {
        var dbModel = await _dbSet.AsNoTracking()
            .Include(p => p.patientNavigation)
            .FirstOrDefaultAsync(p => p.patientNavigation.email == email);
        return dbModel != null ? _mapper.Map<Patient>(dbModel) : null;
    }

    public async Task<Patient?> ValidateCredentialsAsync(string email, string password)
    {
        var dbModel = await _dbSet.AsNoTracking()
            .Include(p => p.patientNavigation)
            .FirstOrDefaultAsync(p =>
                p.patientNavigation.email == email &&
                p.patientNavigation.password == password);
        return dbModel != null ? _mapper.Map<Patient>(dbModel) : null;
    }

    public async Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm)
    {
        var dbModels = await _dbSet.AsNoTracking()
            .Include(p => p.patientNavigation)
            .Where(p => p.name.Contains(searchTerm) ||
                        p.patientNavigation.email.Contains(searchTerm))
            .ToListAsync();
        return _mapper.Map<IEnumerable<Patient>>(dbModels);
    }
}
