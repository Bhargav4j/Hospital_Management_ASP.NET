using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Interfaces.Services;

public interface IOtherStaffService
{
    Task<OtherStaff?> GetStaffByIdAsync(int id);
    Task<IEnumerable<OtherStaff>> GetAllStaffAsync();
    Task<OtherStaff> CreateStaffAsync(OtherStaff staff);
    Task UpdateStaffAsync(OtherStaff staff);
    Task DeleteStaffAsync(int id);
    Task<IEnumerable<OtherStaff>> GetStaffByDesignationAsync(string designation);
}
