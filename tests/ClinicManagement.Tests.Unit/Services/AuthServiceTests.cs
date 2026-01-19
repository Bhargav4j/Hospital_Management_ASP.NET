using ClinicManagement.Application.DTOs;
using ClinicManagement.Application.Services;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ClinicManagement.Tests.Unit.Services;

public class AuthServiceTests
{
    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly Mock<ILogger<AuthService>> _loggerMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IRepository<User>>();
        _loggerMock = new Mock<ILogger<AuthService>>();
        _authService = new AuthService(_userRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task RegisterUserAsync_WithNewEmail_ReturnsUserId()
    {
        var createUserDto = new CreateUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password123",
            PhoneNumber = "1234567890",
            Address = "Test Address",
            BirthDate = DateTime.Now.AddYears(-30),
            Gender = "Male",
            Type = 3
        };

        _userRepositoryMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User>());

        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(new User { Id = 1 });

        var result = await _authService.RegisterUserAsync(createUserDto);

        result.Should().Be(1);
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_WithExistingEmail_ReturnsNegativeOne()
    {
        var createUserDto = new CreateUserDto
        {
            Email = "existing@example.com",
            Password = "password123"
        };

        _userRepositoryMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User> { new User { Email = "existing@example.com" } });

        var result = await _authService.RegisterUserAsync(createUserDto);

        result.Should().Be(-1);
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }
}
