using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace ClinicManagement.Infrastructure.Services;

/// <summary>
/// Service for authentication operations
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
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(bool IsValid, UserType? UserType, int? UserId)> ValidateLoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating login for email {Email}", email);

            var patient = await _patientRepository.GetByEmailAsync(email, cancellationToken);
            if (patient != null && await VerifyPasswordAsync(password, patient.PasswordHash))
            {
                _logger.LogInformation("Login successful for patient {Email}", email);
                return (true, UserType.Patient, patient.Id);
            }

            var doctor = await _doctorRepository.GetByEmailAsync(email, cancellationToken);
            if (doctor != null && await VerifyPasswordAsync(password, doctor.PasswordHash))
            {
                _logger.LogInformation("Login successful for doctor {Email}", email);
                return (true, UserType.Doctor, doctor.Id);
            }

            var staff = await _staffRepository.GetByEmailAsync(email, cancellationToken);
            if (staff != null && await VerifyPasswordAsync(password, staff.PasswordHash))
            {
                _logger.LogInformation("Login successful for staff {Email}", email);
                var userType = staff.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase)
                    ? UserType.Admin
                    : UserType.Staff;
                return (true, userType, staff.Id);
            }

            _logger.LogWarning("Login failed for email {Email}", email);
            return (false, null, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating login for email {Email}", email);
            throw;
        }
    }

    public async Task<string> HashPasswordAsync(string password)
    {
        return await Task.Run(() =>
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        });
    }

    public async Task<bool> VerifyPasswordAsync(string password, string passwordHash)
    {
        var computedHash = await HashPasswordAsync(password);
        return computedHash == passwordHash;
    }
}
