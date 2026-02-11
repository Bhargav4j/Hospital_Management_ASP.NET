using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.Interfaces.Services;
using HospitalManagement.Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagement.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HospitalDbContext _context;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(HospitalDbContext context, ILogger<AuthenticationService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(bool Success, int UserId, string UserType)> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating login for email: {Email}", email);

            // Check patient
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Email == email && p.IsActive, cancellationToken);

            if (patient != null && await VerifyPasswordAsync(password, patient.Password))
            {
                _logger.LogInformation("Patient login successful: {PatientId}", patient.Id);
                return (true, patient.Id, "Patient");
            }

            // Check doctor
            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.Email == email && d.IsActive, cancellationToken);

            if (doctor != null && await VerifyPasswordAsync(password, doctor.Password))
            {
                _logger.LogInformation("Doctor login successful: {DoctorId}", doctor.Id);
                return (true, doctor.Id, "Doctor");
            }

            _logger.LogWarning("Login failed for email: {Email}", email);
            return (false, 0, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating login for email: {Email}", email);
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

    public async Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
    {
        var hashOfInput = await HashPasswordAsync(password);
        return hashOfInput == hashedPassword || password == hashedPassword;
    }
}
