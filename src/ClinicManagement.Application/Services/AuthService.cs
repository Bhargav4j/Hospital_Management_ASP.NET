using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Interfaces;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IRepository<User> userRepository, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var users = await _userRepository.FindAsync(u => u.Email == loginDto.Email);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found with email {Email}", loginDto.Email);
                return new LoginResponseDto { Success = false };
            }

            var isPasswordValid = await VerifyPasswordAsync(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Login failed: Invalid password for email {Email}", loginDto.Email);
                return new LoginResponseDto { Success = false };
            }

            _logger.LogInformation("User {Email} logged in successfully", loginDto.Email);
            return new LoginResponseDto
            {
                Success = true,
                UserId = user.Id,
                UserType = user.Type,
                Token = GenerateToken(user)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email {Email}", loginDto.Email);
            throw;
        }
    }

    public async Task<int> RegisterUserAsync(CreateUserDto createUserDto)
    {
        try
        {
            var existingUsers = await _userRepository.FindAsync(u => u.Email == createUserDto.Email);
            if (existingUsers.Any())
            {
                _logger.LogWarning("Registration failed: Email {Email} already exists", createUserDto.Email);
                return -1;
            }

            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                PasswordHash = await HashPasswordAsync(createUserDto.Password),
                PhoneNumber = createUserDto.PhoneNumber,
                Address = createUserDto.Address,
                BirthDate = createUserDto.BirthDate,
                Gender = createUserDto.Gender,
                Type = createUserDto.Type,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user);
            _logger.LogInformation("User registered successfully with email {Email}", createUserDto.Email);
            return createdUser.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration for email {Email}", createUserDto.Email);
            throw;
        }
    }

    public Task<string> HashPasswordAsync(string password)
    {
        return Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }

    public Task<bool> VerifyPasswordAsync(string password, string passwordHash)
    {
        try
        {
            return Task.FromResult(BCrypt.Net.BCrypt.Verify(password, passwordHash));
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    private string GenerateToken(User user)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{user.Id}:{user.Email}:{user.Type}"));
    }
}
