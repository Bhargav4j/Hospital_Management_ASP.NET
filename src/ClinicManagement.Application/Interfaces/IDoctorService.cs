using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
    Task<DoctorDto?> GetDoctorByIdAsync(int id);
    Task<int> CreateDoctorAsync(CreateDoctorDto createDoctorDto);
    Task UpdateDoctorAsync(DoctorDto doctorDto);
    Task DeleteDoctorAsync(int id);
    Task<IEnumerable<DoctorDto>> GetDoctorsByClinicAsync(int clinicId);
}
