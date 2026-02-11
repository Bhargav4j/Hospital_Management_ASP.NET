using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserDto> CreateAsync(UserCreateDto userCreateDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<LoginResultDto> ValidateLoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}
