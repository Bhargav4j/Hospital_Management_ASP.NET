using Xunit;
using Moq;
using ClinicManagement.Application.Services;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClinicManagement.Tests.Unit.Application.Services;

public class AuthServiceTests
{
    private readonly Mock<IRepository<User>> _mockUserRepository;
    private readonly Mock<ILogger<AuthService>> _mockLogger;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUserRepository = new Mock<IRepository<User>>();
        _mockLogger = new Mock<ILogger<AuthService>>();
        _authService = new AuthService(_mockUserRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnSuccessResponse()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var passwordHash = await _authService.HashPasswordAsync(loginDto.Password);
        var user = new User
        {
            Id = 1,
            Email = loginDto.Email,
            PasswordHash = passwordHash,
            Type = 1
        };

        _mockUserRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User> { user });

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(user.Id, result.UserId);
        Assert.Equal(user.Type, result.UserType);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidEmail_ShouldReturnFailureResponse()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "nonexistent@example.com",
            Password = "password123"
        };

        _mockUserRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User>());

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, result.UserId);
        Assert.Equal(string.Empty, result.Token);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldReturnFailureResponse()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };

        var correctPasswordHash = await _authService.HashPasswordAsync("correctpassword");
        var user = new User
        {
            Id = 1,
            Email = loginDto.Email,
            PasswordHash = correctPasswordHash,
            Type = 1
        };

        _mockUserRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User> { user });

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task LoginAsync_WhenExceptionThrown_ShouldThrowException()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        _mockUserRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(loginDto));
    }

    [Fact]
    public async Task RegisterUserAsync_WithNewEmail_ShouldReturnUserId()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Name = "John Doe",
            Email = "newuser@example.com",
            Password = "password123",
            PhoneNumber = "1234567890",
            Address = "123 Main St",
            BirthDate = new DateTime(1990, 1, 1),
            Gender = "Male",
            Type = 1
        };

        _mockUserRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User>());

        _mockUserRepository
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User u) => { u.Id = 123; return u; });

        // Act
        var result = await _authService.RegisterUserAsync(createUserDto);

        // Assert
        Assert.Equal(123, result);
        _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUserAsync_WithExistingEmail_ShouldReturnNegativeOne()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Email = "existing@example.com",
            Password = "password123"
        };

        var existingUser = new User { Email = createUserDto.Email };

        _mockUserRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User> { existingUser });

        // Act
        var result = await _authService.RegisterUserAsync(createUserDto);

        // Assert
        Assert.Equal(-1, result);
        _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RegisterUserAsync_WhenExceptionThrown_ShouldThrowException()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        _mockUserRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _authService.RegisterUserAsync(createUserDto));
    }

    [Fact]
    public async Task HashPasswordAsync_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = "mypassword123";

        // Act
        var hashedPassword = await _authService.HashPasswordAsync(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
        Assert.NotEqual(password, hashedPassword);
    }

    [Fact]
    public async Task HashPasswordAsync_WithSamePassword_ShouldReturnDifferentHashes()
    {
        // Arrange
        var password = "mypassword123";

        // Act
        var hash1 = await _authService.HashPasswordAsync(password);
        var hash2 = await _authService.HashPasswordAsync(password);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "correctpassword";
        var hashedPassword = await _authService.HashPasswordAsync(password);

        // Act
        var result = await _authService.VerifyPasswordAsync(password, hashedPassword);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var correctPassword = "correctpassword";
        var wrongPassword = "wrongpassword";
        var hashedPassword = await _authService.HashPasswordAsync(correctPassword);

        // Act
        var result = await _authService.VerifyPasswordAsync(wrongPassword, hashedPassword);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task VerifyPasswordAsync_WithInvalidHash_ShouldReturnFalse()
    {
        // Arrange
        var password = "password123";
        var invalidHash = "invalid_hash";

        // Act
        var result = await _authService.VerifyPasswordAsync(password, invalidHash);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("password")]
    [InlineData("P@ssw0rd!")]
    [InlineData("VeryLongPassword123456789")]
    public async Task HashPasswordAsync_ShouldHandleVariousPasswords(string password)
    {
        // Act
        var hashedPassword = await _authService.HashPasswordAsync(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEmpty(hashedPassword);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldSetCreatedAtToUtcNow()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "password123"
        };

        User capturedUser = null!;

        _mockUserRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User>());

        _mockUserRepository
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => capturedUser = u)
            .ReturnsAsync((User u) => { u.Id = 1; return u; });

        // Act
        await _authService.RegisterUserAsync(createUserDto);

        // Assert
        Assert.NotNull(capturedUser);
        Assert.True(capturedUser.CreatedAt <= DateTime.UtcNow);
        Assert.True(capturedUser.CreatedAt >= DateTime.UtcNow.AddSeconds(-5));
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldHashPassword()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "plainpassword"
        };

        User capturedUser = null!;

        _mockUserRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User>());

        _mockUserRepository
            .Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => capturedUser = u)
            .ReturnsAsync((User u) => { u.Id = 1; return u; });

        // Act
        await _authService.RegisterUserAsync(createUserDto);

        // Assert
        Assert.NotNull(capturedUser);
        Assert.NotEqual(createUserDto.Password, capturedUser.PasswordHash);
        Assert.NotEmpty(capturedUser.PasswordHash);
    }

    [Fact]
    public async Task LoginAsync_ShouldGenerateToken()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var passwordHash = await _authService.HashPasswordAsync(loginDto.Password);
        var user = new User
        {
            Id = 99,
            Email = loginDto.Email,
            PasswordHash = passwordHash,
            Type = 2
        };

        _mockUserRepository
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User> { user });

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        Assert.True(result.Success);
        Assert.NotEmpty(result.Token);
        Assert.True(result.Token.Length > 0);
    }
}
