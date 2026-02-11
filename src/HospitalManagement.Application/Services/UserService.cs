using AutoMapper;
using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

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

    public async Task<UserDto> CreateAsync(UserCreateDto userCreateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new user with email {Email}", userCreateDto.Email);

            if (await _userRepository.EmailExistsAsync(userCreateDto.Email, cancellationToken))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var user = _mapper.Map<User>(userCreateDto);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;

            var createdUser = await _userRepository.AddAsync(user, cancellationToken);
            return _mapper.Map<UserDto>(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            throw;
        }
    }

    public async Task UpdateAsync(int id, UserUpdateDto userUpdateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with ID {UserId}", id);

            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            _mapper.Map(userUpdateDto, user);
            user.ModifiedDate = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);
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

            if (!await _userRepository.ExistsAsync(id, cancellationToken))
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            await _userRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users with term {SearchTerm}", searchTerm);
            var users = await _userRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users");
            throw;
        }
    }

    public async Task<LoginResultDto> ValidateLoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating login for email {Email}", loginDto.Email);

            var user = await _userRepository.ValidateLoginAsync(loginDto.Email, loginDto.Password, cancellationToken);

            if (user == null)
            {
                var emailExists = await _userRepository.EmailExistsAsync(loginDto.Email, cancellationToken);
                return new LoginResultDto
                {
                    Success = false,
                    Message = emailExists ? "Incorrect password" : "Email not found"
                };
            }

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
            _logger.LogError(ex, "Error validating login");
            return new LoginResultDto
            {
                Success = false,
                Message = "An error occurred during login"
            };
        }
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _userRepository.EmailExistsAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if email exists");
            throw;
        }
    }
}
