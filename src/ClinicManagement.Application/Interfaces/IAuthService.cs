using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<int> RegisterUserAsync(CreateUserDto createUserDto);
    Task<string> HashPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string password, string passwordHash);
}
