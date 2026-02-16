using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;

namespace ClinicManagement.Application.Services;

public class OtherStaffService : IOtherStaffService
{
    private readonly IOtherStaffRepository _staffRepository;

    public OtherStaffService(IOtherStaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public async Task<OtherStaff?> GetStaffByIdAsync(int id)
    {
        return await _staffRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<OtherStaff>> GetAllStaffAsync()
    {
        return await _staffRepository.GetAllAsync();
    }

    public async Task<OtherStaff> CreateStaffAsync(OtherStaff staff)
    {
        return await _staffRepository.AddAsync(staff);
    }

    public async Task UpdateStaffAsync(OtherStaff staff)
    {
        staff.ModifiedDate = DateTime.UtcNow;
        await _staffRepository.UpdateAsync(staff);
    }

    public async Task DeleteStaffAsync(int id)
    {
        await _staffRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<OtherStaff>> GetStaffByDesignationAsync(string designation)
    {
        return await _staffRepository.GetByDesignationAsync(designation);
    }
}
