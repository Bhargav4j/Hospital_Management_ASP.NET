using Microsoft.Extensions.Logging;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Services;
using ClinicManagement.Domain.Interfaces.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace ClinicManagement.Application.Services;

/// <summary>
/// Authentication service implementation
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        ILogger<AuthenticationService> logger)
    {
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(bool Success, int UserId, string UserType, string Message)> ValidateLoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating login for email: {Email}", email);

            // Check patient
            var patient = await _patientRepository.GetByEmailAsync(email, cancellationToken);
            if (patient != null)
            {
                if (VerifyPassword(password, patient.PasswordHash))
                {
                    _logger.LogInformation("Patient login successful: {Email}", email);
                    return (true, patient.Id, "Patient", "Login successful");
                }
                return (false, 0, string.Empty, "Incorrect password");
            }

            // Check doctor
            var doctor = await _doctorRepository.GetByEmailAsync(email, cancellationToken);
            if (doctor != null)
            {
                if (VerifyPassword(password, doctor.PasswordHash))
                {
                    _logger.LogInformation("Doctor login successful: {Email}", email);
                    return (true, doctor.Id, "Doctor", "Login successful");
                }
                return (false, 0, string.Empty, "Incorrect password");
            }

            _logger.LogWarning("Email not found: {Email}", email);
            return (false, 0, string.Empty, "Email not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating login for email: {Email}", email);
            return (false, 0, string.Empty, "An error occurred during login");
        }
    }

    public async Task<(bool Success, int UserId, string Message)> RegisterPatientAsync(
        string name,
        DateTime birthDate,
        string email,
        string password,
        string phoneNo,
        string gender,
        string address,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering new patient: {Email}", email);

            // Check if email already exists
            var existingPatient = await _patientRepository.GetByEmailAsync(email, cancellationToken);
            if (existingPatient != null)
            {
                _logger.LogWarning("Email already exists: {Email}", email);
                return (false, 0, "Email already registered");
            }

            var patient = new Patient
            {
                Name = name,
                BirthDate = birthDate,
                Email = email,
                PasswordHash = HashPassword(password),
                PhoneNo = phoneNo,
                Gender = gender,
                Address = address,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            var addedPatient = await _patientRepository.AddAsync(patient, cancellationToken);

            _logger.LogInformation("Patient registered successfully: {Email}", email);
            return (true, addedPatient.Id, "Registration successful");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering patient: {Email}", email);
            return (false, 0, "An error occurred during registration");
        }
    }

    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var hashedInput = HashPassword(password);
        return hashedInput == passwordHash;
    }
}
