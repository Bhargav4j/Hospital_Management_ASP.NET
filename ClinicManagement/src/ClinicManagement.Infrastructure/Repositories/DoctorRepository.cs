using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Infrastructure.Repositories;

public class DoctorRepository : Repository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ClinicManagementDbContext context) : base(context)
    {
    }

    public override async Task<Doctor?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(d => d.Department)
            .FirstOrDefaultAsync(d => d.DoctorID == id);
    }

    public override async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        return await _dbSet
            .Include(d => d.Department)
            .ToListAsync();
    }

    public async Task<Doctor?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(d => d.Department)
            .FirstOrDefaultAsync(d => d.Email == email);
    }

    public async Task<Doctor?> ValidateCredentialsAsync(string email, string password)
    {
        return await _dbSet
            .Include(d => d.Department)
            .FirstOrDefaultAsync(d => d.Email == email && d.Password == password);
    }

    public async Task<IEnumerable<Doctor>> GetByDepartmentAsync(int deptNo)
    {
        return await _dbSet
            .Include(d => d.Department)
            .Where(d => d.DeptNo == deptNo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization)
    {
        return await _dbSet
            .Include(d => d.Department)
            .Where(d => d.Specialization == specialization)
            .ToListAsync();
    }

    public async Task<IEnumerable<Doctor>> SearchDoctorsAsync(string searchTerm)
    {
        return await _dbSet
            .Include(d => d.Department)
            .Where(d => d.Name.Contains(searchTerm) || 
                       d.Specialization.Contains(searchTerm) ||
                       d.Department.DeptName.Contains(searchTerm))
            .ToListAsync();
    }
}
