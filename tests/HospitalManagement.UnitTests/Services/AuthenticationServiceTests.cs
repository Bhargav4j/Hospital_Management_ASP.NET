using FluentAssertions;
using HospitalManagement.Application.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HospitalManagement.UnitTests.Services;

public class AuthenticationServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly Mock<ILogger<AuthenticationService>> _loggerMock;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _loggerMock = new Mock<ILogger<AuthenticationService>>();

        _authenticationService = new AuthenticationService(
            _userRepositoryMock.Object,
            _patientRepositoryMock.Object,
            _doctorRepositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
    {
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            Password = "password123",
            UserType = 1,
            IsActive = true
        };

        _userRepositoryMock
            .Setup(x => x.ValidateLoginAsync("test@example.com", "password123", default))
            .ReturnsAsync(user);

        var result = await _authenticationService.LoginAsync("test@example.com", "password123");

        result.Success.Should().BeTrue();
        result.UserId.Should().Be(1);
        result.UserType.Should().Be(1);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ReturnsFailure()
    {
        _userRepositoryMock
            .Setup(x => x.ValidateLoginAsync("test@example.com", "wrongpassword", default))
            .ReturnsAsync((User?)null);

        var result = await _authenticationService.LoginAsync("test@example.com", "wrongpassword");

        result.Success.Should().BeFalse();
        result.UserId.Should().Be(0);
    }
}
