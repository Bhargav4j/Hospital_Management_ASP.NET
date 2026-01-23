using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace ClinicManagement.Application.Services;

public interface IAuthenticationService
{
    Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    Task<LoginResultDto> RegisterPatientAsync(PatientCreateDto patientDto, CancellationToken cancellationToken = default);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAdminRepository _adminRepository;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IAdminRepository adminRepository,
        ILogger<AuthenticationService> logger)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _adminRepository = adminRepository;
        _logger = logger;
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check in Patient table
            var patient = await _patientRepository.GetByEmailAsync(loginDto.Email, cancellationToken);
            if (patient != null)
            {
                if (VerifyPassword(loginDto.Password, patient.PasswordHash))
                {
                    _logger.LogInformation("Patient logged in successfully: {Email}", loginDto.Email);
                    return new LoginResultDto
                    {
                        Success = true,
                        UserId = patient.Id,
                        UserType = UserType.Patient,
                        Message = "Login successful"
                    };
                }
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Incorrect password"
                };
            }

            // Check in Doctor table
            var doctor = await _doctorRepository.GetByEmailAsync(loginDto.Email, cancellationToken);
            if (doctor != null)
            {
                if (VerifyPassword(loginDto.Password, doctor.PasswordHash))
                {
                    _logger.LogInformation("Doctor logged in successfully: {Email}", loginDto.Email);
                    return new LoginResultDto
                    {
                        Success = true,
                        UserId = doctor.Id,
                        UserType = UserType.Doctor,
                        Message = "Login successful"
                    };
                }
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Incorrect password"
                };
            }

            // Check in Admin table
            var admin = await _adminRepository.GetByEmailAsync(loginDto.Email, cancellationToken);
            if (admin != null)
            {
                if (VerifyPassword(loginDto.Password, admin.PasswordHash))
                {
                    _logger.LogInformation("Admin logged in successfully: {Email}", loginDto.Email);
                    return new LoginResultDto
                    {
                        Success = true,
                        UserId = admin.Id,
                        UserType = UserType.Admin,
                        Message = "Login successful"
                    };
                }
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Incorrect password"
                };
            }

            return new LoginResultDto
            {
                Success = false,
                Message = "Email not found"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email {Email}", loginDto.Email);
            throw;
        }
    }

    public async Task<LoginResultDto> RegisterPatientAsync(PatientCreateDto patientDto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if email already exists
            if (await _patientRepository.EmailExistsAsync(patientDto.Email, cancellationToken))
            {
                return new LoginResultDto
                {
                    Success = false,
                    Message = "Email already exists"
                };
            }

            var patient = new Domain.Entities.Patient
            {
                Name = patientDto.Name,
                Email = patientDto.Email,
                PasswordHash = HashPassword(patientDto.Password),
                PhoneNumber = patientDto.PhoneNumber,
                DateOfBirth = patientDto.DateOfBirth,
                Gender = patientDto.Gender,
                Address = patientDto.Address,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            var createdPatient = await _patientRepository.AddAsync(patient, cancellationToken);
            _logger.LogInformation("Patient registered successfully: {Email}", patientDto.Email);

            return new LoginResultDto
            {
                Success = true,
                UserId = createdPatient.Id,
                UserType = UserType.Patient,
                Message = "Registration successful"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during patient registration for email {Email}", patientDto.Email);
            throw;
        }
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyPassword(string password, string hash)
    {
        var passwordHash = HashPassword(password);
        return passwordHash == hash;
    }
}
