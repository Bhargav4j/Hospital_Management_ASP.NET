using HospitalManagement.Domain.Enums;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

/// <summary>
/// Service for handling authentication operations
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IStaffRepository staffRepository,
        ILogger<AuthenticationService> logger)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _staffRepository = staffRepository;
        _logger = logger;
    }

    public async Task<(bool Success, UserType UserType, int UserId, string Message)> ValidateLoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating login for email: {Email}", email);

            // Check Patient
            var patient = await _patientRepository.GetByEmailAsync(email, cancellationToken);
            if (patient != null && patient.IsActive)
            {
                var isValid = await VerifyPasswordAsync(password, patient.Password);
                if (isValid)
                {
                    _logger.LogInformation("Patient login successful for email: {Email}", email);
                    return (true, UserType.Patient, patient.Id, "Login successful");
                }
            }

            // Check Doctor
            var doctor = await _doctorRepository.GetByEmailAsync(email, cancellationToken);
            if (doctor != null && doctor.IsActive)
            {
                var isValid = await VerifyPasswordAsync(password, doctor.Password);
                if (isValid)
                {
                    _logger.LogInformation("Doctor login successful for email: {Email}", email);
                    return (true, UserType.Doctor, doctor.Id, "Login successful");
                }
            }

            // Check Staff
            var staff = await _staffRepository.GetByEmailAsync(email, cancellationToken);
            if (staff != null && staff.IsActive)
            {
                var isValid = await VerifyPasswordAsync(password, staff.Password);
                if (isValid)
                {
                    _logger.LogInformation("Admin login successful for email: {Email}", email);
                    return (true, UserType.Admin, staff.Id, "Login successful");
                }
            }

            _logger.LogWarning("Login failed for email: {Email}", email);
            return (false, UserType.Patient, 0, "Invalid email or password");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login validation for email: {Email}", email);
            return (false, UserType.Patient, 0, "An error occurred during login");
        }
    }

    public async Task<string> HashPasswordAsync(string password)
    {
        // For production, use BCrypt or ASP.NET Core Identity password hasher
        // This is a simplified version for migration purposes
        return await Task.FromResult(Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(password)));
    }

    public async Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
    {
        try
        {
            // For production, use proper password verification
            // This matches the simple hashing above
            var hash = await HashPasswordAsync(password);
            return hash == hashedPassword || password == hashedPassword;
        }
        catch
        {
            return false;
        }
    }
}
