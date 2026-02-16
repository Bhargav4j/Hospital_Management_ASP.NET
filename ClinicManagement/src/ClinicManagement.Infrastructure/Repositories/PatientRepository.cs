using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

public class PatientRepository : Repository<Patient>, IPatientRepository
{
    public PatientRepository(ClinicManagementDbContext context) : base(context)
    {
    }

    public async Task<Patient?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Email == email);
    }

    public async Task<Patient?> ValidateCredentialsAsync(string email, string password)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);
    }

    public async Task<IEnumerable<Patient>> SearchPatientsAsync(string searchTerm)
    {
        return await _dbSet
            .Where(p => p.Name.Contains(searchTerm) || p.Email.Contains(searchTerm))
            .ToListAsync();
    }
}
