using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Repositories;
using ClinicManagement.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.Services.Tests;

public class AuthenticationServiceTests
{
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly Mock<IStaffRepository> _staffRepositoryMock;
    private readonly Mock<ILogger<AuthenticationService>> _loggerMock;

    public AuthenticationServiceTests()
    {
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _staffRepositoryMock = new Mock<IStaffRepository>();
        _loggerMock = new Mock<ILogger<AuthenticationService>>();
    }

    [Fact]
    public void AuthenticationService_Constructor_ShouldThrowArgumentNullException_WhenPatientRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AuthenticationService(
            null!,
            _doctorRepositoryMock.Object,
            _staffRepositoryMock.Object,
            _loggerMock.Object));
    }

    [Fact]
    public void AuthenticationService_Constructor_ShouldThrowArgumentNullException_WhenDoctorRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AuthenticationService(
            _patientRepositoryMock.Object,
            null!,
            _staffRepositoryMock.Object,
            _loggerMock.Object));
    }

    [Fact]
    public void AuthenticationService_Constructor_ShouldThrowArgumentNullException_WhenStaffRepositoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AuthenticationService(
            _patientRepositoryMock.Object,
            _doctorRepositoryMock.Object,
            null!,
            _loggerMock.Object));
    }

    [Fact]
    public void AuthenticationService_Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AuthenticationService(
            _patientRepositoryMock.Object,
            _doctorRepositoryMock.Object,
            _staffRepositoryMock.Object,
            null!));
    }

    [Fact]
    public async Task HashPasswordAsync_ShouldReturnHashedPassword()
    {
        var service = new AuthenticationService(
            _patientRepositoryMock.Object,
            _doctorRepositoryMock.Object,
            _staffRepositoryMock.Object,
            _loggerMock.Object);

        var result = await service.HashPasswordAsync("testpassword");

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_ShouldReturnTrue_WhenPasswordMatches()
    {
        var service = new AuthenticationService(
            _patientRepositoryMock.Object,
            _doctorRepositoryMock.Object,
            _staffRepositoryMock.Object,
            _loggerMock.Object);

        var password = "testpassword";
        var hash = await service.HashPasswordAsync(password);

        var result = await service.VerifyPasswordAsync(password, hash);

        Assert.True(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_ShouldReturnFalse_WhenPasswordDoesNotMatch()
    {
        var service = new AuthenticationService(
            _patientRepositoryMock.Object,
            _doctorRepositoryMock.Object,
            _staffRepositoryMock.Object,
            _loggerMock.Object);

        var password = "testpassword";
        var hash = await service.HashPasswordAsync("differentpassword");

        var result = await service.VerifyPasswordAsync(password, hash);

        Assert.False(result);
    }

    [Fact]
    public async Task ValidateLoginAsync_ShouldReturnPatient_WhenPatientCredentialsValid()
    {
        var service = new AuthenticationService(
            _patientRepositoryMock.Object,
            _doctorRepositoryMock.Object,
            _staffRepositoryMock.Object,
            _loggerMock.Object);

        var password = "testpassword";
        var hash = await service.HashPasswordAsync(password);
        var patient = new Patient { Id = 1, Email = "patient@test.com", PasswordHash = hash };

        _patientRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var result = await service.ValidateLoginAsync("patient@test.com", password);

        Assert.True(result.IsValid);
        Assert.Equal(UserType.Patient, result.UserType);
        Assert.Equal(1, result.UserId);
    }

    [Fact]
    public async Task ValidateLoginAsync_ShouldReturnFalse_WhenCredentialsInvalid()
    {
        var service = new AuthenticationService(
            _patientRepositoryMock.Object,
            _doctorRepositoryMock.Object,
            _staffRepositoryMock.Object,
            _loggerMock.Object);

        _patientRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        _doctorRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Doctor?)null);

        _staffRepositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Staff?)null);

        var result = await service.ValidateLoginAsync("invalid@test.com", "wrongpassword");

        Assert.False(result.IsValid);
        Assert.Null(result.UserType);
        Assert.Null(result.UserId);
    }
}
