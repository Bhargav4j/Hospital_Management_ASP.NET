using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Interfaces;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all users");
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            throw;
        }
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user with ID {UserId}", id);
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
            throw;
        }
    }

    public async Task<UserDto> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new user with email {Email}", dto.Email);

            var existingUser = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            var user = _mapper.Map<User>(dto);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;
            user.CreatedBy = "System";

            var createdUser = await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User created successfully with ID {UserId}", createdUser.Id);

            return _mapper.Map<UserDto>(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            throw;
        }
    }

    public async Task UpdateAsync(int id, UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with ID {UserId}", id);

            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found");
            }

            user.Name = dto.Name;
            user.PhoneNo = dto.PhoneNo;
            user.Address = dto.Address;
            user.ModifiedDate = DateTime.UtcNow;
            user.ModifiedBy = "System";

            await _userRepository.UpdateAsync(user, cancellationToken);
            _logger.LogInformation("User updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID {UserId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID {UserId}", id);
            await _userRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("User deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
            throw;
        }
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Login attempt for email {Email}", dto.Email);

            var user = await _userRepository.ValidateLoginAsync(dto.Email, dto.Password, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Login failed for email {Email}", dto.Email);
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            _logger.LogInformation("Login successful for user {UserId}", user.Id);
            return new LoginResultDto
            {
                Success = true,
                UserId = user.Id,
                UserType = user.UserType,
                Message = "Login successful"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            throw;
        }
    }
}
