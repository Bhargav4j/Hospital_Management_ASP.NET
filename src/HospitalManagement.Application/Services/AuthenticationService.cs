using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IUserRepository userRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        ILogger<AuthenticationService> logger)
    {
        _userRepository = userRepository;
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _logger = logger;
    }

    public async Task<(bool Success, int UserId, int UserType, string Message)> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return (false, 0, 0, "Email and password are required");
            }

            var user = await _userRepository.ValidateLoginAsync(email, password, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Failed login attempt for email: {Email}", email);
                return (false, 0, 0, "Invalid email or password");
            }

            _logger.LogInformation("Successful login for user: {UserId}", user.Id);
            return (true, user.Id, user.UserType, "Login successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", email);
            return (false, 0, 0, "An error occurred during login");
        }
    }

    public async Task<(bool Success, string Message)> RegisterAsync(
        string name,
        string email,
        string password,
        int userType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "All fields are required");
            }

            var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (existingUser != null)
            {
                return (false, "Email already registered");
            }

            var user = new User
            {
                Name = name,
                Email = email,
                Password = password,
                UserType = userType,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            var createdUser = await _userRepository.AddAsync(user, cancellationToken);

            if (userType == 1)
            {
                var patient = new Patient
                {
                    UserId = createdUser.Id,
                    Name = name,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedBy = "System"
                };
                await _patientRepository.AddAsync(patient, cancellationToken);
            }
            else if (userType == 2)
            {
                var doctor = new Doctor
                {
                    UserId = createdUser.Id,
                    Name = name,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedBy = "System"
                };
                await _doctorRepository.AddAsync(doctor, cancellationToken);
            }

            _logger.LogInformation("User registered successfully: {UserId}", createdUser.Id);
            return (true, "Registration successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", email);
            return (false, "An error occurred during registration");
        }
    }
}
