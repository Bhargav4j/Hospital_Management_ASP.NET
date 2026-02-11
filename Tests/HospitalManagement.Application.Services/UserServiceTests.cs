using AutoMapper;
using HospitalManagement.Application.Services;
using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.HospitalManagement.Application.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<UserService>>();
        _userService = new UserService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnUserDtos()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Name = "User1", Email = "user1@test.com" },
            new User { Id = 2, Name = "User2", Email = "user2@test.com" }
        };

        var userDtos = new List<UserDto>
        {
            new UserDto { Id = 1, Name = "User1", Email = "user1@test.com" },
            new UserDto { Id = 2, Name = "User2", Email = "user2@test.com" }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(users);
        _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUserDto_WhenUserExists()
    {
        // Arrange
        var user = new User { Id = 1, Name = "User1", Email = "user1@test.com" };
        var userDto = new UserDto { Id = 1, Name = "User1", Email = "user1@test.com" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

        // Act
        var result = await _userService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mockRepository.Verify(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnUserDto_WhenEmailDoesNotExist()
    {
        // Arrange
        var userCreateDto = new UserCreateDto
        {
            Name = "New User",
            Email = "new@test.com",
            Password = "password"
        };

        var user = new User { Name = "New User", Email = "new@test.com", Password = "password" };
        var createdUser = new User { Id = 1, Name = "New User", Email = "new@test.com", Password = "password" };
        var userDto = new UserDto { Id = 1, Name = "New User", Email = "new@test.com" };

        _mockRepository.Setup(r => r.EmailExistsAsync(userCreateDto.Email, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<User>(userCreateDto)).Returns(user);
        _mockRepository.Setup(r => r.AddAsync(user, It.IsAny<CancellationToken>())).ReturnsAsync(createdUser);
        _mockMapper.Setup(m => m.Map<UserDto>(createdUser)).Returns(userDto);

        // Act
        var result = await _userService.CreateAsync(userCreateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenEmailExists()
    {
        // Arrange
        var userCreateDto = new UserCreateDto
        {
            Name = "New User",
            Email = "existing@test.com",
            Password = "password"
        };

        _mockRepository.Setup(r => r.EmailExistsAsync(userCreateDto.Email, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateAsync(userCreateDto));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
    {
        // Arrange
        var userUpdateDto = new UserUpdateDto { Name = "Updated Name" };
        var user = new User { Id = 1, Name = "Old Name", Email = "test@test.com" };

        _mockRepository.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map(userUpdateDto, user)).Returns(user);
        _mockRepository.Setup(r => r.UpdateAsync(user, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        await _userService.UpdateAsync(1, userUpdateDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var userUpdateDto = new UserUpdateDto { Name = "Updated Name" };
        _mockRepository.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.UpdateAsync(999, userUpdateDto));
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _mockRepository.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        await _userService.DeleteAsync(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteAsync(999));
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Name = "John Doe", Email = "john@test.com" }
        };

        var userDtos = new List<UserDto>
        {
            new UserDto { Id = 1, Name = "John Doe", Email = "john@test.com" }
        };

        _mockRepository.Setup(r => r.SearchAsync("John", It.IsAny<CancellationToken>())).ReturnsAsync(users);
        _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(users)).Returns(userDtos);

        // Act
        var result = await _userService.SearchAsync("John");

        // Assert
        Assert.Single(result);
        _mockRepository.Verify(r => r.SearchAsync("John", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ValidateLoginAsync_ShouldReturnSuccess_WhenCredentialsCorrect()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "test@test.com", Password = "password" };
        var user = new User { Id = 1, Email = "test@test.com", Password = "password", UserType = UserType.Patient };

        _mockRepository.Setup(r => r.ValidateLoginAsync(loginDto.Email, loginDto.Password, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        // Act
        var result = await _userService.ValidateLoginAsync(loginDto);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(1, result.UserId);
        Assert.Equal(UserType.Patient, result.UserType);
    }

    [Fact]
    public async Task ValidateLoginAsync_ShouldReturnFailure_WhenCredentialsIncorrect()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "test@test.com", Password = "wrongpassword" };

        _mockRepository.Setup(r => r.ValidateLoginAsync(loginDto.Email, loginDto.Password, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);
        _mockRepository.Setup(r => r.EmailExistsAsync(loginDto.Email, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _userService.ValidateLoginAsync(loginDto);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Incorrect password", result.Message);
    }

    [Fact]
    public async Task EmailExistsAsync_ShouldReturnTrue_WhenEmailExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.EmailExistsAsync("test@test.com", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _userService.EmailExistsAsync("test@test.com");

        // Assert
        Assert.True(result);
    }
}
