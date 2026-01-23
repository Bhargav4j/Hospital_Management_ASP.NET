using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Application.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Services.Tests;

/// <summary>
/// Unit tests for AuthenticationService
/// </summary>
public class AuthenticationServiceTests
{
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly Mock<ILogger<AuthenticationService>> _mockLogger;

    public AuthenticationServiceTests()
    {
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockDoctorRepository = new Mock<IDoctorRepository>();
        _mockLogger = new Mock<ILogger<AuthenticationService>>();
    }

    [Fact]
    public void AuthenticationService_Constructor_WithNullPatientRepository_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new AuthenticationService(null!, _mockDoctorRepository.Object, _mockLogger.Object));
    }

    [Fact]
    public void AuthenticationService_Constructor_WithNullDoctorRepository_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new AuthenticationService(_mockPatientRepository.Object, null!, _mockLogger.Object));
    }

    [Fact]
    public void AuthenticationService_Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, null!));
    }

    [Fact]
    public async Task ValidateLoginAsync_WithValidPatientCredentials_ShouldReturnSuccess()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);
        var password = "password123";
        var hashedPassword = service.HashPassword(password);

        var patient = new Patient
        {
            Id = 1,
            Email = "patient@test.com",
            PasswordHash = hashedPassword
        };

        _mockPatientRepository.Setup(r => r.GetByEmailAsync("patient@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        // Act
        var result = await service.ValidateLoginAsync("patient@test.com", password);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(1, result.UserId);
        Assert.Equal("Patient", result.UserType);
        Assert.Equal("Login successful", result.Message);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithValidDoctorCredentials_ShouldReturnSuccess()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);
        var password = "doctor123";
        var hashedPassword = service.HashPassword(password);

        var doctor = new Doctor
        {
            Id = 2,
            Email = "doctor@test.com",
            PasswordHash = hashedPassword
        };

        _mockPatientRepository.Setup(r => r.GetByEmailAsync("doctor@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        _mockDoctorRepository.Setup(r => r.GetByEmailAsync("doctor@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctor);

        // Act
        var result = await service.ValidateLoginAsync("doctor@test.com", password);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(2, result.UserId);
        Assert.Equal("Doctor", result.UserType);
        Assert.Equal("Login successful", result.Message);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithInvalidEmail_ShouldReturnFailure()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);

        _mockPatientRepository.Setup(r => r.GetByEmailAsync("invalid@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        _mockDoctorRepository.Setup(r => r.GetByEmailAsync("invalid@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        // Act
        var result = await service.ValidateLoginAsync("invalid@test.com", "password");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.Equal("Email not found", result.Message);
    }

    [Fact]
    public async Task ValidateLoginAsync_WithIncorrectPassword_ShouldReturnFailure()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);
        var correctPassword = "correct123";
        var hashedPassword = service.HashPassword(correctPassword);

        var patient = new Patient
        {
            Id = 1,
            Email = "patient@test.com",
            PasswordHash = hashedPassword
        };

        _mockPatientRepository.Setup(r => r.GetByEmailAsync("patient@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        // Act
        var result = await service.ValidateLoginAsync("patient@test.com", "wrongpassword");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.Equal("Incorrect password", result.Message);
    }

    [Fact]
    public async Task RegisterPatientAsync_WithNewEmail_ShouldReturnSuccess()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);

        _mockPatientRepository.Setup(r => r.GetByEmailAsync("new@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        _mockPatientRepository.Setup(r => r.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient p, CancellationToken ct) => { p.Id = 10; return p; });

        // Act
        var result = await service.RegisterPatientAsync(
            "John Doe",
            new DateTime(1990, 1, 1),
            "new@test.com",
            "password123",
            "1234567890",
            "Male",
            "123 Main St");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(10, result.UserId);
        Assert.Equal("Registration successful", result.Message);
    }

    [Fact]
    public async Task RegisterPatientAsync_WithExistingEmail_ShouldReturnFailure()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);

        var existingPatient = new Patient
        {
            Id = 1,
            Email = "existing@test.com"
        };

        _mockPatientRepository.Setup(r => r.GetByEmailAsync("existing@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPatient);

        // Act
        var result = await service.RegisterPatientAsync(
            "Jane Doe",
            new DateTime(1990, 1, 1),
            "existing@test.com",
            "password123",
            "1234567890",
            "Female",
            "456 Oak St");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.Equal("Email already registered", result.Message);
    }

    [Fact]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);

        // Act
        var result = service.HashPassword("password123");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void HashPassword_WithSameInput_ShouldReturnSameHash()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);

        // Act
        var hash1 = service.HashPassword("password123");
        var hash2 = service.HashPassword("password123");

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void HashPassword_WithDifferentInput_ShouldReturnDifferentHash()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);

        // Act
        var hash1 = service.HashPassword("password123");
        var hash2 = service.HashPassword("password456");

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);
        var password = "password123";
        var hash = service.HashPassword(password);

        // Act
        var result = service.VerifyPassword(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var service = new AuthenticationService(_mockPatientRepository.Object, _mockDoctorRepository.Object, _mockLogger.Object);
        var password = "password123";
        var hash = service.HashPassword(password);

        // Act
        var result = service.VerifyPassword("wrongpassword", hash);

        // Assert
        Assert.False(result);
    }
}
