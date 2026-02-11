using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Services;
using HospitalManagement.Infrastructure.Data;

namespace HospitalManagement.Infrastructure.Services.Tests;

public class AuthenticationServiceTests
{
    private readonly Mock<ILogger<AuthenticationService>> _mockLogger;
    private readonly DbContextOptions<HospitalDbContext> _options;

    public AuthenticationServiceTests()
    {
        _mockLogger = new Mock<ILogger<AuthenticationService>>();
        _options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void AuthenticationService_Constructor_WithNullContext_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AuthenticationService(null!, _mockLogger.Object));
    }

    [Fact]
    public void AuthenticationService_Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AuthenticationService(context, null!));
    }

    [Fact]
    public void AuthenticationService_Constructor_WithValidParameters_CreatesInstance()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var service = new AuthenticationService(context, _mockLogger.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Fact]
    public async Task HashPasswordAsync_WithValidPassword_ReturnsHashedString()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);
        var password = "TestPassword123";

        // Act
        var hashedPassword = await service.HashPasswordAsync(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
        Assert.NotEqual(password, hashedPassword);
    }

    [Fact]
    public async Task HashPasswordAsync_WithSamePassword_ReturnsSameHash()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);
        var password = "TestPassword123";

        // Act
        var hash1 = await service.HashPasswordAsync(password);
        var hash2 = await service.HashPasswordAsync(password);

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public async Task HashPasswordAsync_WithDifferentPasswords_ReturnsDifferentHashes()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        // Act
        var hash1 = await service.HashPasswordAsync("Password1");
        var hash2 = await service.HashPasswordAsync("Password2");

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public async Task HashPasswordAsync_WithEmptyString_ReturnsHash()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        // Act
        var hashedPassword = await service.HashPasswordAsync(string.Empty);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithMatchingPassword_ReturnsTrue()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);
        var password = "TestPassword123";
        var hashedPassword = await service.HashPasswordAsync(password);

        // Act
        var result = await service.VerifyPasswordAsync(password, hashedPassword);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithNonMatchingPassword_ReturnsFalse()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);
        var password = "TestPassword123";
        var wrongPassword = "WrongPassword";
        var hashedPassword = await service.HashPasswordAsync(password);

        // Act
        var result = await service.VerifyPasswordAsync(wrongPassword, hashedPassword);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithPlainTextPassword_ReturnsTrue()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);
        var password = "PlainPassword";

        // Act
        var result = await service.VerifyPasswordAsync(password, password);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithValidPatientCredentials_ReturnsSuccess()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        var password = "PatientPass123";
        var hashedPassword = await service.HashPasswordAsync(password);

        var patient = new Patient
        {
            Id = 1,
            Email = "patient@test.com",
            Password = hashedPassword,
            FirstName = "John",
            LastName = "Doe",
            IsActive = true
        };

        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await service.ValidateLoginAsync("patient@test.com", password);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(1, result.UserId);
        Assert.Equal("Patient", result.UserType);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithValidDoctorCredentials_ReturnsSuccess()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        var password = "DoctorPass123";
        var hashedPassword = await service.HashPasswordAsync(password);

        var doctor = new Doctor
        {
            Id = 2,
            Email = "doctor@test.com",
            Password = hashedPassword,
            FirstName = "Jane",
            LastName = "Smith",
            IsActive = true
        };

        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        var result = await service.ValidateLoginAsync("doctor@test.com", password);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, result.UserId);
        Assert.Equal("Doctor", result.UserType);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithInvalidEmail_ReturnsFailure()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        // Act
        var result = await service.ValidateLoginAsync("nonexistent@test.com", "Password123");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.Equal(string.Empty, result.UserType);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithInvalidPassword_ReturnsFailure()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        var password = "CorrectPassword";
        var hashedPassword = await service.HashPasswordAsync(password);

        var patient = new Patient
        {
            Id = 1,
            Email = "patient@test.com",
            Password = hashedPassword,
            FirstName = "John",
            LastName = "Doe",
            IsActive = true
        };

        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await service.ValidateLoginAsync("patient@test.com", "WrongPassword");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.Equal(string.Empty, result.UserType);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithInactivePatient_ReturnsFailure()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        var password = "PatientPass123";
        var hashedPassword = await service.HashPasswordAsync(password);

        var patient = new Patient
        {
            Id = 1,
            Email = "inactive@test.com",
            Password = hashedPassword,
            FirstName = "Inactive",
            LastName = "User",
            IsActive = false
        };

        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await service.ValidateLoginAsync("inactive@test.com", password);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.Equal(string.Empty, result.UserType);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithInactiveDoctor_ReturnsFailure()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        var password = "DoctorPass123";
        var hashedPassword = await service.HashPasswordAsync(password);

        var doctor = new Doctor
        {
            Id = 2,
            Email = "inactivedoc@test.com",
            Password = hashedPassword,
            FirstName = "Inactive",
            LastName = "Doctor",
            IsActive = false
        };

        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        var result = await service.ValidateLoginAsync("inactivedoc@test.com", password);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.Equal(string.Empty, result.UserType);
    }

    [Fact]
    public async Task ValidateLoginAsync_PrioritizesPatientOverDoctor_WithSameEmail()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        var password = "SharedPass123";
        var hashedPassword = await service.HashPasswordAsync(password);

        var patient = new Patient
        {
            Id = 10,
            Email = "shared@test.com",
            Password = hashedPassword,
            FirstName = "Patient",
            LastName = "User",
            IsActive = true
        };

        var doctor = new Doctor
        {
            Id = 20,
            Email = "shared@test.com",
            Password = hashedPassword,
            FirstName = "Doctor",
            LastName = "User",
            IsActive = true
        };

        context.Patients.Add(patient);
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        var result = await service.ValidateLoginAsync("shared@test.com", password);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(10, result.UserId);
        Assert.Equal("Patient", result.UserType);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithEmptyEmail_ReturnsFailure()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        // Act
        var result = await service.ValidateLoginAsync(string.Empty, "Password123");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.Equal(string.Empty, result.UserType);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithEmptyPassword_ReturnsFailure()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);

        var patient = new Patient
        {
            Id = 1,
            Email = "test@test.com",
            Password = "HashedPassword",
            FirstName = "Test",
            LastName = "User",
            IsActive = true
        };

        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await service.ValidateLoginAsync("test@test.com", string.Empty);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task HashPasswordAsync_WithSpecialCharacters_ReturnsHash()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);
        var password = "P@ssw0rd!#$%";

        // Act
        var hashedPassword = await service.HashPasswordAsync(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
        Assert.NotEqual(password, hashedPassword);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithSpecialCharacters_VerifiesCorrectly()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var service = new AuthenticationService(context, _mockLogger.Object);
        var password = "P@ssw0rd!#$%";
        var hashedPassword = await service.HashPasswordAsync(password);

        // Act
        var result = await service.VerifyPasswordAsync(password, hashedPassword);

        // Assert
        Assert.True(result);
    }
}
